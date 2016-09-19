﻿using Prism.Windows.Mvvm;
using SosowaReader.ViewModels;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace SosowaReader.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : SessionStateAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
