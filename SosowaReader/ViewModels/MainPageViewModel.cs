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

namespace SosowaReader.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand ContentPageNavigateCommand { set; get; }

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
        //public DelegateCommand SelectionChangedommand
        //{
        //    get
        //    {
        //        return this.selectionChangedCommand = this.selectionChangedCommand ??
        //            new DelegateCommand(LoadContentPage);
        //    }
        //}

        private List<Work> works = new List<Work>();
        [RestorableState]
        public List<Work> Works
        {
            get { return works; }
            set { SetProperty(ref works, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
        {
            //これいる？
            //this.contentPageNavigationService = navigationService;
            ContentPageNavigateCommand = new DelegateCommand(() => navigationService.Navigate("Content", null));
            

            //Task.Run(() => Refresh());

            //await Task.Run(async () =>
            //{
            //await CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, Refresh() );
            //});

        }

        public async Task Refresh()
        {
            var service = new BrowserService();
            Works = await service.LoadMainPageAsync();
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> dictionary)
        {
            base.OnNavigatedTo(e, dictionary);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            viewModelState.Add("url", "test");
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }


    }
}
