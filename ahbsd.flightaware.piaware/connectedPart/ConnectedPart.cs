﻿//
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
namespace ahbsd.flightaware.piaware.connectedPart
{
    /// <summary>
    /// Abstract class for basics of <see cref="IConnectedPart"/>s
    /// </summary>
    internal abstract class ConnectedPart : Connected, IConnectedPart
    {
        /// <summary>
        /// Constructor for child classes.
        /// </summary>
        /// <param name="module">The given module.</param>
        /// <param name="line">The source text line</param>
        protected ConnectedPart(IModule module, string line)
            : base(module)
        {
            Line = line;
        }

        #region Implementation of IConnectedPart
        /// <summary>
        /// Gets the original line.
        /// </summary>
        /// <value>The original line</value>
        public string Line { get; private set; }
        #endregion
    }
}
