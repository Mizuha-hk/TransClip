using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransClip.Library;

namespace TransClip.Test
{
    [TestClass]
    public class LibTest
    {
        [TestMethod]
        public async Task TranslatorTest()
        {
            var translator = new Translator(false, "[InputAuthKey]");
            var text = await translator.TranslateAsync("Hello World", Translator.Language.EN);

            Console.WriteLine(text);
        }
    }
}
