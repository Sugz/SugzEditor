using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Settings
{
    public sealed class AppSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool IsDarktheme
        {
            get { return (bool)(this["IsDarktheme"]); }
            set { this["IsDarktheme"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool ShowMenus
        {
            get { return (bool)(this["ShowMenus"]); }
            set { this["ShowMenus"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool ShowToolbar
        {
            get { return (bool)(this["ShowToolbar"]); }
            set { this["ShowToolbar"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool ShowExplorer
        {
            get { return (bool)(this["ShowExplorer"]); }
            set { this["ShowExplorer"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool ShowTabs
        {
            get { return (bool)(this["ShowTabs"]); }
            set { this["ShowTabs"] = value; }
        }
    }
}
