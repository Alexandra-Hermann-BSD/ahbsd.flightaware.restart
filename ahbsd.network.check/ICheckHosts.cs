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
using System.Net;

namespace ahbsd.network.check
{
    /// <summary>
    /// Interface for hosts to check
    /// </summary>
    public interface ICheckHosts : IDictionary<CheckArea, IDictionary<string, ICheckIp>>
    {
        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        void AddHost(CheckArea area, string name);

        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        /// <exception cref="ArgumentException">If the area can't be found by name</exception>
        /// <exception cref="ArgumentNullException">If the area was <c>null</c></exception>
        void AddHost(string area, string name);
        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="address">The address of the host</param>
        void AddHost(CheckArea area, IPAddress address);

        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="address">The address of the host</param>
        /// <exception cref="ArgumentException">If the area can't be found by name</exception>
        /// <exception cref="ArgumentNullException">If the area was <c>null</c></exception>
        void AddHost(string area, IPAddress address);

        /// <summary>
        /// Gets the first findable <see cref="ICheckIp"/> by it's <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="address">The IPAddress to search for</param>
        /// <returns>The first findable <see cref="ICheckIp"/> or <c>null</c>, if not findable</returns>
        ICheckIp GetCheckByAddress(IPAddress address);

        /// <summary>
        /// Gets the first findable <see cref="ICheckIp"/> by it's name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The first findable <see cref="ICheckIp"/> or <c>null</c>, if not findable</returns>
        ICheckIp GetCheckByName(string name);
    }
}
