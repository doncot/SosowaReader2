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

namespace SosowaReader.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService contentPageNavigationService;
        public DelegateCommand ContentPageNavigateCommand { get; set; }

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

        private DelegateCommand selectionChangedCommand;
        public DelegateCommand SelectionChangedommand
        {
            get
            {
                return this.selectionChangedCommand = this.selectionChangedCommand ??
                    new DelegateCommand(LoadContentPage);
            }
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

        public void LoadContentPage()
        {

        }

        private List<Work> works = new List<Work>();
        [RestorableState]
        public List<Work> Works
        {
            get { return works; }
            set { SetProperty(ref works, value); }
        }
    }
}
