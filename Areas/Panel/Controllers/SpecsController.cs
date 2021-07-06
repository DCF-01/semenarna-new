using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize]
    [Area("Panel")]
    public class SpecsController : Controller {
        private ApplicationDbContext _ctx;
        public SpecsController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index() {
            var all_specs = _ctx.Specs.ToList();


            return View(all_specs);
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SpecViewModel specViewModel) {
            if (specViewModel.Name == null) {
                throw new Exception("Name cannot be empty");
            }

            List<string> active_columns = new List<string>();
            foreach (var i in specViewModel.First) {
                if (i != null) {
                    active_columns.Add(i);
                }

            }

            //checked arr
            List<string> rest = new List<string>();
            for (int i = 0; i < specViewModel.Rest.Length; i++) {
                if (specViewModel.Rest[i] != null) {
                    rest.Add(specViewModel.Rest[i]);
                }
            }

            var spec = new Spec {
                First = active_columns.ToArray(),
                Rest = rest.ToArray(),
                ItemsPerRow = active_columns.Count(),
                Name = specViewModel.Name
            };

            await _ctx.Specs.AddAsync(spec);
            await _ctx.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id) {
            try {
                var spec = await _ctx.Specs.FindAsync(id);

                return View(spec);

            }
            catch (Exception e) {
                return View("An error has ocurred.", e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Manage(int id, SpecViewModel specViewModel) {
            if (specViewModel.Name == null) {
                throw new Exception("Name cannot be empty");
            }

            var spec = await _ctx.Specs.FindAsync(id);

            List<string> active_columns = new List<string>();
            foreach (var i in specViewModel.First) {
                if (i != null) {
                    active_columns.Add(i);
                }

            }

            //checked arr
            List<string> rest = new List<string>();
            for (int i = 0; i < specViewModel.Rest.Length; i++) {
                if (specViewModel.Rest[i] != null) {
                    rest.Add(specViewModel.Rest[i]);
                }
            }

            spec.First = active_columns.ToArray();
            spec.Rest = rest.ToArray();
            spec.ItemsPerRow = active_columns.Count();
            spec.Name = specViewModel.Name;


            await _ctx.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            try {
                //delete product
                var spec = _ctx.Specs.Find(id);

                if (spec != null) {
                    _ctx.Specs.Remove(spec);
                    await _ctx.SaveChangesAsync();
                    return Ok();
                }
                else {
                    return StatusCode(500);
                }
            }
            catch (Exception e) {
                //used only for spec foreign key conflict
                return StatusCode(450, e);
            }
        }
    }
}
