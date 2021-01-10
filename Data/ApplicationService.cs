using Microsoft.EntityFrameworkCore;
using ProcessControlApplication.Models;
using ProcessControlApplication.Models.Enums;
using ProcessControlApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Data
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext context;

        public ApplicationService(AppDbContext context)
        {
            this.context = context;

        }
        public async Task<Product> AddNewProduct(string Name, string ImageUrl, int UserId, int RoleId, string Comment, string Description, int Price)
        {
            var user = await context.Users.FindAsync(UserId);
            if (user == null)
                throw new Exception("User not found.");

            var role = await context.Roles.FindAsync(RoleId);
            if (role == null)
                throw new Exception("Invalid user role.");

            if (role.ParentRoleId != null)
                throw new Exception("User cannot create a product.");
            var productId = Guid.NewGuid(); 
            //create the product.
            var product = new Product
            {
                Id = productId,
                Name = Name,
                ImageUrl = ImageUrl,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                IsActive = false,
                CurrentLevelRoleId = RoleId,
                Price = Price,
                Description = Description
            };

            context.Add(product);
            //Add the Creation action.
            context.Add(new Models.Action
            {
                ActionPerformed = Models.Enums.ActionPerformed.Created,
                Comment = Comment,
                UserId = UserId,
                ProductId = productId,
                DateCreated = DateTime.Now,
                RoleId = RoleId
            });

            //update the CurrentLevelRoleId to Role ChildId if available
            if (role.ChildRoleId != null)
                product.CurrentLevelRoleId = (int)role.ChildRoleId;
            // if the user's role has no child role, then it is the final approval level,
            // hence make the product available for customers.
            else
                product.IsActive = true;

            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> EditProduct(Product product, int UserId, int RoleId, string Comment )
        {
            var user = await context.Users.FindAsync(UserId);
            if (user == null)
                throw new Exception("User not found.");

            var role = await context.Roles.FindAsync(RoleId);
            if (role == null)
                throw new Exception("Invalid user role.");

            if (role.ParentRoleId != null)
                throw new Exception("This user does not have permission to edit a product.");

            context.Update(product);
            context.Add(new Models.Action
            {
                ActionPerformed = Models.Enums.ActionPerformed.Edited,
                Comment = Comment,
                UserId = UserId,
                ProductId = product.Id,
                DateCreated = DateTime.Now,
                RoleId =RoleId
            });

            //update the CurrentLevelRoleId to Role ChildId if available
            if (role.ChildRoleId != null)
                product.CurrentLevelRoleId = (int)role.ChildRoleId;
            // if the user's role has no child role, then it is the final approval level,
            // hence make the product available for customers.
            else
                product.IsActive = true;

            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProduct(Guid Id)
        {
            return await context.Products.FindAsync(Id);
        }

        public async Task<Product> PerformActionOnProduct(Guid ProductId, int UserId, int RoleId,
            string Comment, ActionPerformed actionPerformed)
        {
            var product = await context.Products.FindAsync(ProductId);
            if (product == null)
                throw new Exception("Invalid product.");

            var user = await context.Users.FindAsync(UserId);
            if (user == null)
                throw new Exception("User not found.");

            var role = await context.Roles.FindAsync(RoleId);
            if (role == null)
                throw new Exception("Invalid user role.");

            if (role.ParentRoleId != null && (actionPerformed.Equals(ActionPerformed.Created) ||
                actionPerformed.Equals(ActionPerformed.Edited)))
                throw new Exception("User with this role cannot create or edit a product.");

            if (role.ParentRoleId == null && (actionPerformed.Equals(ActionPerformed.Approved) ||
               actionPerformed.Equals(ActionPerformed.Rejected)))
                throw new Exception("User with this role cannot approve or reject a product.");

            context.Actions.Add(new Models.Action
            {
                ActionPerformed = actionPerformed,
                Comment = Comment,
                UserId = UserId,
                ProductId = product.Id,
                DateCreated = DateTime.Now,
                RoleId = RoleId
            });

            // if the product was rejected, move it down one approval level
            if (actionPerformed.Equals(ActionPerformed.Rejected))
                product.CurrentLevelRoleId = (int)role.ParentRoleId;
            else // if the product was created, edited or approved, move it up one approval level
            {
                product.CurrentLevelRoleId = role.ChildRoleId;

                // if the current approval level has no other layer above it, activate the product
                if (role.ChildRoleId == null) product.IsActive = true;
            }

            await context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetActiveProducts()
        {
            return await context.Products.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsForRole(int RoleId)
        {
            var role = await context.Roles.FindAsync(RoleId);
            if (role == null)
                throw new Exception("Invalid user role.");

            return await context.Products.Where(x => x.CurrentLevelRoleId == RoleId).ToListAsync();
        }

        public async Task<List<Models.Action>> GetProductActions(Guid productId)
        {
            var product = await context.Products.FindAsync(productId);
            var actions =  await context.Actions.Where(x => x.ProductId == productId).ToListAsync();
            return actions;
        }

    }
}
