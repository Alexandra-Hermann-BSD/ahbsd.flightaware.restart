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
    /// Enum with defined pi-aware modules.
    /// </summary>
    public enum PiAwareModule
    {
        /// <summary>
        /// An unknown module
        /// </summary>
        unknown,
        /// <summary>
        /// The PiAware master module
        /// </summary>
        /// <example>PiAware master process (piaware) is running with pid 25826.</example>
        piaware,
        /// <summary>
        /// The PiAware default ADS-B client module (1090 MHz)
        /// </summary>
        /// <example>PiAware ADS-B client (faup1090) is running with pid 25876.</example>
        faup1090,
        /// <summary>
        /// The PiAware optional ADS-B client module (978 MHz)
        /// </summary>
        /// <example>PiAware ADS-B UAT client (faup978) is not running (disabled by configuration settings)</example>
        faup978,
        /// <summary>
        /// The PiAware MLAT client module
        /// </summary>
        /// <example>PiAware mlat client (fa-mlat-client) is running with pid 3737.</example>
        fa_mlat_client,
        /// <summary>
        /// The PiAware local ADS-B (1090 MHz) receiver module.
        /// </summary>
        /// <example>Local ADS-B receiver (dump1090-fa) is running with pid 3739.</example>
        dump1090_fa,
        /// <summary>
        /// The PiAware local ADS-B (978 MHz) receiver module.
        /// </summary>
        /// <example>Local ADS-B receiver (dump978-fa) is not running.</example>
        dump978_fa,
    }
}
