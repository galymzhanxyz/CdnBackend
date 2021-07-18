using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaticFilesServer.Models
{
    public class SourceStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ClientBrowsers ClientBrowser { get; set; }
        public Sources Source { get; set; }
        public DateTime DownloadTime { get; set; }
    }
}
