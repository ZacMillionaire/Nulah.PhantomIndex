using Nulah.PhantomIndex.Plugin.RSSReader.Data;
using Nulah.PhantomIndex.Plugin.RSSReader.ViewModels.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nulah.PhantomIndex.Plugin.RSSReader.Pages.Feeds
{
    /// <summary>
    /// Interaction logic for FeedDetails.xaml
    /// </summary>
    public partial class FeedDetails : UserControl
    {
        private readonly FeedDetailsViewModel _viewModel = new();

        private FeedDetails()
        {
            InitializeComponent();
            DataContext = _viewModel;
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            var a = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null,RSSPlugin.Instance.Details.PluginDataLocation,null);

            await WebView.EnsureCoreWebView2Async(a);

            WebView.CoreWebView2.Settings.IsScriptEnabled = false;
        }

        public FeedDetails(string Id) : this()
        {
            _viewModel.Id = Guid.Parse(Id);

            Task.Run(async () =>
            {
                _viewModel.Feed = await RSSPlugin.Instance.Feeder.LoadRssFeed(_viewModel.Id);
                _viewModel.PageEnabled = true;
            });
        }

        private void BackToFeedListButton_Click(object sender, RoutedEventArgs e)
        {
            RSSPlugin.Instance.WindowNavigation.NavigateToPage<FeedList>($"Pages/Feeds/FeedList");
        }

        private void FeedItemGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dc = ((FrameworkElement)sender).DataContext as FeedItem;
            if (dc == null)
            {
                return;
            }

            LoadFeedItemInWebView(dc.Id);
        }

        private async void LoadFeedItemInWebView(string feedItemUrl)
        {
            if (WebView != null && WebView.CoreWebView2 != null)
            {
                WebView.CoreWebView2.Navigate(feedItemUrl);
            }
        }
    }
}
