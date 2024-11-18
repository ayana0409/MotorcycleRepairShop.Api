using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Share.Models
{
    public class MailRequest
    {
        [EmailAddress]
        public string From { get; set; } = string.Empty;
        [EmailAddress]
        public string ToAddress { get; set; } = string.Empty;
        public IEnumerable<string> ToAddresses { get; set; } = [];
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IFormFileCollection? Attachments { get; set; } = null;
    }
}
