using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaticFilesServer.Models
{
    public class Sources
    {
        public enum SourceTypes
        {
            JavaScript,
            Css,
            Image,
        }
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string SourceName { get; set; }
        [Required]
        public SourceTypes SourceType { get; set; }
        [Required]
        public long SourceLength { get; set; }
        [Required]
        public string SourcePath { get; set; }
    }
}
