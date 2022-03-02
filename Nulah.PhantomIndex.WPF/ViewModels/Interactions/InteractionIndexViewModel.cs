using Nulah.PhantomIndex.Core.ViewModels;
using Nulah.PhantomIndex.Lib.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.WPF.ViewModels.Interactions
{
    public class InteractionIndexViewModel : ViewModelBase
    {
        private List<EventType> _eventTypes;
        public List<EventType> EventTypes
        {
            get => _eventTypes;
            set => NotifyAndSetPropertyIfChanged(ref _eventTypes, value);
        }
    }
}
