using Prism.Windows.Mvvm;
using SosowaReader.ViewModels;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace SosowaReader.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : SessionStateAwarePage
    {
        //private MainPageViewModel vm;

        public MainPage()
        {
            this.InitializeComponent();

            //vm = DataContext as MainPageViewModel;
        }

        //private async void SessionStateAwarePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    await vm.Refresh();
        //}
    }
}
