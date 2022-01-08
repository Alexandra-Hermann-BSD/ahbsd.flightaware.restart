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
using System;
using Xunit;
using System.IO;

namespace ahbsd.flightaware.fr24Test
{
    public class StatusContentTest
    {
        [Theory]
        [InlineData("../../../status-fr24feed-2022-01-02.txt", true, true)]
        [InlineData("../../../status-fr24feed-2022-01-02_12:27.txt", true, true)]
        [InlineData("../../../status-fr24feed-2022-01-02_12:28.txt", false, true)]
        [InlineData("../../../status-fr24feed-2022-01-03_0720.txt", true, true)]
        [InlineData("../../../status-fr24feed-2022-01-03_0724.txt", true, true)]
        [InlineData("../../../status-fr24feed-2022-01-03_0727.txt", true, true)]
        [InlineData("../../../status-fr24feed-2022-01-03_0729.txt", false, true)]
        [InlineData("../../../status-fr24feed-now.txt", false, true)]
        public void ImportTest(string fileName, bool critical, bool existingFeederId)
        {
            FileInfo testFile = new FileInfo(fileName);

            Assert.True(testFile.Exists);
            string content = testFile.OpenText().ReadToEnd();
            Assert.NotEqual(string.Empty, content);

        }
    }
}
