using Nulah.PhantomIndex.Plugin.RSSReader.Data.Models;
using Nulah.PhantomIndex.Plugin.RSSReader.ViewModels.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for FeedList.xaml
    /// </summary>
    public partial class FeedList : UserControl
    {
        private readonly FeedListViewModel _viewModel = new();

        private readonly AddFeedViewModel _addFeedViewModel = new()
        {
            PageEnabled = true,
        };

        public FeedList()
        {
            InitializeComponent();

            DataContext = _viewModel;
            AddFeedFormControl.DataContext = _addFeedViewModel;

            Task.Run(async () =>
            {
                await RefreshUIFeeds();
            });
        }

        private void ToggleContentPanelButton_Click(object sender, RoutedEventArgs e)
        {
            AddFeedForm.Visibility = AddFeedForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (AddFeedForm.Visibility == Visibility.Visible)
            {
                ToggleContentPanelButton.Content = "Hide";
            }
            else
            {
                ToggleContentPanelButton.Content = "Show";
            }
        }

        public async Task RefreshUIFeeds()
        {
            _viewModel.PageEnabled = false;
            _viewModel.Feeds = await RSSPlugin.Instance.Feeder.GetFeeds();
            _viewModel.PageEnabled = true;
        }

        private async void AddFeedButton_Click(object sender, RoutedEventArgs e)
        {
            _addFeedViewModel.PageEnabled = false;
            await Task.Run(async () =>
             {
                 var addFeedFormValid = _addFeedViewModel.Validate();
                 if (addFeedFormValid == true)
                 {
                     var feedExists = await RSSPlugin.Instance.Feeder.FeedExistsByUrl(_addFeedViewModel.Url);

                     if (feedExists == false)
                     {
                         if (await RSSPlugin.Instance.Feeder.AddFeed(_addFeedViewModel.Name, _addFeedViewModel.Url))
                         {
                             _addFeedViewModel.Reset();
                             await RefreshUIFeeds();
                         }
                         _addFeedViewModel.PageEnabled = true;
                     }
                     else
                     {
                         _addFeedViewModel.ValidationErrors = new()
                         {
                             "Feed already exists with that URL"
                         };
                         _addFeedViewModel.PageEnabled = true;
                     }
                 }
             });
        }

        private async void RemoveFeedButton_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as Feed;

            if (dc == null)
            {
                return;
            }

            _viewModel.PageEnabled = false;
            await Task.Run(async () =>
            {
                var feedExists = await RSSPlugin.Instance.Feeder.FeedExistsById(dc.Id);

                if (feedExists == true)
                {
                    if (await RSSPlugin.Instance.Feeder.RemoveFeedById(dc.Id))
                    {
                        await RefreshUIFeeds();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove feed");
                    }
                    _viewModel.PageEnabled = true;
                }
                else
                {
                    MessageBox.Show("No feed existed to remove.");
                }
            });
        }

        private async void EditFeedButton_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((FrameworkElement)sender).DataContext as Feed;

            if (dc == null)
            {
                return;
            }

            _viewModel.PageEnabled = false;
            await Task.Run(async () =>
            {
                var feedExists = await RSSPlugin.Instance.Feeder.FeedExistsById(dc.Id);

                if (feedExists == true)
                {
                    //await RSSPlugin.Instance.Feeder.RemoveFeedById(dc.Id);
                    _addFeedViewModel.Url = dc.Url;
                    _addFeedViewModel.Name = dc.Name;
                    _addFeedViewModel.Id = dc.Id;
                    _viewModel.PageEnabled = true;
                }
                else
                {
                    MessageBox.Show("No feed exists to edit.");
                }
            });
        }

        private void FeedItemGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dc = ((FrameworkElement)sender).DataContext as Feed;

            if (dc == null)
            {
                MessageBox.Show("Cannot navigate to feed, context was null");
                return;
            }

            RSSPlugin.Instance.Navigation.NavigateToPage<FeedDetails>($"Pages/Feeds/FeedDetails:{dc.Id}");
        }
    }
}
