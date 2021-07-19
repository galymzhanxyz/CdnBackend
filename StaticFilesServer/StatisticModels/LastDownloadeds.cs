using System;

namespace StaticFilesServer.StatisticModels
{
    public class LastDownloadeds
    {
        /// <summary>
        /// Browser Uid which stored in cookies
        /// </summary>
        public string BrowserId { get; set; }
        /// <summary>
        /// Name of Source
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// Extension of Source
        /// </summary>
        public string SourceType { get; set; }
        /// <summary>
        /// Time when source was requested to download
        /// </summary>
        public DateTime DownloadTime { get; set; }
    }
}
