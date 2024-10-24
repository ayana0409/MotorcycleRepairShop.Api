﻿using MotorcycleRepairShop.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleRepairShop.Domain.Entities
{
    public class ServiceRequest : BaseEntity
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
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
        public DateTime CompletionDate { get; set; }
        public decimal TotalPrice { get; set; }

        public ServiceType ServiceType { get; set; } = ServiceType.Direct;
        public int StatusId { get; set; } = Convert.ToInt32(StatusEnum.New);
        [ForeignKey(nameof(StatusId))]
        public Status? Status { get; set; }

        public virtual ICollection<Video> Videos { get; set; } = [];
        public virtual ICollection<Image> Images { get; set; } = [];
        public virtual ICollection<ServiceRequestProblem> Problems { get; set; } = [];
        public virtual ICollection<ServiceRequestItem> Services { get; set; } = [];

    }
}
