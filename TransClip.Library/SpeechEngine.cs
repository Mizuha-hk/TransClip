using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TransClip.Library
{
    public class SpeechEngine
    {
        public void Say(string text)
        {
            var synthesizer = new SpeechSynthesizer();

            synthesizer.SetOutputToDefaultAudioDevice();

            synthesizer.SpeakAsync(text);
        }
    }
}
