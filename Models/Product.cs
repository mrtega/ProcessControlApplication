using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [ForeignKey(nameof(Role))]
        public int? CurrentLevelRoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
