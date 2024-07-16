using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("ProductionExt")]
    public class ProductionExtController : Controller
    {
        private readonly ProductionContext _context;

        public ProductionExtController(ProductionContext context)
        {
            _context = context;
        }

        // GET: ProductionExt
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductionResults.ToListAsync());
        }

        // POST: ProductionExt/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ProductionPlanComplete plan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate total production and workdays
                    int[] production = { plan.Monday, plan.Tuesday, plan.Wednesday, plan.Thursday, plan.Friday, plan.Saturday, plan.Sunday };
                    
                    _context.Add(plan);
                    await _context.SaveChangesAsync();

                    var result = BalanceProduction(production);
                    
                    var productionResult = new ProductionResult
                    {
                        ProductionPlanId = plan.Id,
                        Monday = result[0],
                        Tuesday = result[1],
                        Wednesday = result[2],
                        Thursday = result[3],
                        Friday = result[4],
                        Saturday = result[5],
                        Sunday = result[6],
                        CreatedAt = DateTime.Now
                    };

                    _context.Add(productionResult);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, data = productionResult });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Invalid model state" });
        }


        private int[] BalanceProduction(int[] dailyProduction)
        {
            int totalProduction = dailyProduction.Where(x => x > 0).Sum();
            int activeDaysCount = dailyProduction.Count(x => x > 0);

            if (activeDaysCount == 0)
            {
                return dailyProduction;
            }

            int averageProduction = totalProduction / activeDaysCount;
            int remainder = totalProduction % activeDaysCount;

            int[] balancedProduction = new int[7];
            for (int i = 0; i < 7; i++)
            {
                if (dailyProduction[i] > 0)
                {
                    balancedProduction[i] = averageProduction;
                }
                else
                {
                    balancedProduction[i] = 0;
                }
            }

            var orderedDays = dailyProduction
                .Select((value, index) => new { value, index })
                .Where(x => x.value > 0) 
                .OrderByDescending(x => x.value)
                .Take(remainder);

            foreach (var day in orderedDays)
            {
                balancedProduction[day.index]++;
            }

            return balancedProduction;
        }








        // GET: ProductionExt/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var productionPlan = await _context.ProductionPlansComplete
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionPlan == null)
            {
                return Json(new { success = false });
            }

            //var productionResult = await _context.ProductionResults
            //    .FirstOrDefaultAsync(r => r.ProductionPlanId == id);

            return Json(new { success = true, data = productionPlan });
        }

        // DELETE: ProductionExt/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _context.ProductionResults.FindAsync(id);
            if (plan == null)
            {
                return Json(new { success = false });
            }

            var result = await _context.ProductionResults
                .FirstOrDefaultAsync(r => r.ProductionPlanId == id);
            if (result != null)
            {
                _context.ProductionResults.Remove(result);
            }

            _context.ProductionResults.Remove(plan);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private ProductionResult AdjustProductionPlan(ProductionPlanComplete plan)
        {
            int totalProduction = plan.Monday + plan.Tuesday + plan.Wednesday + plan.Thursday + plan.Friday + plan.Saturday + plan.Sunday;
            int totalDays = 7;
            int averageProduction = totalProduction / totalDays;

            var result = new ProductionResult
            {
                Monday = plan.Monday > 0 ? averageProduction : 0,
                Tuesday = plan.Tuesday > 0 ? averageProduction : 0,
                Wednesday = plan.Wednesday > 0 ? averageProduction : 0,
                Thursday = plan.Thursday > 0 ? averageProduction : 0,
                Friday = plan.Friday > 0 ? averageProduction : 0,
                Saturday = plan.Saturday > 0 ? averageProduction : 0,
                Sunday = plan.Sunday > 0 ? averageProduction : 0,
                CreatedAt = DateTime.Now
            };

            int remainder = totalProduction - (averageProduction * totalDays);
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            foreach (var day in days.OrderByDescending(day => plan.GetType().GetProperty(day).GetValue(plan)))
            {
                if (remainder <= 0) break;
                result.GetType().GetProperty(day).SetValue(result, averageProduction + 1);
                remainder--;
            }

            return result;
        }
    }
}
