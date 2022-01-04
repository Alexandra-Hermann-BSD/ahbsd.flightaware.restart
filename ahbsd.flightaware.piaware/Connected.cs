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
using System.Linq;
using ahbsd.flightaware.piaware.connectedPart;
namespace ahbsd.flightaware.piaware
{
    internal class Connected : IConnected
    {
        protected internal static readonly char[] BRACKET_SPLITTER = new char[2]{ '(', ')' };
        protected internal static readonly char[] EMPTY_SPLITTER = new char[1] { ' ' };

        protected Connected(IModule module)
        {
            Module = module;
        }


        #region implementation of IConnected
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        public PiAwareModule ModuleType => Module.ModuleType;
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

        public static IConnected GetConnected(string line, IStatusContent statusContent=null)
        {
            Connected result = null;
            /*
            dump1090-fa (pid 3739) is listening for ES connections on port 30005.
            faup1090 is connected to the ADS-B receiver.
            piaware is connected to FlightAware.
            */
            if (statusContent != null && !string.IsNullOrEmpty(line))
            {
                PiAwareModule moduleType = ConvertPiAwareModule.FromString(line.Split(EMPTY_SPLITTER).First());
                foreach (var module in statusContent.Modules.Where(module => module.ModuleType.Equals(moduleType)))
                {
                    result = new Connected(module);
                }

                if (result != null)
                {
                    switch (moduleType)
                    {
                        case PiAwareModule.piaware:
                        case PiAwareModule.faup1090:
                        case PiAwareModule.faup978:
                            result.IsConnected = line.Contains("is connected to");
                            break;
                        default:
                            break;
                    }
                }
            }

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

        public static IConnected<CP> GetConnected(string line, IStatusContent statusContent, CP part=default)
        {
            IConnected simpleResult = Connected.GetConnected(line, statusContent);
            IConnected<CP> result = null;

            try
            {
                result = (IConnected<CP>)simpleResult;
            }
            catch (System.Exception)
            {
                result = new Connected<CP>(simpleResult.Module);
            }

            switch (result.ModuleType)
            {
                case PiAwareModule.dump1090_fa:
                case PiAwareModule.dump978_fa:
                    part = (CP)DumpFaConnectedPart.GetDumpFaConnectedPart(line, statusContent);
                    break;
                case PiAwareModule.piaware:
                    break;
                case PiAwareModule.faup1090:
                case PiAwareModule.faup978:
                    break;
                default:
                    break;
            }
            
            ((Connected<CP>)result).Part = part;
            return result;
        }
    }
}
