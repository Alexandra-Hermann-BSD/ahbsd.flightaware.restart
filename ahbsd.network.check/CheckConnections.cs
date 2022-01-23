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
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
namespace ahbsd.network.check
{
    /// <summary>
    /// A Class for checking the current Network and it's connections inside and outside.
    /// </summary>
    public class CheckConnections
        : ICheckConnections
    {
        /// <summary>
        /// The internal list of all current network interfaces of the running machine.
        /// </summary>
        private static IList<NetworkInterface> nics;

        /// <summary>
        /// Static constructor
        /// </summary>
        static CheckConnections()
        {
            nics = NetworkInterface.GetAllNetworkInterfaces().ToList();
            Gateways = null;

            if (nics?.Count > 0)
            {
                foreach (NetworkInterface nic in nics.Where(nic => nic.OperationalStatus == OperationalStatus.Up
                                           && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    IPInterfaceProperties nicProperties = nic.GetIPProperties();
                    AppendGateways(nicProperties.GatewayAddresses);
                }
            }
        }

        /// <summary>
        /// Append Gateways to <see cref="Gateways"/>
        /// </summary>
        /// <param name="gateways">Gateways to append</param>
        private static void AppendGateways(GatewayIPAddressInformationCollection gateways)
        {
            if (Gateways == null)
            {
                Gateways = gateways;
            }
            else if (!Gateways.IsReadOnly)
            {
                foreach (GatewayIPAddressInformation gatewayIpAddressInformation in gateways)
                {
                    _ = Gateways.Append(gatewayIpAddressInformation);
                }
            }
        }
        
        /// <summary>
        /// Gets all the Interfaces of the current running machine.
        /// </summary>
        /// <value>All the Interfaces of the current running machine</value>
        public static IReadOnlyList<NetworkInterface> NetworkInterfaces => (IReadOnlyList<NetworkInterface>) nics;
        /// <summary>
        /// Gets the global Network properties.
        /// </summary>
        /// <value>The global Network properties</value>
        public static IPGlobalProperties GlobalProperties => IPGlobalProperties.GetIPGlobalProperties();
        /// <summary>
        /// Gets the Gateways of the current Network.
        /// </summary>
        /// <value>The Gateways of the current Network</value>
        [ReadOnly(true)]
        public static GatewayIPAddressInformationCollection Gateways { get; private set;}

        #region implementation of ICheckConnections
        /// <summary>
        /// Gets the hosts to check.
        /// </summary>
        /// <value>The hosts to check</value>
        [ReadOnly(true)]
        public ICheckHosts CheckHost { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckConnections()
        {
            CheckHost = new CheckHosts();

            foreach (GatewayIPAddressInformation gateway in Gateways)
            {
                CheckHost.AddHost(CheckArea.Local, gateway.Address);
            }
        }

        /// <summary>
        /// Gets if any of the gateways of this network is reachable or not.
        /// </summary>
        /// <value>Is any of the gateways of this network reachable or not?</value>
        public bool IsGatewayReachable
        {
            get
            {
                IDictionary<IPAddress, bool> gatewaysReachable
                    = new Dictionary<IPAddress, bool>(Gateways.Count);
                foreach ((GatewayIPAddressInformation gateway, ICheckIp tmpCheck) in from GatewayIPAddressInformation gateway in Gateways
                                                    let tmpCheck = CheckHost.GetCheckByAddress(gateway.Address)
                                                    where tmpCheck != null
                                                    select (gateway, tmpCheck))
                {
                    gatewaysReachable.Add(gateway.Address, tmpCheck.TestPing());
                }

                bool result = IsAnyReachable(gatewaysReachable);

                LocalConnectionStatus = result
                    ? Messages.LocalConnectionAvailable
                    : Messages.LocalConnectionNotAvailable;

                return result;
            }
        }

        /// <summary>
        /// Gets if any of the external Hosts is reachable or not.
        /// </summary>
        /// <value>Is any of the external Hosts reachable or not?</value>
        public bool IsAnyExternReachable
        {
            get
            {
                var pairs = CheckHost[CheckArea.External];
                IDictionary<IPAddress, bool> externalReachable
                    = new Dictionary<IPAddress, bool>(pairs.Count);

                foreach (KeyValuePair<string, ICheckIp> pair in pairs)
                {
                    externalReachable.Add(pair.Value.IpAddresses.First(), pair.Value.TestPing(500));
                }
                
                bool result = IsAnyReachable(externalReachable);

                ExternalConnectionStatus = result
                    ? Messages.ExternalConnectionAvailable
                    : Messages.ExternalConnectionNotAvailable;

                return result;
            }
        }

        /// <summary>
        /// Gets the status of local connections.
        /// </summary>
        /// <value>The status of local connections</value>
        /// <remarks>Is any of the gateways reachable?</remarks>
        [ReadOnly(true)]
        public string LocalConnectionStatus { get; private set; }
        
        /// <summary>
        /// Gets the status of external connection.
        /// </summary>
        /// <value>The status of external connection</value>
        /// <remarks>Is any of the external connections reachable?</remarks>
        [ReadOnly(true)]
        public string ExternalConnectionStatus { get; private set;}
        #endregion
        
        /// <summary>
        /// Gets whether there is a single true in values or not.
        /// </summary>
        /// <param name="keyValuePairs">The key value pairs to search in</param>
        /// <returns>Is a single true in values or not?</returns>
        private static bool IsAnyReachable(IDictionary<IPAddress, bool> keyValuePairs)
        {
            bool result = false;
            foreach (KeyValuePair<IPAddress, bool> item in keyValuePairs.Where(item => item.Value))
            {
                result = item.Value;
            }

            return result;
        }
    }
}
