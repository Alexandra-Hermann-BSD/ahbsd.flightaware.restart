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
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// Interface for status content
    /// </summary>
    public interface IStatusContent
    {
        /// <summary>
        /// Gets a list of Modules.
        /// </summary>
        /// <value>A list of Modules</value>
        [ReadOnly(true)]
        IList<IModule> Modules { get; }
        /// <summary>
        /// Gets the amount of running modules.
        /// </summary>
        /// <value>The amount of running modules</value>
        ushort RunningModules { get; }
        /// <summary>
        /// Gets if something critical is going on. And a restart should be done.
        /// </summary>
        /// <value><c>true</c> if critical, otherwise <c>false</c></value>
        bool Critical { get; }
        /// <summary>
        /// Gets the feeder id.
        /// </summary>
        /// <value>The feeder id</value>
        [ReadOnly(true)]
        Guid FeederId { get; }
        /// <summary>
        /// Gets a list of connected modules.
        /// </summary>
        /// <value>A list of connected modules</value>
        [ReadOnly(true)]
        IList<IConnected<connectedPart.IConnectedPart>> Connected { get; }
        /// <summary>
        /// Gets the <see cref="Uri"/> of the dump module.
        /// </summary>
        /// <value>The Uri of the dump module.</value>
        Uri DumpUri { get; }
    }
}
