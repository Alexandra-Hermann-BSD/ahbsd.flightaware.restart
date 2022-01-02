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
using System.Collections.Generic;
using System.Linq;

namespace ahbsd.flightaware.piaware.connectedPart
{
    internal class DumpFaConnectedPart : ConnectedPart, IDumpFaConnectedPart
    {
        private DumpFaConnectedPart(IModule module, string line)
            : base(module, line)
        {
        }

        #region implementation of IDumpFaConnectedPart

        /// <summary>
        /// Gets the Port.
        /// </summary>
        /// <value>The Port</value>
        public uint Port { get; private set; }
        #endregion

        /// <summary>
        /// Gets a <see cref="IDumpFaConnectedPart"/> from the given text line and an already existing <see cref="IStatusContent"/>.
        /// </summary>
        /// <param name="line">The given text line</param>
        /// <param name="statusContent">An existing status content</param>
        /// <returns>A IDumpFaConnected Part</returns>
        /// <exception cref="WrongModuleTypeException">If the current Line isn't of <see cref="PiAwareModule.dump1090_fa"/> or <see cref="PiAwareModule.dump978_fa"/>.</exception>
        public static IDumpFaConnectedPart GetDumpFaConnectedPart(string line, IStatusContent statusContent)
        {
            IModule dump1090 = null;
            IModule dump978 = null;
            foreach (var module in statusContent.Modules)
            {
                if (module.ModuleType == PiAwareModule.dump1090_fa)
                {
                    dump1090 = module;
                }

                if (module.ModuleType == PiAwareModule.dump978_fa)
                {
                    dump978 = module;
                }
            }

            IModule currentModule;
            switch (InterpreteModuleType(line))
            {
                case PiAwareModule.dump1090_fa:
                    currentModule = dump1090;
                    break;
                case PiAwareModule.dump978_fa:
                    currentModule = dump978;
                    break;
                default:
                    IList<PiAwareModule> expectedModules = new List<PiAwareModule>();
                    expectedModules.Append(PiAwareModule.dump1090_fa);
                    expectedModules.Append(PiAwareModule.dump978_fa);
                    WrongModuleTypeException wrongModule = new WrongModuleTypeException(expectedModules, InterpreteModuleType(line), line);
                    throw wrongModule;
            }

            return new DumpFaConnectedPart(currentModule, line);
        }

        private static PiAwareModule InterpreteModuleType(string line)
        {
            // dump1090-fa (pid 3739) is listening for ES connections on port 30005.

            string[] parts = line.Split(' ');

            return ConvertPiAwareModule.FromString(parts.First());
        }
    }
}
