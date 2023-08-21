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
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

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
        private bool ToolBarIsCollapsed { get; set; } = false;
        private bool ClipboardIsChanged { get; set; } = false;

        private readonly SpeechEngine speechEngine = new SpeechEngine();
        private Translator translator { get; set; }
        #endregion

        public HomePage()
        {
            this.InitializeComponent();

            Loaded += HomePage_Loaded;
            SizeChanged += HomePage_SizeChanged;
            Clipboard.ContentChanged += Clipboard_ContentChanged;
            GotFocus += HomePage_GotFocus;
        }

        private void TranslationTextBoxReturn()
        {
            double breakPoint = 64;

           if (ActualWidth >= TranslatingTextBox.MinWidth + TranslatedTextBox.MinWidth + breakPoint
                && TranslationTextBoxIsReturned)
           {
                TranslationTextBoxLayout.ColumnDefinitions.Add(new ColumnDefinition());
                TranslationTextBoxLayout.RowDefinitions.RemoveAt(0);
                
                TranslationTextBoxIsReturned = false;
           }
           else if(ActualWidth < TranslatingTextBox.MinWidth + TranslatedTextBox.MinWidth + breakPoint
                   && !TranslationTextBoxIsReturned)
           {
                TranslationTextBoxLayout.RowDefinitions.Add(new RowDefinition());
                TranslationTextBoxLayout.ColumnDefinitions.RemoveAt(0);

                TranslationTextBoxIsReturned = true;
           }
        }

        private async Task<SoftwareBitmap> GetClipboardImage()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if(dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                RandomAccessStreamReference randomAccessStream = await dataPackageView.GetBitmapAsync();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(await randomAccessStream.OpenReadAsync());
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                Task.WaitAll();

                if(softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8
                                       || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                return softwareBitmap;

            }
            else
            {
                return null;
            }
        }

        private async Task<string> GetClipboardText()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();

            return dataPackageView.Contains(StandardDataFormats.Text) ? await dataPackageView.GetTextAsync() : null;
        }

        private void HomePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TranslationTextBoxReturn();
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            TranslationTextBoxReturn();
        }

        private async Task ExecuteOcr(SoftwareBitmap image)
        {
            var lang = SourceLanguage.SelectedIndex == 0 ? CharacterRecognizer.Language.EN : CharacterRecognizer.Language.JP;

            if(image != null)
            {
                var text = await CharacterRecognizer.RunOcr(image, lang);
                TranslatingTextBox.Text = text;
            }
        }

        private void Clipboard_ContentChanged(object sender, object e)
        {
            ClipboardIsChanged = true;
        }

        private async void HomePage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClipboardIsChanged)
            {
                var image = await GetClipboardImage();
                var text = await GetClipboardText();
                SoftwareBitmapSource source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(image);
                Task.WaitAll();

                SourceImage.Source = source;
                
                if(image != null)
                {
                    await ExecuteOcr(image);
                }
                else if(text != null)
                {
                    TranslatingTextBox.Text = text;
                }
                else
                {
                    TranslatingTextBox.Text = "";
                }

                ClipboardIsChanged = false;
            }
        }

        private void TranslationToggleContainer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TranslationToggle.IsOn = !TranslationToggle.IsOn;
        }

        private async void TranslationButton_Click(object sender, RoutedEventArgs e)
        {
            await speechEngine.Speech(TranslatingTextBox.Text, SpeechEngine.Language.EN);
        }

        private void ToolBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if(ToolBarIsCollapsed)
            {
                ToolBarLayout.RowDefinitions[2].Height = new GridLength(176);
                ToolBarToggleButtonIcon.Glyph = "\xE96e";
                ToolBarIsCollapsed = false;
            }
            else if(!ToolBarIsCollapsed)
            {
                ToolBarLayout.RowDefinitions[2].Height = new GridLength(0);
                ToolBarToggleButtonIcon.Glyph = "\xE96d";
                ToolBarIsCollapsed = true;
            }
        }

        private async void PlayAudioButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            var lang = SourceLanguage.SelectedIndex == 0 ? SpeechEngine.Language.EN : SpeechEngine.Language.JA;

            await speechEngine.Speech(TranslatingTextBox.Text, lang);
        }

        private async void PlayAudioButtonRight_Click(object sender, RoutedEventArgs e)
        {
            var lang = ResultLanguage.SelectedIndex == 0 ? SpeechEngine.Language.EN : SpeechEngine.Language.JA;

            await speechEngine.Speech(TranslatedTextBox.Text, lang);
        }
    }
}
