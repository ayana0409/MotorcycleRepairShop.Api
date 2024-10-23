using Microsoft.AspNetCore.Http;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestDto
    {
        [Required]
        [MaxLength(11)]
        public string MobilePhone { get; set; } = string.Empty;
        [Required]
        [MaxLength(250)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        public string IssueDescripton { get; set; } = string.Empty;

        public ServiceType ServiceType { get; set; } = ServiceType.Direct;

        public IEnumerable<IFormFile> Videos { get; set; } = [];
        public IEnumerable<IFormFile> Images { get; set; } = [];
        public List<int> Problems { get; set; } = [];

    }
}
