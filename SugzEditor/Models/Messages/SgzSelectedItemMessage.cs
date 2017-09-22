using SugzEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Messages
{
    public class SgzSelectedItemMessage
    {
        public SgzDataItem Item { get; private set; }
        public SgzSelectedItemMessage(SgzDataItem item)
        {
            Item = item;
        }
    }
}
