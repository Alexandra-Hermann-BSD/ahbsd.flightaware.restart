// //
// //  Copyright 2022 Alexandra Hermann – Beratung, Software, Design
// //
// //    Licensed under the Apache License, Version 2.0 (the "License");
// //    you may not use this file except in compliance with the License.
// //    You may obtain a copy of the License at
// //
// //        http://www.apache.org/licenses/LICENSE-2.0
// //
// //    Unless required by applicable law or agreed to in writing, software
// //    distributed under the License is distributed on an "AS IS" BASIS,
// //    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //    See the License for the specific language governing permissions and
// //    limitations under the License.

using System;
using System.Collections.Generic;
using Xunit;

namespace ahbsd.network.check.test
{
    public class CheckHostsTest
    {
        [Fact]
        public void TestCheckHosts()
        {
            ICheckHosts checkHosts = new CheckHosts();

            foreach (KeyValuePair<CheckArea,IDictionary<string,ICheckIp>> areaHosts in checkHosts)
            {
                Assert.NotEmpty(areaHosts.Value);

                foreach (KeyValuePair<string,ICheckIp> checkIp in areaHosts.Value)
                {
                    Assert.True(checkIp.Value.TestPing(300));
                }
            }

            checkHosts = new CheckHosts("Local", "fritz.box");
            checkHosts.AddHost(CheckArea.Local, "localhost");
            checkHosts.AddHost(CheckArea.External, "8.8.8.8");
            checkHosts.AddHost("External", "www.amazon.de");
            
            foreach (KeyValuePair<CheckArea,IDictionary<string,ICheckIp>> areaHosts in checkHosts)
            {
                Assert.NotEmpty(areaHosts.Value);

                foreach (KeyValuePair<string,ICheckIp> checkIp in areaHosts.Value)
                {
                    Assert.True(checkIp.Value.TestPing(300));
                }
            }

            checkHosts = new CheckHosts(CheckArea.Local, "127.0.0.1");
            Assert.NotEmpty(checkHosts[CheckArea.Local]);
            
            checkHosts = new CheckHosts(CheckArea.External, "www.google.com");
            Assert.NotEmpty(checkHosts[CheckArea.External]);
            checkHosts = new CheckHosts("External", "www-amazon.com");
            Assert.NotEmpty(checkHosts[CheckArea.External]);
            try
            {
                checkHosts.AddHost("nix", string.Empty);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentException>(e);
            }

            try
            {
                string area = null;
                checkHosts.AddHost(area, "localhost");
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }

        [Theory]
        [InlineData(CheckArea.External, "www.google.com", "www.google.com", 600, true, true)]
        [InlineData(CheckArea.External, "www.google.com", "www.google.de", 600, false, true)]
        [InlineData(CheckArea.External, "amarschzune.fick", "www.google.com", 600, false, false)]
        public void CheckGetCheckByName(CheckArea area, string name, string name2, int timeout, bool expectedResult, bool expectedFind)
        {
            ICheckHosts checkHosts;
            ICheckIp checkIp = null;
            try
            {
                checkHosts = new CheckHosts(area, name);
                checkIp = checkHosts.GetCheckByName(name2);

                if (expectedFind)
                {
                    Assert.NotNull(checkIp);
                }
                else
                {
                    Assert.Null(checkIp);
                }
            }
            catch (Exception e)
            {
                
            }

            if (checkIp != null && expectedFind)
            {
                bool result = checkIp.TestPing(timeout);
                Assert.Equal(expectedResult, result);
            }
            else if (checkIp == null && !expectedFind)
            {
                Assert.False(expectedFind);
            }
        }
    }
}
