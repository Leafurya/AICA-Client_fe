using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomControl.ViewModel
{
    public class LoadingPageArgs : RoutedEventArgs
    {
        public string[] message { get; }
        public LoadingPageArgs(RoutedEvent routedEvent, string[] message) : base(routedEvent)
        {
            this.message = message;
        }
    }
}
