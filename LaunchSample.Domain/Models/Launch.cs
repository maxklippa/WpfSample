using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LaunchSample.Core.Enumerations;

namespace LaunchSample.Domain.Models
{
    public class Launch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime Month { get; set; }
        public LaunchStatus Status { get; set; }
    }
}