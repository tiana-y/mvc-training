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
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var list = _unitOfWork.Product.GetAll();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
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
        return View(productVM);
    }


    [HttpPost]
    public IActionResult Create(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(productVm.Product);
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
    public IActionResult Edit(int? id)
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

    [HttpPost]
    public IActionResult Edit(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(productVm.Product);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Product updated successfully";
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
