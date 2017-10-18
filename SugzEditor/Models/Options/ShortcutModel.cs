using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Models.Options
{
    public class ShortcutModel : BaseOptionModel
	{
		public IEnumerable<object> ItemSource { get; set; }

		public ShortcutModel(string category) : base(category)
		{
			
		}
	}
}
