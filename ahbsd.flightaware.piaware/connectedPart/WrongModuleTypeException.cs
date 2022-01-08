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
using System.Runtime.Serialization;
using System.Text;

namespace ahbsd.flightaware.piaware.connectedPart
{
    [Serializable]
    public class WrongModuleTypeException : Exception
    {
        protected WrongModuleTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        public WrongModuleTypeException(PiAwareModule expected, PiAwareModule given, string line)
            : base($"The given line is not a '{expected}'! The given module type is '{given}'.")
        {
            Line = line;
            ExpectedModuleTypes = new List<PiAwareModule>(1)
            {
                expected
            };
            GivenModuleType = given;
        }

        public WrongModuleTypeException(IList<PiAwareModule> expected, PiAwareModule given, string line)
            : base($"The given line is not {GetExpected(expected)}! The given module type is '{given}'.")
        {
            Line = line;
            ExpectedModuleTypes = expected;
            GivenModuleType = given;
        }


        public string Line { get; private set; }

        public IList<PiAwareModule> ExpectedModuleTypes { get; private set; }

        public PiAwareModule GivenModuleType { get; private set; }

        private static string GetExpected(IList<PiAwareModule> moduleTypes)
        {
            StringBuilder resultBuilder = new StringBuilder();

            if (moduleTypes.Count == 1)
            {
                resultBuilder.Append($"a '{moduleTypes[0]}'");
            }
            else if (moduleTypes.Count > 1)
            {
                resultBuilder.Append("from the expected types: ");

                for (int i = 0; i < moduleTypes.Count; i++)
                {
                    PiAwareModule moduleType = moduleTypes[i];
                    resultBuilder.Append($"'{moduleType}'");
                    if (i <= moduleTypes.Count - 1)
                    {
                        resultBuilder.Append(", ");
                    }
                }
            }
            else
            {
                resultBuilder.Append("wrong");
            }

            return resultBuilder.ToString();
        }
    }
}
