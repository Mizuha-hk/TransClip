
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransClip.Library;

namespace TransClip.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var synth = new SpeechEngine();
            synth.Say("English");
        }
    }
}
