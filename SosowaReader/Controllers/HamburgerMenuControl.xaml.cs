using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SosowaReader.Controllers
{
    public sealed partial class HamburgerMenuControl : UserControl
    {
        //メニューの展開値
        public static readonly DependencyProperty IsMenuOpenProperty =
        DependencyProperty.RegisterAttached("IsMenuOpen", typeof(bool), typeof(HamburgerMenuControl), new PropertyMetadata(false));
        public bool IsMenuOpen
        {
            get { return (bool)GetValue(IsMenuOpenProperty); }
            set { SetValue(IsMenuOpenProperty, value); }
        }

        public HamburgerMenuControl()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(Object sender, RoutedEventArgs e)
        {
            IsMenuOpen = !IsMenuOpen;
        }
    }
}
