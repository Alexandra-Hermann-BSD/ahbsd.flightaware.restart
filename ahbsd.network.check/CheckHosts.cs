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
using System.Net.Mail;
using System.Linq;

namespace ahbsd.network.check
{
    /// <summary>
    /// Class that holds the hosts to check.
    /// </summary>
    public class CheckHosts : Dictionary<CheckArea, IDictionary<string, ICheckIp>> , ICheckHosts
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckHosts()
            : base()
        {
            AddHost(CheckArea.External, NetworkDevices.GlobalAddress);
            AddHost(CheckArea.Local, NetworkDevices.LocalAddress);
        }

        /// <summary>
        /// Constructor with a given area and a host name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        /// <exception cref="ArgumentException">If the area can't be found by name</exception>
        /// <exception cref="ArgumentNullException">If the area was <c>null</c></exception>
        public CheckHosts(string area, string name)
            : base()
        {
            AddHost(area, name);
        }

        /// <summary>
        /// Constructor with a given area and a host name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        public CheckHosts(CheckArea area, string name)
            : base()
        {
            AddHost(area, name);
        }

        #region implementation of ICheckEvents
        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        public void AddHost(CheckArea area, string name)
        {
            ICheckIp tmpCheckIp;
            
            if (!ContainsKey(area))
            {
                Add(area, new Dictionary<string, ICheckIp>());
            }

            IDictionary<string, ICheckIp> areaDict = this[area];
            if (!areaDict.ContainsKey(name))
            {
                tmpCheckIp = new CheckIp(name);
                areaDict.Add(name, tmpCheckIp);
            }
            else
            {
                tmpCheckIp = new CheckIp(name);

                if (!areaDict[name].IpAddresses.Equals(tmpCheckIp.IpAddresses))
                {
                    areaDict[name] = tmpCheckIp;
                }
            }
        }

        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="name">The name of the host</param>
        /// <exception cref="ArgumentException">If the area can't be found by name</exception>
        /// <exception cref="ArgumentNullException">If the area was <c>null</c></exception>
        public void AddHost(string area, string name)
        {
            CheckArea checkArea = Enum.Parse<CheckArea>(area);
            AddHost(checkArea, name);
        }

        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="address">The address of the host</param>
        /// <exception cref="ArgumentNullException">If the given IP-Address is <c>null</c></exception>
        public void AddHost(CheckArea area, IPAddress address)
        {
            ICheckIp tmpCheckIp;
            
            if (!ContainsKey(area))
            {
                Add(area, new Dictionary<string, ICheckIp>());
            }

            string name = Dns.GetHostEntry(address).HostName;
            IDictionary<string, ICheckIp> areaDict = this[area];
            if (!areaDict.ContainsKey(name))
            {
                tmpCheckIp = new CheckIp(address);
                areaDict.Add(name, tmpCheckIp);
            }
            else
            {
                tmpCheckIp = new CheckIp(address);

                if (!areaDict[name].IpAddresses.Equals(tmpCheckIp.IpAddresses))
                {
                    areaDict[name] = tmpCheckIp;
                }
            }
        }

        /// <summary>
        /// Adds a host by name.
        /// </summary>
        /// <param name="area">The area the host is in</param>
        /// <param name="address">The address of the host</param>
        /// <exception cref="ArgumentException">If the area can't be found by name</exception>
        /// <exception cref="ArgumentNullException">If the area or given IPAddress was <c>null</c></exception>
        public void AddHost(string area, IPAddress address)
        {
            CheckArea checkArea = Enum.Parse<CheckArea>(area);
            AddHost(checkArea, address);
        }



        /// <summary>
        /// Gets the first findable <see cref="ICheckIp"/> by it's <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="address">The IPAddress to search for</param>
        /// <returns>The first findable <see cref="ICheckIp"/> or <c>null</c>, if not findable</returns>
        public ICheckIp GetCheckByAddress(IPAddress address)
        {
            ICheckIp result = null;

            foreach (CheckArea area in Keys)
            {
                IDictionary<string, ICheckIp> areaDict = this[area];
                foreach (var entry in areaDict.Where(entry => entry.Value.IpAddresses.Contains(address)))
                {
                    result = entry.Value;
                }

                if (result != null)
                {
                    break;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Gets the first findable <see cref="ICheckIp"/> by it's name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The first findable <see cref="ICheckIp"/> or <c>null</c>, if not findable</returns>
        public ICheckIp GetCheckByName(string name)
        {
            ICheckIp result = null;
            
            foreach (CheckArea area in Keys)
            {
                IDictionary<string, ICheckIp> areaDict = this[area];
                foreach (var entry in areaDict.Where(entry => entry.Key.Equals(name)))
                {
                    result = entry.Value;
                }

                if (result != null)
                {
                    break;
                }
            }
            
            return result;
        }
        #endregion
        
    }
}
