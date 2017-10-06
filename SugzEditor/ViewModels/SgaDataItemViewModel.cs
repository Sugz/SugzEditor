using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.ViewModels
{
    public class SgaDataItemViewModel : ViewModelBase
    {
        public virtual string Title { get; set; }


        #region IsSelected
        private bool _IsSelected;
        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (value != _IsSelected)
                {
                    Set(ref _IsSelected, value);
                }
            }
        }
        #endregion IsSelected


        #region IsExpanded
        private bool _IsExpanded;
        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanded
        {
            get => _IsExpanded;
            set
            {
                if (value != _IsExpanded)
                {
                    Set(ref _IsExpanded, value);
                }
            }
        }
        #endregion IsExpanded


        #region Icon
        private object _Icon;
        /// <summary>
        /// 
        /// </summary>
        public object Icon
        {
            get => _Icon;
            set
            {
                if (value != _Icon)
                {
                    Set(ref _Icon, value);
                }
            }
        } 
        #endregion Icon

    }
}
