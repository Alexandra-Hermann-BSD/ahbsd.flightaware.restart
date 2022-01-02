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
using System.ComponentModel;
using System.Linq;

namespace ahbsd.flightaware.piaware
{
    public class StatusContent : IStatusContent
    {
        /// <summary>
        /// Gets the list of lines.
        /// </summary>
        /// <value>The list of lines</value>
        [ReadOnly(true)]
        protected internal IList<string> Lines { get; private set; }

        private readonly IDictionary<LineType, IList<string>> contentDictionary;

        /// <summary>
        /// Constructor with given content.
        /// </summary>
        /// <param name="content">The given Content</param>
        public StatusContent(string content)
        {
            Modules = new List<IModule>(5);
            Connected = new List<IConnected<connectedPart.IConnectedPart>>(3);
            contentDictionary = new Dictionary<LineType, IList<string>>(5);

            FillContentDictionary(content);
            SetParts();
        }

        /// <summary>
        /// Fills the content directory
        /// </summary>
        /// <param name="content">The given Content</param>
        private void FillContentDictionary(string content)
        {
            char[] splitChars = { '\r', '\n' };
            string[] lines = content.Split(splitChars, StringSplitOptions.None);
            foreach ((string line, LineType lineType) in from string line in lines
                                             let lineType = PreInterpreteLine(line)
                                             select (line, lineType))
            {
                if (!contentDictionary.ContainsKey(lineType))
                {
                    contentDictionary.Add(lineType, new List<string>());
                }

                contentDictionary[lineType].Add(line);
            }
        }

        /// <summary>
        /// Set the Parts of <see cref="IStatusContent"/>.
        /// </summary>
        private void SetParts()
        {
            foreach (var item in contentDictionary)
            {
                switch (item.Key)
                {
                    case LineType.Module:
                        foreach (string line in item.Value)
                        {
                            Modules.Add(new Module(line));
                        }
                        break;
                    case LineType.FeederId:
                        FeederId = GetFeederID(line: item.Value.First());
                        break;
                    case LineType.Connected:
                        foreach (string line in item.Value)
                        {

                        }
                        break;
                    case LineType.DumpUri:
                        DumpUri = GetDumpUri(item.Value.First());
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
        }

        #region implementation of IStatusContent
        /// <summary>
        /// Gets a list of Modules.
        /// </summary>
        /// <value>A list of Modules</value>
        [ReadOnly(true)]
        public IList<IModule> Modules { get; private set; }
        /// <summary>
        /// Gets the amount of running modules.
        /// </summary>
        /// <value>The amount of running modules</value>
        public ushort RunningModules => (ushort)Modules.Count(item => item.IsRunning);
        /// <summary>
        /// Gets if something critical is going on. And a restart should be done.
        /// </summary>
        /// <value><c>true</c> if critical, otherwise <c>false</c></value>
        public bool Critical => (Modules.Count - 1) > RunningModules;
        /// <summary>
        /// Gets the feeder id.
        /// </summary>
        /// <value>The feeder id</value>
        [ReadOnly(true)]
        public Guid FeederId { get; private set; }
        /// <summary>
        /// Gets a list of connected modules.
        /// </summary>
        /// <value>A list of connected modules</value>
        [ReadOnly(true)]
        public IList<IConnected<connectedPart.IConnectedPart>> Connected { get; private set; }
        /// <summary>
        /// Gets the <see cref="Uri"/> of the dump module.
        /// </summary>
        /// <value>The Uri of the dump module.</value>
        public Uri DumpUri { get; private set; }
        #endregion

        /// <summary>
        /// Gets the feeder id from a given line.
        /// </summary>
        /// <param name="line">The given line</param>
        /// <returns>The feeder id</returns>
        /// <example>
        /// line = "Your feeder ID is 335bb436-4746-41d3-81b1-ce05b8ba8ecb (from /var/cache/piaware/feeder_id)";<br />
        /// result = {335bb436-4746-41d3-81b1-ce05b8ba8ecb};
        /// </example>
        private Guid GetFeederID(string line)
        {
            string firstPart = "Your feeder ID is ";
            int firstLength = firstPart.Length;
            string cutLine = line[firstLength..];
            string[] parts = cutLine.Split(' ');
            if (parts.Length < 1 || !Guid.TryParse(parts[0], out Guid result))
            {
                result = Guid.Empty;
            }
            return result;
        }

        /// <summary>
        /// Gets the dump-uri from a given line.
        /// </summary>
        /// <param name="line">The given line</param>
        /// <returns>The dump uri</returns>
        /// <example>
        /// line = "dump1090 is producing data on localhost:30005.";</br>
        /// result = "localhost:30005";
        /// </example>
        private static Uri GetDumpUri(string line)
        {
            string[] parts = line.Split(' ');
            string lastPart = parts.Last();
            // removing the dot at the end
            lastPart = lastPart.Substring(0, lastPart.Length - 1);
            string[] uriParts = lastPart.Split(':');
            string host = uriParts[0].Trim();
            if (!int.TryParse(uriParts[1], out int port))
            {
                port = 80;
            }
            UriBuilder uriBuilder = new UriBuilder
            {
                Host = host,
                Port = port
            };

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Pre interpretes the line as line type.
        /// </summary>
        /// <param name="line">A single text line</param>
        /// <returns>The interpreted line type</returns>
        /// <remarks>
        /// Helps to pre sort each line to a specific type of line in the <see cref="contentDictionary"/>.
        /// </remarks>
        internal static LineType PreInterpreteLine(string line)
        {
            LineType lineType = LineType.Unknown;

            if (line.StartsWith("PiAware ") || line.StartsWith("Local ADS-B"))
            {
                lineType = LineType.Module;
            }
            else if (line.StartsWith("dump1090-fa")
                || line.StartsWith("faup1090 is")
                || line.StartsWith("piaware is"))
            {
                lineType = LineType.Connected;
            }
            else if (line.StartsWith("dump1090 is "))
            {
                lineType = LineType.DumpUri;
            }
            else if (line.StartsWith("Your feeder ID is"))
            {
                lineType = LineType.FeederId;
            }
            return lineType;
        }
    }
}
