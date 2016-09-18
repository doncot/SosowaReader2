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
        //private INavigationService navigationService;
        //public DelegateCommand NavigateCommand { get; set; }

        private DelegateCommand loadedCommand;
        public DelegateCommand LoadedCommand
        {
            get
            {
                return this.loadedCommand = this.loadedCommand ??
                    DelegateCommand.FromAsyncHandler(Refresh);
            }
        }


        public MainPageViewModel(INavigationService navigationService)
        {
            //this.navigationService = navigationService;

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

            //Works = new List<Work> { new Work { Title = "test" } };
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
