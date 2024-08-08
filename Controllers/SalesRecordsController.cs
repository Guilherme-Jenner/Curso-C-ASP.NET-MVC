using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDEMY_PROJECT.Data;
using UDEMY_PROJECT.Models;
using UDEMY_PROJECT.Services;

namespace UDEMY_PROJECT.Controllers
{
    public class SalesRecordsController : Controller
    {
        public SalesRecordsController()
        {

        }

        // GET: SalesRecords
        public async Task<IActionResult> Index([FromServices] RepositoryService<SalesRecord> _repositoryService)
        {
            var result = await _repositoryService.GetAllAsync();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DateTime? dateInitial, DateTime? dateFinal, string tipo , [FromServices] RepositoryService<SalesRecord> _salesRecordsService)
        {

            //if(tipo == "buscaSimples")
            //{
                var salesRecords = await _salesRecordsService.
                                        GetByDateAsync(dateInitial,
                                                  dateFinal,
                                                  a => a.Date >= dateInitial,
                                                  a => a.Date <= dateFinal,
                                                  a => a.Seller,
                                                  a => a.Seller.Department,
                                                  a => a.Date
                                                  );

                return View(salesRecords);
            //}

            /*Criar uma nova página para que o modelo seja um IGrouping
            var salesRecordsGroup = await _salesRecordsService.
                                        GetByDateByGroupAsync(dateInitial,
                                                  dateFinal,
                                                  a => a.Date >= dateInitial,
                                                  a => a.Date <= dateFinal,
                                                  a => a.Seller,
                                                  a => a.Seller.Department,
                                                  a => a.Date
                                                  );

            ViewData["dateInitial"] = dateInitial;
            ViewData["dateFinal"] = dateFinal;

            return View(salesRecordsGroup);

            */
        }

        // GET: SalesRecords/Details/5
        public async Task<IActionResult> Details(int? id, [FromServices] RepositoryService<SalesRecord> _repositoryService)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _repositoryService.GetOneAsync(id, null, null);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // GET: SalesRecords/Delete/5
        public async Task<IActionResult> Delete(int? id, [FromServices] RepositoryService<SalesRecord> _repositoryService)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _repositoryService.GetOneAsync(id, null, null);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // POST: SalesRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [FromServices] RepositoryService<SalesRecord> _repositoryService)
        {
            var salesRecord = await _repositoryService.GetOneAsync(id, null, null);
            if (salesRecord != null)
            {
                await _repositoryService.RemoveAsync(salesRecord);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SalesRecordExists(int id, [FromServices] RepositoryService<SalesRecord> _repositoryService)
        {
            return await _repositoryService.ExistsAsync(s => s.Id == id);
        }
    }
}
