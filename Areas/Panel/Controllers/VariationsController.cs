using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class VariationsController : Controller {
        readonly ApplicationDbContext _ctx;

        public VariationsController(ApplicationDbContext context) {
            _ctx = context;
        }

        [HttpGet]
        public IActionResult Index() {

            var variations = _ctx.Variations.ToList();


            return View(variations);
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(VariationViewModel variationViewModel) {


            var variation = new Variation {
                Name = variationViewModel.Name,
                Options = variationViewModel.Options
            };
            await _ctx.Variations.AddAsync(variation);
            await _ctx.SaveChangesAsync();

            return View();
        }


        [HttpDelete]
        public IActionResult Delete([FromRoute] int id) {
            try {
                //delete product
                var variation = _ctx.Variations.Find(id);

                if (variation != null) {
                    _ctx.Variations.Remove(variation);
                    _ctx.SaveChanges();
                    return Ok();
                }
                else {
                    return StatusCode(500);
                }
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

    }
}
