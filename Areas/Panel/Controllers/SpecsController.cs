﻿using Microsoft.AspNetCore.Authorization;
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
            List<string> first_row = new List<string>();
            List<string> second_row = new List<string>();
            List<string> third_row = new List<string>();
            List<string> fourth_row = new List<string>();

            foreach (var item in specViewModel.First) {
                if (item != null) {
                    first_row.Add(item);
                }
            }
            foreach (var item in specViewModel.Second) {
                if (item != null) {
                    second_row.Add(item);
                }
            }
            foreach (var item in specViewModel.Third) {
                if (item != null) {
                    third_row.Add(item);
                }
            }
            foreach (var item in specViewModel.Fourth) {
                if (item != null) {
                    fourth_row.Add(item);
                }
            }

            var spec = new Spec {
                Name = spec_name,
                First = first_row.ToArray(),
                Second = second_row.ToArray(),
                Third = third_row.ToArray(),
                Fourth = fourth_row.ToArray()
            };

            await _ctx.Specs.AddAsync(spec);
            await _ctx.SaveChangesAsync();
            
            return View();
        }
    }
}
