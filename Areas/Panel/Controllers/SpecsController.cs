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
            if(specViewModel.Name == null) {
                throw new Exception("Name cannot be empty");
            }
            string spec_name = specViewModel.Name;
            List<string> first_col = new List<string>();
            List<string> second_col = new List<string>();
            List<string> third_col = new List<string>();
            List<string> fourth_col = new List<string>();

            foreach (var item in specViewModel.First) {
                if (item != null) {
                    first_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Second) {
                if (item != null) {
                    second_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Third) {
                if (item != null) {
                    third_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Fourth) {
                if (item != null) {
                    fourth_col.Add(item);
                }
            }

            var spec = new Spec {
                Name = spec_name,
                First = first_col.ToArray(),
                Second = second_col.ToArray(),
                Third = third_col.ToArray(),
                Fourth = fourth_col.ToArray()
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
                return View("An error has ocurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Manage(int id, SpecViewModel specViewModel) {
            if (specViewModel.Name == null) {
                throw new Exception("Name cannot be empty");
            }
            //spec entity to be updated
            var spec = await _ctx.Specs.FindAsync(id);

            //helper vars
            string spec_name = specViewModel.Name;
            List<string> first_col = new List<string>();
            List<string> second_col = new List<string>();
            List<string> third_col = new List<string>();
            List<string> fourth_col = new List<string>();

            foreach (var item in specViewModel.First) {
                if (item != null) {
                    first_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Second) {
                if (item != null) {
                    second_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Third) {
                if (item != null) {
                    third_col.Add(item);
                }
            }
            foreach (var item in specViewModel.Fourth) {
                if (item != null) {
                    fourth_col.Add(item);
                }
            }

            spec.Name = spec_name;
            spec.First = first_col.ToArray();
            spec.Second = second_col.ToArray();
            spec.Third = third_col.ToArray();
            spec.Fourth = fourth_col.ToArray();

            await _ctx.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
