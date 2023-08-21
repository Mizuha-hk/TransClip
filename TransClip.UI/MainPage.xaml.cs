using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using TransClip.UI.Pages;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TransClip.UI
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var coreTitlebar = CoreApplication.GetCurrentView().TitleBar;
            coreTitlebar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(TitleBar);
            ContentFrame.Navigate(typeof(HomePage));

            #region AddEvent
            Loaded += MainPage_Loaded;
            #endregion
        }

        private void NavigatePage(
            Type navPageType, 
            NavigationTransitionInfo transitionInfo)
        {
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            if(navPageType != null && !Type.Equals(preNavPageType, navPageType))
            {
                ContentFrame.Navigate(navPageType, null ,transitionInfo);
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void ViewSourceButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Mizuha-hk/TransClip"));
        }

        private void SideBar_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigatePage(typeof(SettingPage), args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                Type navType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                NavigatePage(navType, args.RecommendedNavigationTransitionInfo);
            }
        }
    }
}
