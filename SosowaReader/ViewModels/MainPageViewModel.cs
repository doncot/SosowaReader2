using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SosowaReader.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SosowaReader.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        //private INavigationService navigationService;
        //public DelegateCommand NavigateCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
        {
            //this.navigationService = navigationService;

            Works.Add(new Work { Title = "terse", Author = "ロリっ娘" });
        }

        private ObservableCollection<Work> works = new ObservableCollection<Work>();
        [RestorableState]
        public ObservableCollection<Work> Works
        {
            get { return works; }
            set { SetProperty(ref works, value); }
        }

        public string TestString { get { return "Can you see me?"; } }
    }
}
