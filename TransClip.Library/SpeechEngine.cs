using System;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace TransClip.Library
{
    public class SpeechEngine
    {
        public async Task Say(string text)
        {
            if(!string.IsNullOrWhiteSpace(text))
            {
                var media = new MediaElement();

                var synth = new SpeechSynthesizer
                {
                    Voice = SpeechSynthesizer.DefaultVoice
                };
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);

                media.SetSource(stream, stream.ContentType);
                media.Play();
            }
        }
    }
}
