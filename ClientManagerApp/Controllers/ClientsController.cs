using ClientManagerApp.Models;
using ClientManagerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClientManagerApp.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDBContext _context;
        public ClientsController(ApplicationDBContext context)
        {
            _context = context;
        }


        //Метод создание клиентов
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        //Чтение клиентов
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                                        .FromSqlRaw("SELECT * FROM Clients")
                                        .ToListAsync();
            return View(clients);
        }


        //Метод обновления данных о клиентах
        [HttpPost]
        public async Task<IActionResult> UpdateClient(Client client)
        {
            var parameters = new[] {
        new SqlParameter("@Id", client.Id),
        new SqlParameter("@FirstName", client.FirstName ?? (object)DBNull.Value),
        new SqlParameter("@LastName", client.LastName ?? (object)DBNull.Value),
        new SqlParameter("@Email", client.Email ?? (object)DBNull.Value),
        new SqlParameter("@Phone", client.Phone ?? (object)DBNull.Value)
    };

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateClient @Id, @FirstName, @LastName, @Email, @Phone", parameters);
            return RedirectToAction("Index");
        }

        //Метод удаления клиента
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return RedirectToAction("Index", "Clients");
            }
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Clients");
        }




        // Метод для фильтрации клиентов по имени или фамилии
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var clients = from m in _context.Clients
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.FirstName.Contains(searchString)
                                       || s.LastName.Contains(searchString));
            }

            return View("Index", await clients.ToListAsync());
        }

        //Метод обрабатывает GET-запросы для получения данных клиента по идентификатору.
        [HttpGet]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return Json(client);
        }


        //Мтод обрабатывает GET-запросы для отображения детальной страницы клиента.
        [HttpGet]
        public async Task<IActionResult> ClientCard(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }


    }
}
