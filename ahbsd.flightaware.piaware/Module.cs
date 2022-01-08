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

namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// Class for a PiAware module
    /// </summary>
    public class Module : IModule
    {
        /// <summary>
        /// Currently the string for, that a module is running
        /// </summary>
        public const string IS_RUNNING = "is running";
        /// <summary>
        /// Currently the string for, which process id a module has
        /// </summary>
        public const string WITH_PID = "with pid ";
        /// <summary>
        /// The character array for brackets as splitter
        /// </summary>
        /// <remarks>An array can't be a constant... so it is static readonly...</remarks>
        /// <example>
        /// string helloUglyWorld = "Hello (ugly) world";<br/>
        /// string[] splitted = helloUglyWorld.Split(BRACKET_SPLITTER);<br/>
        /// // would be<br/>
        /// // splitted[0] = "Hello "<br/>
        /// // splitted[1] = "ugly"<br/>
        /// // splitted[2] = " world"<br/>
        /// </example>
        public static readonly char[] BRACKET_SPLITTER = { '(', ')' };

        /// <summary>
        /// a simple constructor, that fills the variables with default walues
        /// </summary>
        protected Module()
        {
            InputLine = string.Empty;
            ModuleType = PiAwareModule.unknown;
            IsRunning = false;
            Pid = null;
            Notice = string.Empty;
        }

        /// <summary>
        /// A constructor with a line to interprete
        /// </summary>
        /// <param name="line">the line to interprete</param>
        public Module(string line)
            : this()
        {
            InputLine = line.Trim();
            InterpreteLine();
        }

        #region implementation of IModule
        /// <summary>
        /// Gets the input line.
        /// </summary>
        /// <value>The input line</value>
        public string InputLine { get; private set; }
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        public PiAwareModule ModuleType { get; private set; }
        /// <summary>
        /// Gets if the current model is running or not.
        /// </summary>
        /// <value>Is the current model running or not?</value>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// Gets the process id if it's running. Otherwise <c>null</c>.
        /// </summary>
        /// <value>the process id if it's running. Otherwise <c>null</c></value>
        public uint? Pid { get; private set; }
        /// <summary>
        /// Gets an otional notice.
        /// </summary>
        /// <value>An otional notice</value>
        public string Notice { get; private set; }
        #endregion

        /// <summary>
        /// Interpretes the line
        /// </summary>
        protected void InterpreteLine()
        {
            string[] parts = InputLine.Split(BRACKET_SPLITTER);
            if (parts.Length >= 3)
            {
                ModuleType = GetModuleType(parts[1]);
                KeyValuePair<bool, uint?> keyValuePair = GetRunning(parts[2]);
                IsRunning = keyValuePair.Key;
                Pid = keyValuePair.Value;

                if (!IsRunning && parts.Length >= 5)
                {
                    Notice = parts[3].Trim();
                }
            }
        }

        /// <summary>
        /// Gets the module type from a given string. 
        /// </summary>
        /// <param name="name">The given string</param>
        /// <returns>The module type</returns>        
        protected static PiAwareModule GetModuleType(string name)
        {
            PiAwareModule result;
            string nameWithUnderscore = name.Replace('-', '_').Replace(" ", string.Empty);

            try
            {
                result = Enum.Parse<PiAwareModule>(nameWithUnderscore);
            }
            catch (Exception)
            {
                result = PiAwareModule.unknown;
            }

            return result;
        }

        /// <summary>
        /// Gets, if the module is running and (if it's running) the process id.
        /// </summary>
        /// <param name="thirdPart">The given string.</param>
        /// <returns>Is the module running as Key and if it's running the process id as value. If it's not running the value is <c>null</c></returns>
        protected static KeyValuePair<bool, uint?> GetRunning(string thirdPart)
        {
            uint? pid = null;

            bool running = thirdPart.Contains(IS_RUNNING);

            if (running && thirdPart.Contains(WITH_PID))
            {
                string sPid = FilterPid(thirdPart);

                if (uint.TryParse(sPid, out uint tmpPid))
                {
                    pid = tmpPid;
                }
            }

            KeyValuePair<bool, uint?> result = new KeyValuePair<bool, uint?>(running, pid);
            return result;
        }

        /// <summary>
        /// Filters out the part of the process id from a given string and returns the process id as string.
        /// </summary>
        /// <param name="part">The given string</param>
        /// <returns>The process id as string</returns>
        private static string FilterPid(string part)
        {
            int startPos = part.IndexOf(WITH_PID) + WITH_PID.Length;
            int endPos = part.Length - 1;
            int length = endPos - startPos;
            return part.Substring(startPos, length);
        }
    }
}
