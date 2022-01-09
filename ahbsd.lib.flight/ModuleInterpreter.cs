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
    /// An abstract class to interprete an input line.
    /// </summary>
    public abstract class ModuleInterpreter : IModule
    {
        /// <summary>
        /// Is the current model running or not?
        /// </summary>
        private bool isRunning;
        /// <summary>
        /// The process id if it's running. Otherwise <c>null</c>
        /// </summary>
        private uint? pid;
        /// <summary>
        /// An otional notice
        /// </summary>
        private string notice;
        /// <summary>
        /// The input line
        /// </summary>
        private string inputLine;

        /// <summary>
        /// A constructor to set all inner variables to default values.
        /// </summary>
        private ModuleInterpreter()
        {
            InputLine = string.Empty;
            ModuleType = Module.Unknown;
            isRunning = false;
            pid = null;
            notice = string.Empty;
        }

        /// <summary>
        /// A protected constructor for child classes. 
        /// </summary>
        /// <param name="line">The input line</param>
        protected ModuleInterpreter(string line)
            : this()
        {
            InputLine = line;
        }

        /// <summary>
        /// Re sets the input line
        /// </summary>
        /// <param name="line">The input line</param>
        protected virtual void ReSetInputLine(string line)
        {
            InputLine = line;
        }

        /// <summary>
        /// Interpretes the input line.
        /// </summary>
        /// <param name="line">The input line</param>
        protected abstract string InterpreteInputLine(string line);

        #region Implementation of IModule
        /// <summary>
        /// Gets the input line.
        /// </summary>
        /// <value>The input line</value>
        public string InputLine
        {
            get => inputLine;
            private set => inputLine = InterpreteInputLine(value);
        }
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        public Module ModuleType { get; protected set; }
        /// <summary>
        /// Gets if the current model is running or not.
        /// </summary>
        /// <value>Is the current model running or not?</value>
        public bool IsRunning
        {
            get => isRunning;
            protected set
            {
                if (value != isRunning)
                {
                    ChangeEventArgs<bool> cea = new ChangeEventArgs<bool>(isRunning, value);
                    isRunning = value;
                    OnRunningChanged?.Invoke(this, cea);
                }
            }
        }
        /// <summary>
        /// Gets the process id if it's running. Otherwise <c>null</c>.
        /// </summary>
        /// <value>the process id if it's running. Otherwise <c>null</c></value>
        public uint? Pid
        {
            get => pid;
            protected set
            {
                ChangeEventArgs<uint?> cea;
                if (value != pid)
                {
                    cea = new ChangeEventArgs<uint?>(pid, value);
                    pid = value;
                    OnPidChanged?.Invoke(this, cea);
                }
            }
        }
        /// <summary>
        /// Gets an otional notice.
        /// </summary>
        /// <value>An otional notice</value>
        public string Notice
        {
            get => notice;
            protected set
            {
                ChangeEventArgs<string> cea;
                if (value == null && !string.IsNullOrEmpty(notice) 
                    || !string.IsNullOrEmpty(value) && !value.Equals(notice))
                {
                    cea = new ChangeEventArgs<string>(notice, value);
                    notice = value;
                    OnNoticeChanged?.Invoke(this, cea);
                }
            }
        }

        /// <summary>
        /// Happens, when IsRunning changed.
        /// </summary>
        public event ChangeEventHandler<bool> OnRunningChanged;
        /// <summary>
        /// Happens, when PID changed.
        /// </summary>
        public event ChangeEventHandler<uint?> OnPidChanged;
        /// <summary>
        /// Happens, when Notice changed.
        /// </summary>
        public event ChangeEventHandler<string> OnNoticeChanged;
        #endregion
    }
}
