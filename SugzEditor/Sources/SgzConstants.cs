using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Src
{
    public static class SgzConstants
    {
        public static readonly Dictionary<string, string> Commands = new Dictionary<string, string>()
        {
            {"New File", "Ctrl+N" },
            {"Open File", "Ctrl+Shift+N" },
            {"Open Folder", "Ctrl+Shift+F" },
        };

        public static readonly Dictionary<string, string> FileTypes = new Dictionary<string, string>()
        {
            {"Maxscript", ".ms" },
            {"Macroscript", ".mcr" },
            {"Python", ".py" },
            {"CSharp", ".cs" },
        };

        //public static readonly string[] FileTypes = { ".ms", ".mcr", ".py", ".cs" };
    }
}
