using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nulah.PhantomIndex.WPF.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private double _titleHeight = SystemParameters.CaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight * 4;

        public double TitleHeight
        {
            get => _titleHeight;
            set => NotifyAndSetPropertyIfChanged(ref _titleHeight, value);
        }

        private string _windowTitle = typeof(AppViewModel).Namespace;

        public string WindowTitle
        {
            get => _windowTitle;
            set => NotifyAndSetPropertyIfChanged(ref _windowTitle, value);
        }

    }
}
