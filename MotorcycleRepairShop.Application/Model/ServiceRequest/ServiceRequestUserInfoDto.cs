﻿using System.ComponentModel.DataAnnotations;

namespace MotorcycleRepairShop.Application.Model
{
    public class ServiceRequestUserInfoDto
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

    }
}
