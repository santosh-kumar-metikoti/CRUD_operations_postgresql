using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication4.Models;
using Npgsql;
using System.Data;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string connString = "Host=localhost;Username=postgres;Password=1234;Database=itemsDB";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var dataset = new DataSet();
            using var connection = new NpgsqlConnection(connString);
            connection.Open();
            var query = @"SELECT * FROM public.""items""";

            using (var command = new NpgsqlCommand(query, connection))
            {

                var adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(dataset);
            }

            return View(dataset);
        }

        [HttpPost]
        public IActionResult Create(string name, string count, string size)
        {
            using var connection = new NpgsqlConnection(connString);
            connection.Open();
            string query = @"INSERT INTO public.""items""(""item_count"")VALUES ('" + count + "')";
            using var command = new NpgsqlCommand(query, connection);
            int result = command.ExecuteNonQuery();

            if (result < 0)
            {
                return Error();
            }
            return View(nameof(Create));

        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}