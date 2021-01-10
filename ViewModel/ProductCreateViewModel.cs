using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.ViewModel
{
    public class ProductCreateViewModel
    {
        
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }

        public string Comment { get; set; }
    }
}
