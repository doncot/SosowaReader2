using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Prism.Unity.Windows;
using Prism.Windows;
using Windows.UI.Xaml.Data;
using Prism.Mvvm;
using SosowaReader.Views;
using SosowaReader.ViewModels;

namespace SosowaReader
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : PrismApplication
    {
        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    this.DebugSettings.IsTextPerformanceVisualizationEnabled = true;
            //}
#endif

            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            ViewModelLocationProvider.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(NavigationService));

            return base.OnInitializeAsync(args);
        }
    }
}
