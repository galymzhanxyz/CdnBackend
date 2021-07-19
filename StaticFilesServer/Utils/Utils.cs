using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Serilog;
using StaticFilesServer.Models;
using StaticFilesServer.StatisticModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace StaticFilesServer.Utils
{
    public static class Utils
    {
        private static ILogger _log = Log.ForContext(typeof(PathHelper));

        /// <summary>
        /// Gets FileStream by given source_path
        /// </summary>
        /// <param name="project_root">Global root of project</param>
        /// <param name="source">Source information</param>
        /// <returns>Content of Source</returns>
        public static FileStreamResult GetSourceByPath(string project_root, Sources source)
        {
            string source_path = Path.Join(project_root, source.SourcePath);
            try
            {
                var stream = File.OpenRead(source_path);
                return new FileStreamResult(
                            stream,
                            GetMimeBySourceType(source.SourceType));
            }
            catch (Exception ex)
            {
                _log.Error($"{ex.Message} {ex.StackTrace}");
                throw;
            }
        }
        /// <summary>
        /// Get mime string by Type of Source 
        /// </summary>
        /// <param name="sourceType">Type of source (Enum)</param>
        /// <returns>Mime type string</returns>
        public static string GetMimeBySourceType(Sources.SourceTypes sourceType)
        {
            switch (sourceType)
            {
                case Sources.SourceTypes.JavaScript:
                    return "application/javascript";
                case Sources.SourceTypes.Css:
                    return "text/css";
                case Sources.SourceTypes.Image:
                    return "image/*";
                default:
                    return "application/empty";
            }
        }
        /// <summary>
        /// Runs raw sql and deserilize it to inner model LastDownloadeds
        /// </summary>
        /// <param name="connection">Database connection of Dbcontext</param>
        /// <returns>Generator of LastDownloadeds type</returns>
        public static IEnumerable<LastDownloadeds> GetLastDownloadeds(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection.Open();
                command.CommandText = @"
                    SELECT d2.BrowserId,
                           d3.SourceName,
                           d3.SourceType,
                           DownloadTime
                    FROM
                      (SELECT ClientBrowserUid,
                              SourceId,
                              DownloadTime,
                              ROW_NUMBER() OVER(PARTITION BY SourceId, ClientBrowserUid
                                                ORDER BY DownloadTime DESC) AS rank
                       FROM SourceStatistic) AS d1
                    LEFT JOIN ClientBrowser AS d2 ON d1.ClientBrowserUid = d2.Uid
                    LEFT JOIN SOURCE AS d3 ON d1.SourceId = d3.Id
                    WHERE rank = 1
                ";
                using (var sqReader = command.ExecuteReader())
                {
                    LastDownloadeds lastDownloadeds = null;
                    while (sqReader.Read())
                    {
                        try
                        {
                            var source_type = int.Parse(sqReader[2].ToString());
                            lastDownloadeds = new LastDownloadeds
                            {
                                BrowserId = sqReader[0].ToString(),
                                SourceName = sqReader[1].ToString(),
                                SourceType = ((Sources.SourceTypes)source_type).ToString(),
                                DownloadTime = DateTime.Parse(sqReader[3].ToString()),
                            };
                        }
                        catch (Exception ex)
                        {
                            _log.Error($"{ex.Message} {ex.StackTrace}");
                            throw;
                        }
                        yield return lastDownloadeds;
                    }
                }
            }
        }
        /// <summary>
        /// Runs raw sql and deserilize it to inner model CountDownloadeds
        /// </summary>
        /// <param name="connection">Database connection of Dbcontext</param>
        /// <returns>Generator of CountDownloadeds type</returns>
        public static IEnumerable<CountDownloadeds> GetCountDownloadeds(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection.Open();
                command.CommandText = @"
                    SELECT d3.BrowserId,
                           d2.SourceName,
                           d1.Count
                    FROM
                      (SELECT ClientBrowserUid,
                              SourceId,
                              count(*) AS COUNT
                       FROM SourceStatistic
                       GROUP BY ClientBrowserUid,
                                SourceId) AS d1
                    LEFT JOIN SOURCE d2 ON d1.SourceId = d2.Id
                    LEFT JOIN ClientBrowser d3 ON d1.ClientBrowserUid = d3.Uid
                ";
                using (var sqReader = command.ExecuteReader())
                {
                    CountDownloadeds countDownloadeds = null;
                    while (sqReader.Read())
                    {
                        try
                        {
                            countDownloadeds = new CountDownloadeds
                            {
                                BrowserId = sqReader[0].ToString(),
                                SourceName = sqReader[1].ToString(),
                                Count = int.Parse(sqReader[2].ToString())
                            };
                        }
                        catch (Exception ex)
                        {
                            _log.Error($"{ex.Message} {ex.StackTrace}");
                            throw;
                        }

                        yield return countDownloadeds;
                    }
                }
            }
        }
    }
}
