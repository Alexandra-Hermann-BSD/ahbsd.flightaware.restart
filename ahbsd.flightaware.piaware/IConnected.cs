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
namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// An interface for connected into.
    /// </summary>
    public interface IConnected
    {
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        PiAwareModule ModuleType { get; }
        /// <summary>
        /// Is the module connected?
        /// </summary>
        /// <value><c>true</c> if connected, otherwise <c>false</c></value>
        bool IsConnected { get; }
        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>The Module</value>
        IModule Module { get; }
    }

    /// <summary>
    /// A generic interface for connected info.
    /// </summary>
    public interface IConnected<CP> : IConnected where CP : connectedPart.IConnectedPart
    {
        /// <summary>
        /// Gets the connected part.
        /// </summary>
        /// <value>The connected part</value>
        CP Part { get; }
    }
}
