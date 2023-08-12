using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using TransClip.Library;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TransClip.UI.Pages
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        #region paramater
        private bool TranslationTextBoxIsReturned { get; set; } = false;
        #endregion

        public HomePage()
        {
            this.InitializeComponent();

            Loaded += HomePage_Loaded;
            SizeChanged += HomePage_SizeChanged;
        }

        private void TranslationTextBoxReturn()
        {
           if (ActualWidth >= TranslatingTextBox.MinWidth + TranslatedTextBox.MinWidth + 32
                && TranslationTextBoxIsReturned)
           {
                TranslationTextBoxLayout.ColumnDefinitions.Add(new ColumnDefinition());
                TranslationTextBoxLayout.RowDefinitions.RemoveAt(0);
                
                TranslationTextBoxIsReturned = false;
           }
           else if(ActualWidth < TranslatingTextBox.MinWidth + TranslatedTextBox.MinWidth + 32
                   && !TranslationTextBoxIsReturned)
           {
                TranslationTextBoxLayout.RowDefinitions.Add(new RowDefinition());
                TranslationTextBoxLayout.ColumnDefinitions.RemoveAt(0);

                TranslationTextBoxIsReturned = true;
           }
        }

        private void HomePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TranslationTextBoxReturn();
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            TranslationTextBoxReturn();
        }

        private void TranslationToggleContainer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TranslationToggle.IsOn = !TranslationToggle.IsOn;
        }

        private async void TranslationButton_Click(object sender, RoutedEventArgs e)
        {
            var speechEngine = new SpeechEngine();
            await speechEngine.Speech(TranslatingTextBox.Text, SpeechEngine.Language.JA);
        }
    }
}
