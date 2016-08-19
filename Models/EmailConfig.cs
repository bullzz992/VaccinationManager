using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VaccinationManager.Models
{
    public class EmailConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ConfigType { get; set; }

        [Required]
        public string FromAddress { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string EmailContent { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

    }
}