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

    public static class CharacterRecognizer
    {

        public struct Language
        {
            public const string JP = "ja-JP";
            public const string EN = "en-US";
        }

        public static async Task<string> RunOcr(SoftwareBitmap bitmap, string lang)
        {
            OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language(lang));
            if(ocrEngine != null )
            {
                var str = string.Empty;
                var result = await ocrEngine.RecognizeAsync(bitmap);
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
