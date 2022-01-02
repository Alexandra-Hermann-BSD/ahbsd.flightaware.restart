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
    /// The line type to pre sort
    /// </summary>
    internal enum LineType
    {
        /// <summary>
        /// unknown line type
        /// </summary>
        Unknown,
        /// <summary>
        /// The line describes a module
        /// </summary>
        Module,
        /// <summary>
        /// The line describes a connection
        /// </summary>
        Connected,
        /// <summary>
        /// The line shows the Dump Uri
        /// </summary>
        DumpUri,
        /// <summary>
        /// The line contains the feeder ID
        /// </summary>
        FeederId,
    }
}
