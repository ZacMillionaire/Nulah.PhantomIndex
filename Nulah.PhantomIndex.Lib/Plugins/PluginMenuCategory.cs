using Nulah.PhantomIndex.Core.Controls;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    /// <summary>
    /// Used to create sub menus for a plugin.
    /// <para>
    /// If <see cref="PluginMenuCategory.Pages"/> is null, or has no items, the category will not be visible.
    /// </para>
    /// </summary>
    public record PluginMenuCategory : PluginMenuItem
    {
        public List<PluginMenuItem>? Pages;

        /// <summary>
        /// Creates a menu category with the given display name, containing the given <paramref name="subPages"/>.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="subPages">
        /// If null or contains no pages, the category will not be displayed.
        /// </param>
        public PluginMenuCategory(string displayName, List<PluginMenuItem>? subPages = null)
            : base(displayName)
        {
            Pages = subPages;
        }

        /// <summary>
        /// Creates a menu category with the given display name, containing the given <paramref name="subPages"/> with optional <see cref="FontIcon"/>.
        /// <para>
        /// Refer to <see href="https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-ui-symbol-font"/> for names and icons available.
        /// </para>
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="icon">
        /// Optional icon to display in the menu.
        /// <para>
        /// Refer to <see href="https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-ui-symbol-font"/> for names and icons available.
        /// </para>
        /// </param>
        /// <param name="subPages">
        /// If null or contains no pages, the category will not be displayed.
        /// </param>
        public PluginMenuCategory(string displayName, FontIcon icon, List<PluginMenuItem>? subPages = null)
            : base(displayName, icon)
        {
            Pages = subPages;
        }
    }
}
