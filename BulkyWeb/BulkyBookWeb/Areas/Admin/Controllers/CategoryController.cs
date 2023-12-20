using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var list = _unitOfWork.Category.GetAll();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category item)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(item);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(item);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }
        var item = _unitOfWork.Category.GetFirstOfDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(Category item)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(item);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        return View(item);
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }
        var item = _unitOfWork.Category.GetFirstOfDefault(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(Category item)
    {
        _unitOfWork.Category.Remove(item);
        _unitOfWork.SaveChanges();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}
