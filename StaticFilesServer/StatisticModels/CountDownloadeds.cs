namespace StaticFilesServer.StatisticModels
{
    public class CountDownloadeds
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
        /// Count how much Source was downloaded
        /// </summary>
        public int Count { get; set; }
    }
}
