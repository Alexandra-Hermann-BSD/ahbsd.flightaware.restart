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
using ahbsd.lib.flight;

namespace ahbsd.flightaware.fr24
{
    public class Module : ModuleInterpreter, IModule
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
        /// A constructor with a line to interprete
        /// </summary>
        /// <param name="line">the line to interprete</param>
        public Module(string line)
            : base(line.Trim())
        {
#if DEBUG
            OnRunningChanged += Module_OnRunningChanged;
            OnPidChanged += Module_OnPidChanged;
            OnNoticeChanged += Module_OnNoticeChanged;
#endif
        }

#if DEBUG
        private void Module_OnNoticeChanged(object sender, lib.ChangeEventArgs<string> e)
        {
            //
        }

        private void Module_OnPidChanged(object sender, lib.ChangeEventArgs<uint?> e)
        {
            //
        }

        private void Module_OnRunningChanged(object sender, lib.ChangeEventArgs<bool> e)
        {
            //
        }
#endif

        #region implementation of IModule
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        public FR24Module FR24ModuleType { get; private set; }
        #endregion

        /// <summary>
        /// Interpretes the line
        /// </summary>
        protected override string InterpreteInputLine(string line)
        {
            string result = line.Trim();
            char[] splitArr = { '(', ')' };
            string[] parts = result.Split(splitArr);
            if (parts.Length >= 3)
            {
                FR24ModuleType = GetModuleType(parts[1]);
                KeyValuePair<bool, uint?> keyValuePair = GetRunning(parts[2]);
                IsRunning = keyValuePair.Key;
                Pid = keyValuePair.Value;

                if (!IsRunning && parts.Length >= 5)
                {
                    Notice = parts[3].Trim();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the module type from a given string. 
        /// </summary>
        /// <param name="name">The given string</param>
        /// <returns>The module type</returns>
        protected static FR24Module GetModuleType(string name)
        {
            FR24Module result;
            string nameWithUnderscore = name.Replace('/', '_').Replace(" ", string.Empty).Replace(":", string.Empty);

            try
            {
                result = Enum.Parse<FR24Module>(nameWithUnderscore);
            }
            catch (Exception)
            {
                result = FR24Module.Unknown;
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
