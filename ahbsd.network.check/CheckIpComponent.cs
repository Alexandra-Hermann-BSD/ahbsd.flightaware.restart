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

using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ahbsd.network.check
{
    /// <summary>
    /// <see cref="ICheckIp"/> as Component
    /// </summary>
    public class CheckIpComponent : Component, ICheckIp
    {
        /// <summary>
        /// The internal checker.
        /// </summary>
        private readonly ICheckIp checker;

        /// <summary>
        /// Constructor with an IP or Uri to check.
        /// </summary>
        /// <param name="address">An IP or Uri to check</param>
        public CheckIpComponent(string address)
            : base() =>
            checker = new CheckIp(address);

        /// <summary>
        /// Constructor with a parent container and an IP or Uri to check.
        /// </summary>
        /// <param name="parent">The parent container</param>
        /// <param name="address">An IP or Uri to check</param>
        public CheckIpComponent(IContainer parent, string address)
            : base()
        {
            checker = new CheckIp(address);
            parent?.Add(this);
        }

        #region implementation of ICheckIp
        /// <summary>
        /// Gets the IP-Address.
        /// </summary>
        /// <value>The IP-Address</value>
        public IList<IPAddress> IpAddresses => checker.IpAddresses;

        /// <summary>
        /// Checks if the given IP is available.
        /// </summary>
        /// <param name="timeout">[optional] the wished timeout in ms, by default 100ms are selected</param>
        /// <returns><c>true</c> if the given IP is reachable, otherwise <c>false</c></returns>
        public bool TestPing(int timeout = 100) => checker.TestPing(timeout);
        #endregion
    }
}
