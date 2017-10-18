using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Messages
{
    public class OpenDialogMessage
    {
		public WindowType WindowType { get; private set; }
		public OpenDialogMessage(WindowType windowType)
		{
			WindowType = windowType;
		}
    }
}
