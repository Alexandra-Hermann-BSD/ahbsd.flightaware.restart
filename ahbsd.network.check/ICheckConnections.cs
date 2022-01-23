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

using System.ComponentModel;

namespace ahbsd.network.check
{
    /// <summary>
    /// Interface for checking connections.
    /// </summary>
    public interface ICheckConnections
    {
        /// <summary>
        /// Gets the hosts to check.
        /// </summary>
        /// <value>The hosts to check</value>
        [ReadOnly(true)]
        ICheckHosts CheckHost { get; }

        /// <summary>
        /// Gets if any of the gateways of this network is reachable or not.
        /// </summary>
        /// <value>Is any of the gateways of this network reachable or not?</value>
        bool IsGatewayReachable { get; }

        /// <summary>
        /// Gets if any of the external Hosts is reachable or not.
        /// </summary>
        /// <value>Is any of the external Hosts reachable or not?</value>
        bool IsAnyExternReachable { get; }
        
        /// <summary>
        /// Gets the status of local connections.
        /// </summary>
        /// <value>The status of local connections</value>
        /// <remarks>Is any of the gateways reachable?</remarks>
        [ReadOnly(true)]
        string LocalConnectionStatus { get; }
        
        /// <summary>
        /// Gets the status of external connection.
        /// </summary>
        /// <value>The status of external connection</value>
        /// <remarks>Is any of the external connections reachable?</remarks>
        [ReadOnly(true)]
        string ExternalConnectionStatus { get; }
    }
}
