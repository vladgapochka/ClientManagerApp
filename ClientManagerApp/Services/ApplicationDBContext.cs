using ClientManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClientManagerApp.Services
{
    public class ApplicationDBContext: IdentityDbContext<AppUser>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
    }
}
