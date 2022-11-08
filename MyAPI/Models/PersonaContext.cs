using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MyAPI.Models
{
    public class PersonaContext : DbContext
    {
        public PersonaContext(DbContextOptions<PersonaContext> options)
            : base(options)
        { }
        public DbSet<Persona> Persona { get; set; } = null!;
    }
}
