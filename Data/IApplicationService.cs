using ProcessControlApplication.Models;
using ProcessControlApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Data
{
    public interface IApplicationService
    {
        Task<Product> AddNewProduct(string Name, string ImageUrl, int UserId, int RoleId, string Comment, string Description, int Price);
      
        Task<IEnumerable<Product>> GetActiveProducts();
        Task<IEnumerable<Product>> GetProductsForRole(int RoleId);
        Task<Product> PerformActionOnProduct(Guid ProductId, int UserId, int RoleId,
            string Comment, ActionPerformed actionPerformed);
        Task<Product> GetProduct(Guid Id);
        Task<Product> EditProduct(Product product, int UserId, int RoleId, string Comment);
        Task<List<Models.Action>> GetProductActions(Guid productId);
    }
}
