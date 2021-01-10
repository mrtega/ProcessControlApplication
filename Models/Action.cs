using ProcessControlApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Models
{
    public class Action
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Comment { get; set; }
        public ActionPerformed ActionPerformed { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public int RoleId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
