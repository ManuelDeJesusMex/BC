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
                optionsBuilder.UseMySQL("Server=bsbaye9m1zq9tmfw6egq-mysql.services.clever-cloud.com; database=bsbaye9m1zq9tmfw6egq; user=uhqcu4f66e13aovn; password=oalmMP7fdubenbBEazI8; port=3306;");
            }
        }
    }
}
