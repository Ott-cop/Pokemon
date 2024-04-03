using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Models;
using System.Text.Json;

namespace Pokemon.Controllers;

public class HomeController : Controller
{

    private List<Pokemon.Models.Pokemon> GetPokemons()
    {
        List<Pokemon.Models.Pokemon> pokemons = [];

        using(StreamReader leitor = new("Data/pokemons.json"))
        {
            string dados = leitor.ReadToEnd();
            pokemons = JsonSerializer.Deserialize<List<Pokemon.Models.Pokemon>>(dados);
        }
        
        return pokemons;
    }

    private List<Tipo> GetTipos()
    {
        List<Tipo> tipos = [];

        using (StreamReader leitor = new("Data/tipos.json"))
        {
            string dados = leitor.ReadToEnd();
            tipos = JsonSerializer.Deserialize<List<Tipo>>(dados);
        }

        return tipos;
    }
    private readonly ILogger<HomeController> _logger;
    
    

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Pokemon.Models.Pokemon> pokemons = GetPokemons();
        
        List<Tipo> tipos = GetTipos();

        ViewData["Tipos"] = tipos;

        return View(pokemons);
    }

    public IActionResult Details(int id)
    {
        List<Pokemon.Models.Pokemon> pokemons = GetPokemons();
        
        List<Tipo> tipos = GetTipos();

        ViewData["Tipos"] = tipos;

        DetailsVM details = new() {
            Anterior = pokemons.OrderByDescending(p => p.Numero).FirstOrDefault(p => p.Numero < id),
            Atual = pokemons.FirstOrDefault(p => p.Numero == id),
            Proximo = pokemons.OrderBy(p => p.Numero).FirstOrDefault(p => p.Numero > id),
            Tipos = tipos
        };
        
        return View(details);

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
