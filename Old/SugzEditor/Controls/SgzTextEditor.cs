using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using SugzEditor.Models;
using SugzEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace SugzEditor.Controls
{
    public class SgzTextEditor : TextEditor
    {
        MaxscriptFoldingStrategy foldingStrategy = new MaxscriptFoldingStrategy();
        FoldingManager foldingManager;


        public SgzTextEditor()
        {
            SetSyntaxHighlighting();
            Loaded += delegate { SgzTextEditor_Loaded(); };

        }

        private void SgzTextEditor_Loaded()
        {
            SetFolding();
            MouseMove += OnMouseMove;
        }


        private void SetSyntaxHighlighting()
        {
            using (Stream s = typeof(SgzTextEditor).Assembly.GetManifestResourceStream("SugzEditor.Resources.Maxscript.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                    SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }


        public void SetFolding()
        {
            if (SyntaxHighlighting == null)
                foldingStrategy = null;
            else
            {
                if (Document is null)
                    Document = new TextDocument();
                if (Document != null)
                {
                    if (foldingManager == null)
                        foldingManager = FoldingManager.Install(TextArea);

                    foldingStrategy.UpdateFoldings(foldingManager, Document);
                }
            }
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is FoldingMargin foldingMargin)
                foldingStrategy.UpdateFoldings(foldingManager, Document);
        }
    }
}
