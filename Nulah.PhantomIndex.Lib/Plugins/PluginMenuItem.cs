using Nulah.PhantomIndex.Core.Controls;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    /// <summary>
    /// Menu item for a plugin
    /// </summary>
    public record PluginMenuItem
    {
        public string DisplayName;
        public FontIcon? Icon;
        public string? PageLocation;

        /// <summary>
        /// Creates a plugin menu item
        /// </summary>
        /// <param name="displayName">Name to display in the menu</param>
        /// <param name="pageLocation">
        /// Location of the user control within the plugin assembly.
        /// <para>
        /// Generally this will follow the same structure as your project layout.
        /// </para>
        /// </param>
        public PluginMenuItem(string displayName, string pageLocation)
        {
            DisplayName = displayName;
            PageLocation = pageLocation;
        }

        /// <summary>
        /// Creates a plugin menu item
        /// </summary>
        /// <param name="displayName">Name to display in the menu</param>
        /// <param name="pageLocation">
        /// Location of the user control within the plugin assembly.
        /// <para>
        /// Generally this will follow the same structure as your project layout.
        /// </para>
        /// </param>
        /// <param name="icon">
        /// Optional icon to display in the menu.
        /// <para>
        /// Refer to <see href="https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-ui-symbol-font"/> for names and icons available.
        /// </para>
        /// </param>
        public PluginMenuItem(string displayName, string pageLocation, FontIcon icon)
        {
            DisplayName = displayName;
            PageLocation = pageLocation;
            Icon = icon;
        }

        /// <summary>
        /// Used for <see cref="PluginMenuCategory"/>
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="icon"></param>
        internal PluginMenuItem(string displayName, FontIcon? icon = null)
        {
            DisplayName = displayName;
            Icon = icon;
        }
    }
}
