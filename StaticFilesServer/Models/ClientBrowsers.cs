using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaticFilesServer.Models
{
    public class ClientBrowsers
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uid { get; set; }
        [Required]
        public string BrowserId { get; set; }
        public string ClientIp { get; set; }
        [Required]
        public DateTime RegisterTime { get; set; }
    }
}
