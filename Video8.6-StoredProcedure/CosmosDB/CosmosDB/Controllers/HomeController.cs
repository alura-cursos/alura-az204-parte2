using CosmosDB.Models;
using CosmosDB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosmosDB.Controllers
{
    public class HomeController : Controller
    {
		private readonly CosmosDbservice cosmosDbservice;

		public HomeController(CosmosDbservice cosmosDbservice)
		{
			this.cosmosDbservice = cosmosDbservice;
		}

		public IActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public IActionResult CreateItem()
		{
			List<string> categorias = new List<string>
			{
				"Cerificacao", "Gastronomia", "Tecnologia", "Saude"
			};

			ViewBag.Categorias = new SelectList(categorias);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateItem(Curso curso)
		{
			curso.id = Guid.NewGuid().ToString();
			await this.cosmosDbservice.AddNewItemAsync(curso);
			return RedirectToAction("ListAllItems");
		}

		[HttpGet]
		public async Task<IActionResult> ListAllItems()
		{
			var lista = await cosmosDbservice.GetAllItemsAsync();
			return View(lista);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateItem(string id, string categoria)
		{
			List<string> categorias = new List<string>
			{
				"Cerificacao", "Gastronomia", "Tecnologia", "Saude"
			};

			ViewBag.Categorias = new SelectList(categorias);

			var curso = await cosmosDbservice.FindItemAsync(id, categoria);
			return View(curso);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateItem(Curso curso)
		{
			await cosmosDbservice.UpdateItemAsync(curso);
			return RedirectToAction("ListAllItems");
		}

		[HttpGet]
		public async Task<IActionResult> RemoveItem(string id, string categoria)
		{
			var curso = await cosmosDbservice.FindItemAsync(id, categoria);
			return View(curso);
		}

		[HttpPost]
		public async Task<IActionResult> RemoveItem(Curso curso)
		{
			await cosmosDbservice.RemoveItemAsync(curso.id!, curso.categoria!);
			return RedirectToAction("ListAllItems");
		}

		[HttpGet]
		public IActionResult CreateItemSP()
		{
			List<string> categorias = new List<string>
			{
				"Cerificacao", "Gastronomia", "Tecnologia", "Saude"
			};

			ViewBag.Categorias = new SelectList(categorias);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateItemSP(Curso curso)
		{
			curso.id = Guid.NewGuid().ToString();
			await this.cosmosDbservice.AddNewItemSpAsync(curso);
			return RedirectToAction("ListAllItems");
		}
	}

}
