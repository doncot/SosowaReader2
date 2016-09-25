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

        public DelegateCommand LoadedCommand
        {
            get
            {
                return DelegateCommand.FromAsyncHandler(Load);
                //return this.refreshCommand = this.refreshCommand ??
                //    DelegateCommand.FromAsyncHandler(Refresh);
                // new DelegateCommand(Refresh); //同期版
            }
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return DelegateCommand.FromAsyncHandler(Refresh);
            }
        }

        private List<Entry> entries;
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

        /// <summary>
        /// データをロードする。（既データがある場合スルーされる）
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            var service = new BrowserService();
            if(Entries == null || Entries.Count == 0)
            {
                Entries = await service.LoadMainPageAsync();
            }
        }

        /// <summary>
        /// データを再ロードする（既データは破棄される）
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            var service = new BrowserService();
            //ロード時に時間がかかるため、即開けておく
            Entries = null;
            Entries = await service.LoadMainPageAsync();
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
