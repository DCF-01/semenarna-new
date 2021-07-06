using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class PromotionsController : Controller {
        readonly ApplicationDbContext _ctx;
        public PromotionsController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        public async Task<IActionResult> Index() {

            var all_promotions = await _ctx.Promotions.ToListAsync();

            return View(all_promotions);
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id) {

            var current_promotion = await _ctx.Promotions.FindAsync(id);

            var model = new PromotionViewModel {
                Name = current_promotion.Name,
                Text = current_promotion.Text,
                Price = current_promotion.Price,
                DateFrom = current_promotion.DateFrom,
                DateTo = current_promotion.DateTo,
                Active = current_promotion.Active,
                GetImg = Convert.ToBase64String(current_promotion.Img)
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(int id, PromotionViewModel viewModel) {



            var current_promotion = await _ctx.Promotions.FindAsync(id);
            current_promotion.Name = viewModel.Name;
            current_promotion.DateTo = viewModel.DateTo;
            current_promotion.Text = viewModel.Text;
            current_promotion.Price = viewModel.Price;

            if (viewModel.Active) {
                var active_promotions = await _ctx.Promotions
                .Where(p => p.Active == true)
                .Select(p => p)
                .ToListAsync();

                if (active_promotions != null) {
                    foreach (var item in active_promotions) {
                        item.Active = false;
                    }
                }
            }

            current_promotion.Active = viewModel.Active;

            if (viewModel.Img != null) {
                current_promotion.Img = await viewModel.Img.GetBytesAsync();
            }

            await _ctx.SaveChangesAsync();

            return RedirectToAction("Index", "Promotions");
        }


        [HttpPost]
        public async Task<IActionResult> Create(PromotionViewModel viewModel) {
            var new_promotion = new Promotion {
                Name = viewModel.Name,
                Text = viewModel.Text,
                Price = viewModel.Price,
                DateTo = viewModel.DateTo,
                DateFrom = DateTime.Now,
                Img = (await viewModel.Img.GetBytesAsync()).CompressBytes(),
                Active = viewModel.Active
            };
            if (viewModel.Active) {
                var active_promotions = await _ctx.Promotions
                .Where(p => p.Active == true)
                .Select(p => p)
                .ToListAsync();

                if (active_promotions != null) {
                    foreach (var item in active_promotions) {
                        item.Active = false;
                    }
                }
            }

            _ctx.Promotions.Add(new_promotion);
            await _ctx.SaveChangesAsync();

            return RedirectToAction("Create", "Promotions");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id) {

            try {
                var promotion = await _ctx.Promotions.FindAsync(id);
                
                if(promotion != null && !promotion.Active) {
                    _ctx.Remove(promotion);
                    await _ctx.SaveChangesAsync();

                    return Ok();
                }

                else {
                    return BadRequest();
                }

            
                
            }

            catch (Exception e) {
                return BadRequest(e);
            }

        }
    }
}
