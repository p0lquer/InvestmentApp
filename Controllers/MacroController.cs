using System.Diagnostics;
using Bussines.Dtos.MacroDto;
using Bussines.Dtos.PaisDto;
using Bussines.Services;
using Bussines.ViewModel.MacroVM;
using Bussines.ViewModel.PaisVM;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace InvestmentApp.Controllers
{
    public class MacroController : Controller
    {
            private readonly MacroService _macroService;

            public MacroController(InvestContext context)
            {
                _macroService = new MacroService(context);
            }

            public async Task<IActionResult> Index()
            {
                var tso = await _macroService.GetAll();

            var listEntitiesVMs = tso.Select(p => new MacroViewModel
            {
                Id = p.Id,
                MacroInd = p.MacroInd,
                Peso = p.Peso,
                Nombre = p.Nombre
            }).ToList();
            return View(listEntitiesVMs);
        }

            public IActionResult Add()
            {
                return View("Save", new SaveMacroViewModel() { Nombre = "", MacroInd = false, Peso = 0});
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Add(SaveMacroViewModel vm)
            {
                if (!ModelState.IsValid)
                {
                    return View("Save", vm);
                }
                MacroDto dto = new() { Id = 0, Nombre = vm.Nombre, MacroInd = vm.MacroInd, Peso = vm.Peso };
                var result = await _macroService.AddAsync(dto);
                if (!result)
                {
                    ModelState.AddModelError("", "Failed to save Macro. Please try again.");
                    return View("Save", vm);
                }
                return RedirectToRoute(new { controller = "Macro", action = "Index" });
            }

            public async Task<IActionResult> Edit(int id)
            {
                ViewBag.EditMode = true;
                var dto = await _macroService.GetByIdAsync(id);
                if (dto == null)
                {
                    return RedirectToRoute(new { controller = "Macro", action = "Index" });
                }
                SaveMacroViewModel vm = new() { MacroInd = dto.MacroInd, Peso = dto.Peso, Nombre = dto.Nombre, Id = dto.Id };
                return View("Save", vm);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(SaveMacroViewModel vm)
            {
                if (!ModelState.IsValid || vm.Id == null)
                {
                    return View("Save", vm);
                }
                MacroDto dto = new() { MacroInd = vm.MacroInd, Peso = vm.Peso, Nombre = vm.Nombre, Id = (int)vm.Id };
                var result = await _macroService.UpdateAsync(dto);
                if (!result)
                {
                    ModelState.AddModelError("", "Failed to update Macro. Please try again.");
                    return View("Save", vm);
                }
                return RedirectToRoute(new { controller = "Macro", action = "Index" });
            }


            public async Task<IActionResult> Delete(int id)
            {

                ViewBag.EditMode = true;
                var dto = await _macroService.GetByIdAsync(id);
                if (dto == null)
                {
                    return RedirectToRoute(new { controller = "Macro", action = "Index" });
                }
                RemoveMacroViewModel vm = new() { Nombre = dto.Nombre, Id = dto.Id };
                return View(vm);


            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeletePost(RemoveMacroViewModel vm)
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var result = await _macroService.DeleteAsync(vm.Id);
                if (!result)
                {
                    ModelState.AddModelError("", "Failed to delete Macro. Please try again.");
                    return View(vm);
                }
                return RedirectToRoute(new { controller = "Macro", action = "Index" });
            }
        }


}
