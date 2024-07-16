using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductionController : Controller
    {
        // GET: Production
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ProductionPlan plan)
        {
            int[] production = { plan.Monday, plan.Tuesday, plan.Wednesday, plan.Thursday, plan.Friday };
            int total = production.Sum();
            int average = total / 5;
            int remainder = total % 5;

            int[] result = new int[5];
            for (int i = 0; i < 5; i++)
            {
                result[i] = average;
            }

            var orderedProduction = production
                                    .Select((value, index) => new { value, index })
                                    .OrderByDescending(x => x.value)
                                    .ToList();

            for (int i = 0; i < remainder; i++)
            {
                result[orderedProduction[i].index]++;
            }

            ViewBag.Result = result;

            return View(plan);
        }
    }
}
