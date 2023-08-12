using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace TransClip.Library
{
    public class SpeechEngine
    {
        public struct Language
        {
            public const string EN = "en-US";
            public const string JA = "ja-JP";
        }

        public async Task Speech(string text, string voiceLang)
        {
            if(!string.IsNullOrWhiteSpace(text))
            {
                var media = new MediaElement();

                var speechSynth = new SpeechSynthesizer
                {
                    Voice = SpeechSynthesizer.AllVoices
                        .Where(synth => synth.Language == voiceLang)
                        .Where(synth => synth.Gender == VoiceGender.Female)
                        .First()
                };

                SpeechSynthesisStream stream = await speechSynth.SynthesizeTextToStreamAsync(text);

                media.SetSource(stream, stream.ContentType);
                media.Play();
            }
        }
    }
}
