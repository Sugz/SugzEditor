using GalaSoft.MvvmLight;
using SugzEditor.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SugzEditor.Models
{
    public interface IPathItem
    {
        /// <summary>
        /// Get if the given path exist
        /// </summary>
        bool IsValidPath { get; }

        /// <summary>
        /// The path of the folder or the file
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The relative path either from 3ds max install or enu folders
        /// </summary>
        string RelativePath { get; }

    }


    public class SgzDataItem : ViewModelBase
    {
        private bool _IsSelected;
        public virtual bool IsSelected
        {
            get => _IsSelected;
            set
            {
                Set(ref _IsSelected, value);
                //if (value)
                //    MessengerInstance.Send(new SgzSelectedItemMessage(this));
            }
        }


        private bool _IsExpanded;
        public virtual bool IsExpanded
        {
            get => _IsExpanded;
            set => Set(ref _IsExpanded, value);
        }


        public virtual string Text { get; set; }


        private object _Icon;
        public object Icon
        {
            get => _Icon;
            set => Set(ref _Icon, value);
        }

    }
}
