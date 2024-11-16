using Inventario_Hotel.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Inventario_Hotel.Context
{
    class AplicationDbContext
    {
        public class InventoryContext : DbContext
        {
            public DbSet<Producto> Products { get; set; }
            public DbSet<Almacenista> Almacenistas { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySQL("");
            }
        }
    }
}
