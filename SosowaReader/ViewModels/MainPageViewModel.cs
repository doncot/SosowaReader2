using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SosowaReader.Models;
using SosowaReader.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                return new DelegateCommand(async () => await LoadAsync());
            }
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () => await RefreshAsync());
            }
        }

        public DelegateCommand GoToSearchPageCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationService.Navigate("Search", null);
                });
            }
        }

        public DelegateCommand GoToPreviousCollectionCommand
        {
            get
            {
                return new DelegateCommand(async () =>
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
                    catch(InvalidOperationException ex)
                    {
                        var dialog = new MessageDialog(ex.Message, "データ展開中に問題が発生しました。");
                        await dialog.ShowAsync();
                    }
                    finally
                    {
                        IsLoading = false;
                    }

                    //選択された並び替えを元に変更
                    ChangeSortType(SortType);
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
                        || x.Author.Contains(searchText) || x.Tags.Any(y=>y.Name.Contains(searchText)));

                    
                });
            }
        }

        public DelegateCommand<String> SortTypeChangeCommand
        {
            get { return new DelegateCommand<String>(ChangeSortType); }
        }

#endregion Commands

        private IEnumerable<Entry> entries;
        //DateTimeがシリアイズできないせいで落ちる
        //[RestorableState]
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

        private SortEnum sortType = SortEnum.UploadDate;
        [RestorableState]
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
        public async Task LoadAsync()
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
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.Message, "通信エラーが発生しました");
                await dialog.ShowAsync();
            }

            catch (InvalidOperationException ex)
            {
                var dialog = new MessageDialog(ex.Message, "データ展開中に問題が発生しました。");
                await dialog.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }

            ChangeSortType(SortType);
        }

        /// <summary>
        /// データを再ロードする（既データは破棄される）
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            //既データは破棄
            Entries = null;

            await LoadAsync();
        }

        private void ChangeSortType(String rawtype)
        {
            //表示
            ChangeSortType((SortEnum)Enum.Parse(typeof(SortEnum), rawtype));
        }

        private void ChangeSortType(SortEnum sortType)
        {
            SortType = sortType;

            switch (SortType)
            {
                case SortEnum.Title:
                    Entries = Entries.OrderBy(x => x.Title);
                    break;

                case SortEnum.Author:
                    Entries = Entries.OrderBy(x => x.Author);
                    break;

                case SortEnum.UploadDate:
                    Entries = Entries.OrderByDescending(x => x.UploadDate);
                    break;

                case SortEnum.Point:
                    Entries = Entries.OrderByDescending(x => x.Points);
                    break;
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
