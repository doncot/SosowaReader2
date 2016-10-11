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
using System;
using System.Net.Http;
using Windows.UI.Popups;
using SosowaReader.Enums;

namespace SosowaReader.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
#region Commands
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

        public DelegateCommand GoToPreviousCollectionCommand
        {
            get
            {
                return DelegateCommand.FromAsyncHandler(async () =>
                {
                    IsLoading = true;
                    Entries = null;
                    try
                    {
                        var browser = new SosowaBrowseService();
                        var id = await browser.GetActiveCollectionNo();
                        Entries = await browser.LoadCollectionAsync(id - 1);
                    }
                    catch (HttpRequestException ex)
                    {
                        var dialog = new MessageDialog(ex.Message, "通信エラーが発生しました");
                        await dialog.ShowAsync();
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                });
            }
        }

        public DelegateCommand TextChangeSearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Entries = Entries.Where(x => x.Title.Contains(searchText) 
                        || x.Author.Contains(searchText));
                });
            }
        }

        public DelegateCommand<String> SortTypeChangeCommand
        {
            get
            {
                return new DelegateCommand<String>((String rawtype) =>
                {
                    SortType = (SortEnum) Enum.Parse(typeof(SortEnum), rawtype);
                });
            }
        }

#endregion Commands

        private IEnumerable<Entry> entries;
        [RestorableState]
        public IEnumerable<Entry> Entries
        {
            get { return entries; }
            set { SetProperty(ref entries, value); }
        }

        public Entry SelectedEntry { get; set; }

        private string searchText = String.Empty;
        [RestorableState]
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private SortEnum sortType;
        public SortEnum SortType
        {
            get { return sortType; }
            set { SetProperty(ref sortType, value); }
        }

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
            try
            {
                IsLoading = true;
                var service = new SosowaBrowseService();
                if (Entries == null)
                {
                    Entries = await service.LoadCollectionAsync();
                }
            }
            catch(HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.Message, "通信エラーが発生しました");
                await dialog.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// データを再ロードする（既データは破棄される）
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                var service = new SosowaBrowseService();
                //ロード時に時間がかかるため、即開けておく
                Entries = null;
                Entries = await service.LoadCollectionAsync();
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.Message, "通信エラーが発生しました");
                await dialog.ShowAsync();
            }
            finally
            {
                IsLoading = false;
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
