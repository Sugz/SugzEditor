// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Utils;

namespace ICSharpCode.AvalonEdit.Folding
{
	sealed class FoldingMarginMarker : UIElement
	{
		internal VisualLine VisualLine;
		internal FoldingSection FoldingSection;
		
		bool isExpanded;
		
		public bool IsExpanded {
			get { return isExpanded; }
			set {
				if (isExpanded != value) {
					isExpanded = value;
					InvalidateVisual();
				}
				if (FoldingSection != null)
					FoldingSection.IsFolded = !value;
			}
		}


		public FoldingMarginMarker()
		{
			
		}
		
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (!e.Handled) {
				if (e.ChangedButton == MouseButton.Left) {
					IsExpanded = !IsExpanded;
					e.Handled = true;
				}
			}
		}
		
		const double MarginSizeFactor = 0.7;
		
		protected override Size MeasureCore(Size availableSize)
		{
			double size = MarginSizeFactor * FoldingMargin.SizeFactor * (double)GetValue(TextBlock.FontSizeProperty);
			size = PixelSnapHelpers.RoundToOdd(size, PixelSnapHelpers.GetPixelSize(this).Width);
			return new Size(size, size);
		}
		
		protected override void OnRender(DrawingContext drawingContext)
		{
			FoldingMargin margin = VisualParent as FoldingMargin;
			Pen activePen = new Pen(margin.SelectedFoldingMarkerBrush, 1);
			Pen inactivePen = new Pen(margin.FoldingMarkerBrush, 1);
			activePen.StartLineCap = inactivePen.StartLineCap = PenLineCap.Square;
			activePen.EndLineCap = inactivePen.EndLineCap = PenLineCap.Square;
			Size pixelSize = PixelSnapHelpers.GetPixelSize(this);

			Rect rect = new Rect(pixelSize.Width / 2,
								 pixelSize.Height / 2,
								 this.RenderSize.Width - pixelSize.Width,
								 this.RenderSize.Height - pixelSize.Height);

			//Make a transparent rectangle for mouse detection
			drawingContext.DrawRectangle(Brushes.Transparent, null, rect);

			// The point defining the triangle for expanded and collapsed state
			Point start = isExpanded ? new Point(rect.Left, rect.Top + 2) : new Point(rect.Left + 1, rect.Top + 1);
			Point mid = isExpanded ? new Point(rect.Right, rect.Top + 2) : new Point(rect.Left + 1, rect.Bottom + 1);
			Point end = isExpanded ? new Point(rect.Right / 2, rect.Bottom) : new Point(rect.Right - 1, (rect.Bottom / 2) + 1);
			LineSegment[] segments = new LineSegment[]
			{
				new LineSegment(mid, true),
				new LineSegment(end, true)
			};

			// Draw the triangle 
			drawingContext.DrawGeometry(
				IsMouseDirectlyOver ? new SolidColorBrush(Color.FromRgb(175, 175, 175)) : new SolidColorBrush(Color.FromRgb(125, 125, 125)),
				null,
				new PathGeometry(new[] { new PathFigure(start, segments, true) }));

		}
		
		protected override void OnIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsMouseDirectlyOverChanged(e);
			InvalidateVisual();
		}
	}
}
