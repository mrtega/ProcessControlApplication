using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProcessControlApplication.Data;
using ProcessControlApplication.Models;
using ProcessControlApplication.Models.Enums;
using ProcessControlApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationService productService;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProductController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IApplicationService productService,
            IHostingEnvironment hostingEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.productService = productService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "StoreKeeper")]
        [HttpGet]
        public  ViewResult Create()
        {
            return View();
        }

        [Authorize(Roles = "StoreKeeper")]
        [HttpPost]
        public async Task<JsonResult> Create(ProductCreateViewModel product)
        {
            string result;
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            var role = await roleManager.FindByNameAsync(roles.SingleOrDefault());
           

            var imageUrl = SavePhoto(product.Photo);
            var res = await productService.AddNewProduct(product.Name, imageUrl, user.Id, role.Id, 
                product.Comment, product.Description, product.Price);
            if (res != null)
            {
                result = "1|Product created successfully!";
            }
            else
            {
                result = "2|Error creating product.";
            }
            return Json(result);
        }


        [HttpGet]
        public  async Task<ViewResult> GetProduct(Guid Id)
        {
            var model = await productService.GetProduct(Id);
            if (model == null)
            {
                throw new Exception("Product not found.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> GetAllProducts()
        {
            var products = await productService.GetActiveProducts();
            return View(products);
        }

        [Authorize(Roles = "StoreKeeper, ProductManager, Supervisor, Manager")]
        [HttpGet]
        public async Task<ViewResult> ProductsAction(Guid Id)
        {
            var product = await productService.GetProduct(Id);
            var actions = await productService.GetProductActions(Id);
            return View(new ProductViewModel { Product = product, Actions = actions.OrderBy(x => x.DateCreated) });
        }

        [Authorize(Roles = "StoreKeeper")]
        [HttpGet]
        public async Task<ViewResult> EditProduct(Guid Id)
        {
            var product = await productService.GetProduct(Id);
            var actions = await productService.GetProductActions(Id);
            return View(new ProductViewModel { Product = product, Actions = actions.OrderBy(x => x.DateCreated) });
        }
        [Authorize(Roles = "StoreKeeper")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel productViewModel)
        {
            
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            var role = await roleManager.FindByNameAsync(roles.SingleOrDefault());

            var res = await productService.EditProduct(productViewModel.Product, user.Id, role.Id,
                productViewModel.Comment);
            return RedirectToAction("productspending", "product");
        }

        [Authorize(Roles = "StoreKeeper, ProductManager, Supervisor, Manager")]
        [HttpGet]
        public async Task<ViewResult> ProductsPending()
        {
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            var role = await roleManager.FindByNameAsync(roles.SingleOrDefault());
 
            var res = await productService.GetProductsForRole(role.Id);
            return View(res);
        }

        [Authorize(Roles = "StoreKeeper, ProductManager, Supervisor, Manager")]
        [HttpPost]
        public async Task<IActionResult> ProductsAction(string button, ProductActionViewModel productActionViewModel)
        {
            if (button == "accept")
            {
                productActionViewModel.ActionPerformed = 2;
            }
            else
            {
                productActionViewModel.ActionPerformed = 3;
            }
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            var role = await roleManager.FindByNameAsync(roles.SingleOrDefault());

            var res = await productService.PerformActionOnProduct(productActionViewModel.Id, user.Id, role.Id,
                productActionViewModel.Comment, (ActionPerformed)productActionViewModel.ActionPerformed);
            return RedirectToAction("productspending", "product");
        }

        private string SavePhoto(IFormFile photo)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
