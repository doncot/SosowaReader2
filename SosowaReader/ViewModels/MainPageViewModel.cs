using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SosowaReader.Models;
using SosowaReader.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Linq;

namespace SosowaReader.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private DelegateCommand selectionChangedCommand;
        public DelegateCommand SelectionChangedCommand
        {
            set { selectionChangedCommand = value; }
            get
            {
                return new DelegateCommand(NavigateToContentPage);
            }
        }

        private DelegateCommand loadedCommand;
        public DelegateCommand LoadedCommand
        {
            get
            {
                return this.loadedCommand = this.loadedCommand ??
                    DelegateCommand.FromAsyncHandler(Refresh);
                // new DelegateCommand(Refresh); //同期版
            }
        }

        //private DelegateCommand selectionChangedCommand;
        //public DelegateCommand SelectionChangedCommand
        //{
        //    get
        //    {
        //        return this.selectionChangedCommand = this.selectionChangedCommand ??
        //            new DelegateCommand(LoadContentPage);
        //    }
        //}

        private List<Entry> entries = new List<Entry>();
        [RestorableState]
        public List<Entry> Entries
        {
            get { return entries; }
            set { SetProperty(ref entries, value); }
        }

        public Entry SelectedEntry { get; set; }
        public int Index { get; set; }

        private INavigationService NavigationService { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            //ContentPageNavigateCommand = new DelegateCommand(() => navigationService.Navigate("Content", null));
        }

        public async Task Refresh()
        {
            var service = new BrowserService();
            if (Entries?.Count == 0)
            {
                Entries = await service.LoadMainPageAsync();
            }
        }

        public void NavigateToContentPage()
        {
            NavigationService.Navigate("Content", SelectedEntry.Url);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> dictionary)
        {
            base.OnNavigatedTo(e, dictionary);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }


    }
}
