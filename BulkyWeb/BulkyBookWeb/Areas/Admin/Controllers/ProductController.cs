using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnv;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnv)
    {
        _unitOfWork = unitOfWork;
        _webHostEnv = webHostEnv;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var list = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult Upsert(int? id)
    {
        IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll(includeProperties: "Category")
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            });
        var productVM = new ProductViewModel
        {
            CategoryList = categoryList,
            Product = new Product()
        };
        // For Update
        if (id is not null && id != 0)
        {
            productVM.Product = _unitOfWork.Product.GetFirstOfDefault(x => x.Id == id);
        }
        return View(productVM);
    }


    [HttpPost]
    public IActionResult Upsert(ProductViewModel productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnv.WebRootPath;
            if (file is not null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVm.Product.ImageUrl)) {
                    var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                   file.CopyTo(fileStream);
                }

                productVm.Product.ImageUrl = @"\images\product\" + fileName;
            }
            if (productVm.Product.Id != 0)
            {
                _unitOfWork.Product.Update(productVm.Product);
            }
            else
            {
                _unitOfWork.Product.Add(productVm.Product);
            }
            _unitOfWork.SaveChanges();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }
        IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
           .Select(c => new SelectListItem
           {
               Value = c.Id.ToString(),
               Text = c.Name,
           });
        productVm.CategoryList = categoryList;
        return View(productVm);
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }
        var item = _unitOfWork.Product.GetFirstOfDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(Product item)
    {
        _unitOfWork.Product.Remove(item);
        _unitOfWork.SaveChanges();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}
