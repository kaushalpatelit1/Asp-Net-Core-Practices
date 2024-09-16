using Microsoft.AspNetCore.Mvc;
using CRUDWithRepository.Infrastructure.Interfaces;
using CRUDWithRepository.Core;

namespace CRUDWithRepositoryPatternUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository = repository;   
        }
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEditProduct(int id = 0)
        {
            if(id == 0)
            {
                return View(new Product());
            }
            else
            {
                var product = await _repository.GetById(id);
                if(product != null) 
                {
                    return View(product);
                }
                TempData["errorMessage"] = $"Product details not found with given Id: {id}";
                return RedirectToAction(nameof(Index));
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEditProduct(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Id == 0)
                    {
                        await _repository.Add(model);
                        TempData["successMessage"] = $"Product created successfully.";
                    }
                    else
                    {
                        await _repository.Update(model);
                        TempData["successMessage"] = $"Product updated successfully.";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Invalid Product.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Invalid Product. Exception = {ex}";
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Product product = await _repository.GetById(id);
                if(product != null)
                {
                    return View(product);
                }
                TempData["errorMessage"] = $"Product details not found with given Id: {id}";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = $"Invalid Product. Exception = {ex}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Product model)
        {
            try
            {
                await _repository.Delete(model.Id);
                TempData["successMessage"] = $"Product Deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
