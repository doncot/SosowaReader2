using Prism.Commands;
using Prism.Windows.Mvvm;
using SosowaReader.Models;
using SosowaReader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SosowaReader.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand
        {
            set { searchCommand = value; }
            get
            {
                return new DelegateCommand(async () => await SearchAsync());
            }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private string searchString = "";
        public string SearchString
        {
            get { return searchString; }
            set { SetProperty(ref searchString, value); }
        }

        public async Task SearchAsync()
        {
            try
            {
                IsLoading = true;
                var service = new SosowaBrowseService();
                var query = new EntryQuery
                {
                    SearchString = SearchString
                };
                var result = await service.SearchEntriesAsync(query);
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
        }
    }
}
