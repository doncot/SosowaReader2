using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SosowaReader.ViewModels
{
    public class ContentPageViewModel : ViewModelBase
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> dictionary)
        {
            base.OnNavigatedTo(e, dictionary);

            Text = dictionary["url"] as String;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }
    }
}
