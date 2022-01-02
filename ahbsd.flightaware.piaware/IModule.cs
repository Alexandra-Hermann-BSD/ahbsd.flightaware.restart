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
    /// Interface for a module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the input line.
        /// </summary>
        /// <value>The input line</value>
        string InputLine { get; }
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type</value>
        PiAwareModule ModuleType { get; }
        /// <summary>
        /// Gets if the current model is running or not.
        /// </summary>
        /// <value>Is the current model running or not?</value>
        bool IsRunning { get; }
        /// <summary>
        /// Gets the process id if it's running. Otherwise <c>null</c>.
        /// </summary>
        /// <value>the process id if it's running. Otherwise <c>null</c></value>
        uint? Pid { get; }
        /// <summary>
        /// Gets an otional notice.
        /// </summary>
        /// <value>An otional notice</value>
        string Notice { get; }
    }
}
