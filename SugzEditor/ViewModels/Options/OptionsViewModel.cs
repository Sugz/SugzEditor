using GalaSoft.MvvmLight;
using SugzEditor.Models.Options;
using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
		public ObservableCollection<BaseOptionModel> Items { get; set; } = new ObservableCollection<BaseOptionModel>();

		public OptionsViewModel()
		{
			Items.Add(new BaseOptionModel(SgzConstants.OptionInterface) { IsSelected = true });
			Items.Add(GetShortcuts());
		}

		private BaseOptionModel GetShortcuts()
		{
			BaseOptionModel shortcut = new BaseOptionModel("Shortcuts", null);
			shortcut.Children = new ObservableCollection<BaseOptionModel>()
			{
				new BaseOptionModel(SgzConstants.MenuName_File),
				new BaseOptionModel(SgzConstants.MenuName_Edit),
				new BaseOptionModel(SgzConstants.MenuName_Evaluate),
				new BaseOptionModel(SgzConstants.MenuName_View),
			};
			return shortcut;
		}
	}
}
