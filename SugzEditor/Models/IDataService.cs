using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SugzEditor.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
