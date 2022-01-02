//
//  Copyright 2022  Alexandra Hermann – Beratung, Software, Design
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using ahbsd.flightaware.piaware;
using Xunit;
namespace ahbsd.flightaware.piawareTest
{
    /// <summary>
    /// Test class for the static converter class <see cref="ConvertPiAwareModule"/>.
    /// </summary>
    public class ConvertPiAwareModuleTest
    {
        /// <summary>
        /// Tests the conversion from <see cref="PiAwareModule"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="module">The <see cref="PiAwareModule"/> to convert</param>
        /// <param name="expectedString">The expected conversion result</param>
        [Theory]
        [InlineData(PiAwareModule.dump1090_fa, "dump1090-fa")]
        [InlineData(PiAwareModule.dump978_fa, "dump978-fa")]
        [InlineData(PiAwareModule.faup1090, "faup1090")]
        [InlineData(PiAwareModule.faup978, "faup978")]
        [InlineData(PiAwareModule.fa_mlat_client, "fa-mlat-client")]
        [InlineData(PiAwareModule.piaware, "piaware")]
        [InlineData(PiAwareModule.unknown, "unknown")]
        public void TestConvertFromPiAwareModule(PiAwareModule module, string expectedString)
        {
            string converted = ConvertPiAwareModule.FromModule(module);
            Assert.Equal(expectedString, converted);
        }

        /// <summary>
        /// Test the conversion from <see cref="string"/> to <see cref="PiAwareModule"/>.
        /// </summary>
        /// <param name="module">The string to convert</param>
        /// <param name="expectedModule">The expected conversion result</param>
        [Theory]
        [InlineData("dump1090-fa", PiAwareModule.dump1090_fa)]
        [InlineData("dump1090 - fa", PiAwareModule.dump1090_fa)]
        [InlineData("dump978-fa", PiAwareModule.dump978_fa)]
        [InlineData("DuMp 978 - fa", PiAwareModule.dump978_fa)]
        [InlineData("faup1090", PiAwareModule.faup1090)]
        [InlineData("Faup 1090", PiAwareModule.faup1090)]
        [InlineData("faup978", PiAwareModule.faup978)]
        [InlineData("FAUP9 7 8", PiAwareModule.faup978)]
        [InlineData("PAUP978", PiAwareModule.unknown)]
        [InlineData("fa-mlat-client", PiAwareModule.fa_mlat_client)]
        [InlineData("piaware", PiAwareModule.piaware)]
        [InlineData("unknown", PiAwareModule.unknown)]
        [InlineData("Blödsinn", PiAwareModule.unknown)]
        [InlineData("", PiAwareModule.unknown)]
        [InlineData(null, PiAwareModule.unknown)]
        public void TestConvertFromString(string module, PiAwareModule expectedModule)
        {
            PiAwareModule converted = ConvertPiAwareModule.FromString(module);
            Assert.Equal(expectedModule, converted);
        }
    }
}
