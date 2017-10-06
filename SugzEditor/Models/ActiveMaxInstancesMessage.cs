using SugzEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Models
{
    public class ActiveMaxInstancesMessage
    {
		public SgzMaxInstanceViewModel MaxInstance { get; protected set; }
		public ActiveMaxInstancesMessage(SgzMaxInstanceViewModel maxInstance)
		{
			MaxInstance = maxInstance;
		}
	}
}
