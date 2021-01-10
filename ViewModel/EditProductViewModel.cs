using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.ViewModel
{
    public class EditProductViewModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ExistingPhoto { get; set; }

        public string Comment { get; set; }
    }
}
