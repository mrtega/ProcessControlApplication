using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.ViewModel
{
    public class ProductActionViewModel
    {
        public int ActionPerformed { get; set; }
        public Guid Id { get; set; }

        public string Comment { get; set; }
    }
}
