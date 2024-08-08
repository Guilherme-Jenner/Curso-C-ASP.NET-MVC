using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDEMY_PROJECT.Services.Exceptions;
using UDEMY_PROJECT.Models;
using UDEMY_PROJECT.Models.ViewModels;
using UDEMY_PROJECT.Services;

namespace UDEMY_PROJECT.Controllers
{
    public class SellersController : Controller
    {

        public SellersController()
        {
            
        }

        // GET: Sellers
        public async Task<IActionResult> Index([FromServices] RepositoryService<Seller> _sellerService)
        {
            var sellersList = await _sellerService.GetAllAsync();

            return View(sellersList);
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id, [FromServices] RepositoryService<Seller> _sellerService)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            var seller = await _sellerService.GetOneAsync(id, a=> a.Department, a=> a.Id == id);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            return View(seller);
        }

        // GET: Sellers/Create
        public async Task<IActionResult> Create([FromServices] RepositoryService<Department> _deparmentService)
        {
            var departmentList = await _deparmentService.GetAllAsync();
            var viewModel = new SellerViewModel { Departments = departmentList };

            return View(viewModel);
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromServices] RepositoryService<Seller> _sellerService, [FromServices] RepositoryService<Department> _departmentService , [Bind("Id,Name,Email,BirthDate,BaseSalary,DepartmentId")] Seller seller)
        {
            var departments = await _departmentService.GetAllAsync();

            var sellerViewModel = new SellerViewModel { Departments = departments, Seller = seller };

            if (!ModelState.IsValid)
            {
                if(seller.Department != null)
                {
                    return View(sellerViewModel);
                }
            }

            try
            {
                await _sellerService.AddAsync(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {

                return View(sellerViewModel);
            }
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromServices] RepositoryService<Seller> _sellerService, [FromServices] RepositoryService<Department> _departmentService)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            var seller = await _sellerService.GetOneAsync(id, a => a.Department, a => a.Id == id);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            var listDepartments = await _departmentService.GetAllAsync();

            var viewModel = new SellerViewModel() { Seller = seller, Departments = listDepartments }; 

            return View(viewModel);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,BirthDate,BaseSalary, DepartmentId")] Seller seller, [FromServices] RepositoryService<Department> _departmentService , [FromServices] RepositoryService<Seller> _sellerService)
        {

            var departments = await _departmentService.GetAllAsync();
            var sellerViewModel = new SellerViewModel { Departments = departments, Seller = seller };

            if (!ModelState.IsValid) 
            {
                if (seller.Department != null)
                {
                    return View(sellerViewModel);
                }
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "The Id Provided is not the same of the Sellers Id" });
            }

         
            try
            {
                await _sellerService.UpdateAsync(seller);
            }
            catch (DbUpdateConcurrencyException e)
            {
                var exists = await _sellerService.ExistsAsync(a => a.Id == id);
                if (!exists)
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
                else
                {
                    throw new DbUpdateConcurrencyException(e.Message);
                }
            }


            return RedirectToAction(nameof(Details), new {id = seller.Id});

        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id, [FromServices] RepositoryService<Seller> _sellerService)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new {message = "Id Not Provided"});
            }

            var seller = await _sellerService.GetOneAsync(id, null, null);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [FromServices] RepositoryService<Seller> _sellerService)
        {
            try
            {
                var seller = await _sellerService.GetOneAsync(id, null, null);

                if (seller != null)
                {
                    await _sellerService.RemoveAsync(seller);
                }
            }
            catch(IntegrityException e)
            {
                RedirectToAction(nameof(Error), new {message = e.Message});
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SellerExists(int id, [FromServices] RepositoryService<Seller> _sellerService)
        {
            return await _sellerService.ExistsAsync(s => s.Id == id);
        }

        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(errorViewModel);

        }
    }
}
