﻿using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

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
