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
namespace ahbsd.flightaware.fr24
{
    /// <summary>
    /// Server-Modules
    /// </summary>
    public enum FR24Module
    {
        /*
FR24 Feeder/Decoder Process: running.
FR24 Stats Timestamp: 2022-01-01 15:37:51.
FR24 Link: connected [UDP].
FR24 Radar: T-EDTL60.
FR24 Tracked AC: 53.
Receiver: connected (3278019 MSGS/0 SYNC).
FR24 MLAT: ok [UDP].
FR24 MLAT AC seen: 46.         
         */
        Unknown,
        Feeder_Decoder,
        Stats,
        Link,
        Radar,
        Tracked,
        connected,
        MLAT,
    }
}
