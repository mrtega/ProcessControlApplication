using Microsoft.AspNetCore.Http;
using ProcessControlApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.ViewModel
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Models.Action> Actions { get; set; }

        public Guid Id { get; set; }
        public string Comment { get; set; }


    }
}
