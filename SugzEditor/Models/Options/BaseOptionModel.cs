using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Models.Options
{
    public class BaseOptionModel : ObservableObject
    {
		public string Title { get; set; }
		public ObservableCollection<BaseOptionModel> Children { get; set; }
		public string View { get; set; }

		private bool _IsSelected;
		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
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

		public BaseOptionModel(string title)
		{
			Title = title;
			View = title;
		}

		public BaseOptionModel(string title, string view)
		{
			Title = title;
			View = view;
		}
	}
}
