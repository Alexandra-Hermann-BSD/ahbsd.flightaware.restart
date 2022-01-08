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
namespace ahbsd.lib.flight
{
    /// <summary>
    /// Basic modules
    /// </summary>
    public enum Module : ushort
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0b0,
        /// <summary>
        /// ADS-B Module
        /// </summary>
        ADS_B = 0b01000,
        /// <summary>
        /// MLAT Module
        /// </summary>
        MLAT = 0b10000,
        /// <summary>
        /// Sender Module
        /// </summary>
        Receiver = 0b0010,
        /// <summary>
        /// Receiver Module
        /// </summary>
        Sender = 0b0001,
        /// <summary>
        /// Receiver and Sender module
        /// </summary>
        SenderReceiver = Receiver | Sender,
    }
}
