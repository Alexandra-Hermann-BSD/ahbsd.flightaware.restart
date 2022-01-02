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

namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// Static class to convert <see cref="PiAwareModule"/> to string and vice versa.
    /// </summary>
    public static class ConvertPiAwareModule
    {
        /// <summary>
        /// Converts from String to <see cref="PiAwareModule"/>.
        /// </summary>
        /// <param name="module">The given String</param>
        /// <returns>The converted <see cref="PiAwareModule"/></returns>
        public static PiAwareModule FromString(string module)
        {
            string unifiedModule = string.IsNullOrEmpty(module)
                ? module
                : module.Trim().Replace('-', '_').Replace(" ", string.Empty).ToLower();
            if (!Enum.TryParse(unifiedModule, out PiAwareModule result))
            {
                result = PiAwareModule.unknown;
            }

            return result;
        }

        /// <summary>
        /// Converts from <see cref="PiAwareModule"/> to String.
        /// </summary>
        /// <param name="module">The given <see cref="PiAwareModule"/></param>
        /// <returns>The converted String</returns>
        public static string FromModule(PiAwareModule module)
        {
            string result = Enum.GetName(module.GetType(), module);

            return result != null ? result.Replace('_', '-') : result;
        }
    }
}
