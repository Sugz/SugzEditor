using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.AvalonEdit.AddIn
{
    public sealed class FolderService : DocumentColorizingTransformer, IBackgroundRenderer, IFolderMarkerService
    {
        readonly ICodeEditor codeEditor;
        ParserFoldingStrategy service;
        ITextEditorProvider provider;
        TextSegmentCollection<FolderMarker> markers;
        Color lineColor;
        Color activeLineColor;

        #region Brushes

        /// <summary>
        /// Gets/sets the Brush used for displaying the snap lines of folding.
        /// </summary>
        public Brush SnapLinesBrush
        {
            get { return new SolidColorBrush(lineColor); }
            set {
                lineColor = (value as SolidColorBrush).Color;
                Redraw();
            }
        }

        /// <summary>
        /// Gets/sets the Brush used for displaying the snap line of active folding.
        /// </summary>
        public Brush ActiveSnapLineBrush
        {
            get { return new SolidColorBrush(activeLineColor); }
            set {
                activeLineColor = (value as SolidColorBrush).Color;
                Redraw();
            }
        }

        #endregion

        public FolderService(ICodeEditor codeEditor)
        {
            instance = this;
            if (codeEditor == null)
                throw new ArgumentNullException("codeEditor");
            this.codeEditor = codeEditor;
            codeEditor.DocumentChanged += codeEditor_DocumentChanged;
            codeEditor_DocumentChanged(null, null);

            //lineColor = Colors.Gray;
            //activeLineColor = Colors.Yellow;

            var sdCustom = Environment.GetEnvironmentVariable("SDCUSTOM");

            if (sdCustom != null && sdCustom.Equals("1")) {
                //lineColor = Colors.Yellow;
                //activeLineColor = Colors.Red;
            }
        }

        private static FolderService instance=null;
        public static FolderService Current { get { return instance; } }

        void codeEditor_DocumentChanged(object sender, EventArgs e)
        {
            if (markers != null) {
                foreach (FolderMarker m in markers.ToArray()) {
                    m.Delete();
                }
            }
            if (codeEditor.Document == null)
                markers = null;
            else
                markers = new TextSegmentCollection<FolderMarker>(codeEditor.Document);
        }

        protected override void ColorizeLine(DocumentLine line)
        {
			if (markers == null)
				return;
			int lineStart = line.Offset;
			int lineEnd = lineStart + line.Length;
			foreach (FolderMarker marker in markers.FindOverlappingSegments(lineStart, line.Length)) {
				Brush foregroundBrush = null;
				ChangeLinePart(
					Math.Max(marker.StartOffset, lineStart),
					Math.Min(marker.EndOffset, lineEnd),
					element => {
						if (foregroundBrush != null) {
							element.TextRunProperties.SetForegroundBrush(foregroundBrush);
						}
					}
				);
			}
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        public void Draw(TextView textView, System.Windows.Media.DrawingContext drawingContext)
        {
            if (service == null) {
                provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorProvider;
                if (provider == null || provider.TextEditor == null) return;
                service = provider.TextEditor.GetService(typeof(ParserFoldingStrategy)) as ParserFoldingStrategy;
                provider.TextEditor.Caret.PositionChanged += new EventHandler(Caret_PositionChanged);
            }

            if (textView == null)
                throw new ArgumentNullException("textView");
            if (drawingContext == null)
                throw new ArgumentNullException("drawingContext");
            if (markers == null || !textView.VisualLinesValid)
                return;
            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
                return;
            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

            int currentOffset = provider.TextEditor.Caret.Offset;

            FolderMarker contMarker = null;

            try {
                contMarker = GetMarkerContaining(currentOffset, viewStart, viewEnd);
            } catch {
                contMarker = null;
            }

            foreach (FolderMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart)) {

                //var folderOffset = service.FoldingManager.GetNextFoldedFoldingStart(marker.StartOffset);
                //if (folderOffset != -1 && folderOffset >= marker.StartOffset && folderOffset <= marker.EndOffset) continue;

                bool goNext = false;
                var folders = service.FoldingManager.GetFoldingsContaining(marker.StartOffset);
                foreach (var folder in folders) {
                    if (folder.IsFolded) {
                        goNext = true;
                        break;
                    }
                }
                if (goNext) continue;

                var activeMarker = marker;
                if (contMarker != null) activeMarker = contMarker;

                var startOffset = activeMarker.StartOffset;
                var endOffset = activeMarker.EndOffset;

                if (startOffset < 0) startOffset = 0;
                if (startOffset > provider.TextEditor.Document.TextLength) startOffset = provider.TextEditor.Document.TextLength;

                if (endOffset < 0) endOffset = 0;
                if (endOffset > provider.TextEditor.Document.TextLength) endOffset = provider.TextEditor.Document.TextLength;

                var firstLine = provider.TextEditor.Document.GetLineForOffset(startOffset);

                IDocumentLine lastLine = null;

                try {
                    lastLine = provider.TextEditor.Document.GetLineForOffset(endOffset);
                } catch (ArgumentOutOfRangeException) {
                    lastLine = provider.TextEditor.Document.GetLine(provider.TextEditor.Document.TotalNumberOfLines);
                }

                int firstLineStartOffset = activeMarker.StartOffset;
                int firstLineEndOffset = firstLine.EndOffset;
                int lastLineStartOffset = lastLine.Offset + lastLine.Text.IndexOf(lastLine.Text.Trim());
                int lastLineEndOffset = lastLine.EndOffset;

                var color = lineColor;
                if (marker == contMarker && currentOffset >= firstLineStartOffset && currentOffset <= lastLineEndOffset) color = activeLineColor;

                Pen usedPen = new Pen(new SolidColorBrush(color), 1);
                usedPen.DashStyle = DashStyles.Dot;
                usedPen.Freeze();

                Point start = new Point();
                Point end = new Point();
                CalculatePoints(textView, marker, out start, out end);

                drawingContext.DrawLine(usedPen, start, end);
            }
        }

        void Caret_PositionChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private FolderMarker GetMarkerContaining(int offset, int viewStart, int viewEnd)
        {
            int delta = int.MaxValue;
            FolderMarker retMarker = null;

            foreach (var marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart)) {
                int startOffset = marker.StartOffset;
                int endOffset = provider.TextEditor.Document.GetLineForOffset(marker.EndOffset).EndOffset;
                if (startOffset > offset) break;
                if (endOffset < offset) continue;
                if ((offset - startOffset) < delta) {
                    delta = offset - startOffset;
                    retMarker = marker;
                }
            }

            return retMarker;
        }

        private void CalculatePoints(TextView textView, FolderMarker marker, out Point start, out Point end)
        {
            Vector scrollOffset = textView.ScrollOffset;
            int segmentStart = marker.StartOffset;
            int segmentEnd = marker.EndOffset;

            segmentStart = CoerceValue(segmentStart, 0, textView.Document.TextLength);
            segmentEnd = CoerceValue(segmentEnd, 0, textView.Document.TextLength);

            var startPosition = new TextViewPosition(textView.Document.GetLocation(segmentStart));
            var endPosition = new TextViewPosition(textView.Document.GetLocation(segmentEnd));

            //startPosition.Column += 1;
            //endPosition.Column += 1;

            var startPoint = textView.GetVisualPosition(startPosition, VisualYPosition.LineBottom);
            var endPoint = textView.GetVisualPosition(endPosition, VisualYPosition.LineTop);

            // Disegno la snap line a inizio carattere, e non a metà
            //start = new Point(startPoint.X - scrollOffset.X + (textView.WideSpaceWidth / 2), startPoint.Y - scrollOffset.Y);
            //end = new Point(startPoint.X - scrollOffset.X + (textView.WideSpaceWidth / 2), endPoint.Y - scrollOffset.Y);
            start = new Point(startPoint.X - scrollOffset.X , startPoint.Y - scrollOffset.Y);
            end = new Point(startPoint.X - scrollOffset.X , endPoint.Y - scrollOffset.Y);
        }

        private int CoerceValue(int value, int minimum, int maximum)
        {
            return Math.Max(Math.Min(value, maximum), minimum);
        }

        IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
                yield return new Point(start.X + i * offset, start.Y - ((i + 1) % 2 == 0 ? offset : 0));
        }

        private void Redraw()
        {
            if (markers != null) {
                foreach (FolderMarker m in markers.ToArray()) {
                    Redraw(m);
                }
            }
        }

        internal void Redraw(ISegment segment)
        {
            codeEditor.Redraw(segment, DispatcherPriority.Normal);
        }

        public IFolderMarker Create(int startOffset, int endOffset)
        {
            if (markers == null)
                return null;
                //throw new InvalidOperationException("Cannot create a marker when not attached to a document");

            int textLength = codeEditor.Document.TextLength;
            if (startOffset < 0 || startOffset > textLength)
                return null;
                //throw new ArgumentOutOfRangeException("startOffset", startOffset, "Value must be between 0 and " + textLength);
            if (endOffset < 0 || endOffset > textLength)
                return null;
                //throw new ArgumentOutOfRangeException("endOffset", endOffset, "Value must be between 0 and " + textLength);

            FolderMarker m = new FolderMarker(this, startOffset, endOffset);
            markers.Add(m);
            // no need to mark segment for redraw: the text marker is invisible until a property is set
            return m;
        }

        public IEnumerable<IFolderMarker> GetMarkersAtOffset(int offset)
        {
            if (markers == null)
                return Enumerable.Empty<IFolderMarker>();
            else
                return markers.FindSegmentsContaining(offset);
        }

        public IEnumerable<IFolderMarker> TextMarkers
        {
            get { return markers ?? Enumerable.Empty<IFolderMarker>(); }
        }

        public void Remove(IFolderMarker marker)
        {
            if (marker == null)
                throw new ArgumentNullException("marker");
            FolderMarker m = marker as FolderMarker;
            if (markers != null && markers.Remove(m)) {
                Redraw(m);
                m.OnDeleted();
            }
        }

        public void RemoveAll()
        {
            if (markers != null) markers.Clear();
        }

        public void RemoveAll(Predicate<IFolderMarker> predicate)
        {
            if (markers != null) {
                foreach (FolderMarker m in markers.ToArray()) {
                    if (predicate == null || predicate(m))
                        Remove(m);
                }
            }
        }
    }

    public sealed class FolderMarker : TextSegment, IFolderMarker
    {
        readonly FolderService service;

        public FolderMarker(FolderService service, int startOffset, int endOffset)
		{
			if (service == null)
				throw new ArgumentNullException("service");
			this.service = service;
			this.StartOffset = startOffset;
            this.EndOffset = endOffset;
		}
		
		public event EventHandler Deleted;
		
		public bool IsDeleted {
			get { return !this.IsConnectedToCollection; }
		}
		
		public void Delete()
		{
			service.Remove(this);
		}
		
		internal void OnDeleted()
		{
			if (Deleted != null)
				Deleted(this, EventArgs.Empty);
		}
		
		void Redraw()
		{
			service.Redraw(this);
		}

		public object Tag { get; set; }
    }
}