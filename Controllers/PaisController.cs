using System.Diagnostics;
using System.Threading.Tasks;
using Bussines.Dtos.PaisDto;
using Bussines.Services;
using Bussines.ViewModel.PaisVM;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using Persistence.Repositories;

namespace InvestmentApp.Controllers
{
    public class PaisController : Controller
    {
        private readonly PaisService _paisService;

        public PaisController(InvestContext context)
        { 
            _paisService = new PaisService(context);
        }

        public async Task<IActionResult> Index()
        {
            var tso = await _paisService.GetAllWithInclude();

            var listEntitiesVMs = tso.Select(p => new PaisViewModel
            {
                Id = p.Id,
                CodigoIso = p.CodigoIso,
                RetornoId = p.RetornoId ?? 0,
                Nombre = p.Nombre
            }).ToList();
            return View(listEntitiesVMs);
        }

        public IActionResult Add()
        {
            return View("Save", new SavePaisViewModel() { Nombre = "", CodigoIso = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SavePaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }
            PaisDto dto = new() { Id = 0, CodigoIso = vm.CodigoIso, Nombre = vm.Nombre };
            var result = await _paisService.AddAsync(dto);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to save Pais. Please try again.");
                return View("Save", vm);
            }
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var dto = await _paisService.GetByIdAsync(id);
            if(dto == null)
            {
                return RedirectToRoute(new { controller = "Pais", action = "Index" });
            }
            SavePaisViewModel vm = new() { CodigoIso = dto.CodigoIso, Nombre = dto.Nombre, Id  = dto.Id };
            return View("Save", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePaisViewModel vm)
        {
            if (!ModelState.IsValid || vm.Id == null)
            {
                return View("Save", vm);
            }
            PaisDto dto = new() { CodigoIso = vm.CodigoIso, Nombre = vm.Nombre, Id = (int)vm.Id };
            var result = await _paisService.UpdateAsync(dto);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update Pais. Please try again.");
                return View("Save", vm);
            }
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {

            ViewBag.EditMode = true;
            var dto = await _paisService.GetByIdAsync(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Pais", action = "Index" });
            }
            RemovePaisViewModel vm = new() {Nombre = dto.Nombre, Id = dto.Id };
            return View(vm);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(RemovePaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _paisService.DeleteAsync(vm.Id);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete Pais. Please try again.");
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }
    }

}
