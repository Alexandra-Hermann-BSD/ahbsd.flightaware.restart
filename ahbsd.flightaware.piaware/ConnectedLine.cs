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

namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// A simple class to get the parts of a connected / listening text line.
    /// </summary>
    public class ConnectedLine
    {
        /*
        dump1090-fa (pid 3739) is listening for ES connections on port 30005.
        faup1090 is connected to the ADS-B receiver.
        piaware is connected to FlightAware.
        */

        public PiAwareModule ModuleType { get; private set; }

        public ConnectionTarget Target { get; private set; }

        public string Line { get; private set; }

        public ConnectedLine(string line)
        {
            string trimLine = line.Trim();
            if (trimLine.EndsWith('.'))
            {
                int l = trimLine.Length;
                Line = trimLine.Substring(0, l - 1);
            }
            else
            {
                Line = trimLine;
            }

            string[] lineParts = Line.Split(Connected.EMPTY_SPLITTER);
            string firstWord = lineParts.First();
            string lastWord = lineParts.Last();
            ModuleType = ConvertPiAwareModule.FromString(firstWord);

            switch (ModuleType)
            {
                case PiAwareModule.faup1090:
                case PiAwareModule.faup978:
                    string twoLast = $"{lineParts[5]} {lastWord}";
                    Target = ConvertConnectionTarget.FromString(twoLast);
                    break;
                case PiAwareModule.piaware:
                    Target = ConvertConnectionTarget.FromString(lastWord);
                    break;
                default:
                    Target = ConnectionTarget.unknown;
                    break;
            }

            if (ModuleType.Equals(PiAwareModule.dump1090_fa) || ModuleType.Equals(PiAwareModule.dump978_fa))
            {
                IList<string> bracketParts = Line.Split(Connected.BRACKET_SPLITTER).ToList();
            }
        }
    }
}
