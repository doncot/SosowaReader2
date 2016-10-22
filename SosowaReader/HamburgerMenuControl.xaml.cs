using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SosowaReader
{
    public sealed partial class HamburgerMenuControl : UserControl
    {
        //メニューの展開値
        public static readonly DependencyProperty IsMenuOpenProperty =
        DependencyProperty.RegisterAttached("IsMenuOpen", typeof(bool),typeof(HamburgerMenuControl),new PropertyMetadata(false));
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
