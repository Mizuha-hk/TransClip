
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransClip.Library;

namespace TransClip.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            var synth = new SpeechEngine();
            await synth.Say("English");
        }
    }
}
