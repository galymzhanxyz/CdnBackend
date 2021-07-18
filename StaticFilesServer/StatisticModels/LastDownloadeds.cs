using System;

namespace StaticFilesServer.StatisticModels
{
    public class LastDownloadeds
    {
        public string BrowserId { get; set; }
        public string SourceName { get; set; }
        public string SourceType { get; set; }
        public DateTime DownloadTime { get; set; }
    }
}
