using Nulah.PhantomIndex.WPF.ViewModels.Profiles;
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

namespace Nulah.PhantomIndex.WPF.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {
        private ProfileIndexViewModel _viewModel = new();

        public Index()
        {
            InitializeComponent();
            DataContext = _viewModel;

            Task.Run(async () =>
            {
                _viewModel.PageEnabled = false;

                _viewModel.TotalProfiles = await App.Database.Profiles.GetProfileCount();

                _viewModel.Profiles = (await App.Database.Profiles.GetProfiles())
                .Select(x => new ProfileInfoShort
                {
                    Id = x.Id,
                    ProfileName = x.Name,
                    DisplayName = string.IsNullOrWhiteSpace(x.DisplayLastName)
                        ? $"{x.DisplayFirstName}"
                        : $"{x.DisplayFirstName} {x.DisplayLastName}",
                    Pronouns = x.Pronouns,
                    ImageBlob = x.ImageBlob,
                    Created = x.CreatedUtc.ToLocalTime()
                })
                .ToList();

                _viewModel.PageEnabled = true;
            });
        }

        public Index(string viewParameters) : this()
        {
            _viewModel.PageName = viewParameters;
        }

        private void ProfileShort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border profileShort && profileShort.DataContext is ProfileInfoShort profileInfo)
            {
                //MainWindow.Navigation.NavigateToPage($"Pages/Profiles/ViewProfile:{profileInfo.Id}");
            }
        }
    }

    // https://stackoverflow.com/questions/27641606/loading-a-large-amount-of-images-to-be-displayed-in-a-wrappanel/27865101#27865101
    public sealed class LazyScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty IsInViewportProperty =
            DependencyProperty.RegisterAttached("IsInViewport", typeof(bool), typeof(LazyScrollViewer));

        public static bool GetIsInViewport(UIElement element)
        {
            return (bool)element.GetValue(IsInViewportProperty);
        }

        public static void SetIsInViewport(UIElement element, bool value)
        {
            element.SetValue(IsInViewportProperty, value);
        }

        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            base.OnScrollChanged(e);

            var panel = Content as Panel;
            if (panel == null)
            {
                return;
            }

            Rect viewport = new Rect(new Point(0, 0), RenderSize);

            foreach (UIElement child in panel.Children)
            {
                if (!child.IsVisible)
                {
                    SetIsInViewport(child, false);
                    continue;
                }

                GeneralTransform transform = child.TransformToAncestor(this);
                Rect childBounds = transform.TransformBounds(new Rect(new Point(0, 0), child.RenderSize));
                SetIsInViewport(child, viewport.IntersectsWith(childBounds));
            }
        }
    }
}
