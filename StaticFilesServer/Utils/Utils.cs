using Microsoft.AspNetCore.Mvc;
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
        public static FileStreamResult GetSourceByPath(string project_root, Sources source)
        {
            string source_path = Path.Join(project_root, source.SourcePath);
            var stream = File.OpenRead(source_path);
            return new FileStreamResult(
                stream,
                GetMimeBySourceType(source.SourceType)
            );
        }

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
            }

            return string.Empty;
        }

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
                    while (sqReader.Read())
                    {
                        var source_type = int.Parse(sqReader[2].ToString());
                        yield return new LastDownloadeds
                        {
                            BrowserId = sqReader[0].ToString(),
                            SourceName = sqReader[1].ToString(),
                            SourceType = ((Sources.SourceTypes)source_type).ToString(),
                            DownloadTime = DateTime.Parse(sqReader[3].ToString()),
                        };
                    }
                }
            }
        }

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
                    while (sqReader.Read())
                    {
                        var source_type = int.Parse(sqReader[2].ToString());
                        yield return new CountDownloadeds
                        {
                            BrowserId = sqReader[0].ToString(),
                            SourceName = sqReader[1].ToString(),
                            Count = int.Parse(sqReader[2].ToString())
                        };
                    }
                }
            }
        }
    }
}
