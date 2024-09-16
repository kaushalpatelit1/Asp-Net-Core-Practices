using BookAppMVC.Data;
using BookAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BookAppMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BookAppDbContext _bookDbContext;
        public CategoryController(BookAppDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _bookDbContext.Categories;
            return View(categoryList);
        }

        #region Create Category

        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // to protect against Cross-Site Request Forgery attacks.
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString()) //custom validation
            {
                ModelState.AddModelError("Name", "The DisplayOrder and Name can not be same.");
            }
            if(ModelState.IsValid) //Server-side validation
            {
                _bookDbContext.Categories.Add(category);
                _bookDbContext.SaveChanges(); //This will post the data in db and save it.
                TempData["success"] = "Successfully created.";
                return RedirectToAction("Index");//Now It will redirect to Home page.
            }
            return View(category); //Object is empty, returning back.

        }
        #endregion

        #region Edit Category
        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFound = _bookDbContext.Categories.Find(id);
            if(categoryFound == null)
            {
                TempData["error"] = "Error in getting category.";
                return NotFound();
            }
            return View(categoryFound);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // to protect against Cross-Site Request Forgery attacks.
        public IActionResult Edit(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString()) //custom validation
            {
                ModelState.AddModelError("Name", "The DisplayOrder and Name can not be same.");
            }
            if(ModelState.IsValid) //Server-side validation
            {
                _bookDbContext.Categories.Update(category);
                _bookDbContext.SaveChanges(); //This will post the data in db and save it.
                TempData["success"] = "Successfully updated.";
                return RedirectToAction("Index");//Now It will redirect to Home page.
            }
            return View(category); //Object is empty, returning back.

        }
        #endregion

        #region Delete Category
        //GET
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var category = _bookDbContext.Categories.Find(id);
            if(category == null)
            {
                TempData["error"] = "Error in getting category.";
                return NotFound();
            }
            _bookDbContext.Categories.Remove(category);
            _bookDbContext.SaveChanges();
            TempData["success"] = "Successfully deleted.";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
