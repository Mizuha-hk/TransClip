using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.UI.Xaml.Shapes;

namespace TransClip.Library
{
    public struct Language
    {
        public const string JP = "ja-JP";
        public const string EN = "en-US";
    }

    public static class CharcterRecognizer
    {
        public static async Task<string> RunOcr(SoftwareBitmap bitmap, string lang)
        {
            OcrEngine ocrEngin = OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language(lang));
            if(ocrEngin != null )
            {
                var str = string.Empty;
                var result = await ocrEngin.RecognizeAsync(bitmap);
                foreach(var line in result.Lines)
                {
                    str += line + "\n";
                }
                return str;
            }

            return null;
        }
    }
}
