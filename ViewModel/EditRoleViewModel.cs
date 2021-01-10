using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public int Id { get; set; }

        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
