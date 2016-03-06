using System;
using System.Linq;
using System.Web.Mvc;
using InventoryAllocationEngine.Web.Models;
using InventoryAllocationEngine.Web.Services;
using InventoryAllocationEngine.Web.Services.Enumerations;

namespace InventoryAllocationEngine.Web.Controllers
{
   public class ProductsController : Controller
   {
      private readonly IAEContext dbContext;
      private readonly AllocationService allocationService;

      public ProductsController()
      {
         dbContext = new IAEContext();
         allocationService = new AllocationService(dbContext);
      }

      public ActionResult Index()
      {
         return View(dbContext.Products.OrderBy(p => p.Description));
      }

      public ActionResult Details(Guid id)
      {
         return View(dbContext.Products.Find(id));
      }

      [HttpPost]
      public ActionResult Allocate(Guid id, AllocationMethod allocationMethod, double weighting)
      {

         allocationService.Allocate(id, allocationMethod, weighting);

         return View("Details", dbContext.Products.Find(id));
      }
   }
}