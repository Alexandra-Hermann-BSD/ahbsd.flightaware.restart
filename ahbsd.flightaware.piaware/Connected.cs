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
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using ahbsd.flightaware.piaware.connectedPart;
namespace ahbsd.flightaware.piaware
{
    internal abstract class Connected
    {
        protected Connected(IModule module)
        {
            Module = module;
            ModuleType = module.ModuleType;
            IsConnected = false;
        }

        #region implementation of IConnected
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        public PiAwareModule ModuleType { get; private set; }
        /// <summary>
        /// Is the module connected?
        /// </summary>
        /// <value><c>true</c> if connected, otherwise <c>false</c></value>
        public bool IsConnected { get; protected set; }
        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>The Module</value>
        public IModule Module { get; private set; }
        #endregion

        public static IConnected GetConnected(string line)
        {
            IConnected result = null;


            return result;
        }
    }

    internal class Connected<CP> : Connected, IConnected<CP> where CP : IConnectedPart
    {
        protected Connected(IModule module)
            : base(module)
        {
            IsConnected = false;
        }

        #region implementation of IConnected<CP>
        /// <summary>
        /// Gets the connected part.
        /// </summary>
        /// <value>The connected part</value>
        public CP Part { get; protected internal set; }
        #endregion

        public static new IConnected<CP> GetConnected(string line)
        {
            IConnected<CP> result = (IConnected<CP>)Connected.GetConnected(line);
            IConnectedPart part;

            switch (result.ModuleType)
            {
                case PiAwareModule.dump1090_fa:
                case PiAwareModule.dump978_fa:
                    part = (IDumpFaConnectedPart)null;
                    break;
                case PiAwareModule.piaware:
                    break;
                case PiAwareModule.faup1090:
                    break;
                case PiAwareModule.faup978:
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
