using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Reflection;
using SosowaReader.Models;
using SosowaReader.Services;

namespace SosowaReader.ViewModels
{
    public class ContentPageViewModel : ViewModelBase
    {
        private string contentBody;
        public string ContentBody
        {
            get { return contentBody; }
            set { SetProperty(ref contentBody, value); }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> dictionary)
        {
            base.OnNavigatedTo(e, dictionary);

            var url = e.Parameter as String;
            if (!String.IsNullOrEmpty(url))
            {
                //本文をロード
                var service = new BrowserService();
                Entry entry = new Entry();
                entry = await service.LoadContentAsync(url);
                ContentBody = entry.Content;

            }
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }
    }
}
