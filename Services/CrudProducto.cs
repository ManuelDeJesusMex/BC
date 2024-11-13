using Inventario_Hotel.Entities;
using Inventario_Hotel.Views;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using static Inventario_Hotel.Context.AplicationDbContext;
using Document = MigraDoc.DocumentObjectModel.Document;
using System.IO;

namespace Inventario_Hotel.Services
{
    public class CrudProducto : Producto
    {
        private readonly InventoryContext _context;

        public CrudProducto()
        {
            _context = new InventoryContext();
            _context.Database.EnsureCreated();
        }

        public async Task AddProductoAsync(Producto producto)
        {
            var allProducts = await _context.Products.ToListAsync();

            if (allProducts.Any(p => p.Codigo == producto.Codigo))
            {
                throw new InvalidOperationException("El código del producto ya está registrado.");
            }
            if (allProducts.Any(p => Similar(p.Descripcion, producto.Descripcion)))
            {
                throw new InvalidOperationException("El producto con la misma descripcion ya está registrado, por favor modifique el producto registrado.");
            }

            producto.FechaEntrada = DateTime.Now;
            _context.Products.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductoAsync(Producto updatedProducto)
        {
            try
            {
                var producto = await _context.Products.FindAsync(updatedProducto.Id);
    
                // Verifica si algún otro producto ya tiene el mismo código
                var mismocodigo = await _context.Products
               .Where(p => p.Id != updatedProducto.Id && p.Codigo == updatedProducto.Codigo)
               .FirstOrDefaultAsync();
                if (mismocodigo != null)
                {
                    throw new InvalidOperationException("El código del producto ya está registrado para otro producto.");
                }
                producto.Codigo = updatedProducto.Codigo;
                producto.Descripcion = updatedProducto.Descripcion;
                producto.Pieza = updatedProducto.Pieza;
                producto.MinPiezas = updatedProducto.MinPiezas;
                producto.MaxPiezas = updatedProducto.MaxPiezas;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteProductoAsync(int id)
        {
            var producto = _context.Products.Find(id);
            if (producto != null)
            {
                _context.Products.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Producto>> GetProductosAsync() //De forma asyncrona para esperar a que los datos se guarden y luego traerlos
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Producto> GetProductoByCodigoAsync(int codigo)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.Codigo == codigo);
        }

        private bool Similar(string desc1, string desc2)
        {
            int levenshteinDistance = Calcularletras(desc1.ToLower(), desc2.ToLower());
            int length = Math.Max(desc1.Length, desc2.Length);
            double similarity = (double)(length - levenshteinDistance) / length;

            // Ajustar umbral
            return similarity >= 0.8;
        }

        private int Calcularletras(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target.Length;
            if (string.IsNullOrEmpty(target)) return source.Length;

            int[,] distance = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; distance[i, 0] = i++) { }
            for (int j = 0; j <= target.Length; distance[0, j] = j++) { }

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[source.Length, target.Length];
        }
        public async Task<List<int>> GetAniosDisponiblesAsync()
        {
            return await _context.Products
                                 .Select(p => p.FechaEntrada.Year)
                                 .Distinct()
                                 .ToListAsync();
        }

        // Método para obtener los meses disponibles según el año
        public async Task<List<int>> GetMesesPorAnioAsync(int anio)
        {
            return await _context.Products
                                 .Where(p => p.FechaEntrada.Year == anio)
                                 .Select(p => p.FechaEntrada.Month)
                                 .Distinct()
                                 .ToListAsync();
        }

        // Método para obtener los datos del reporte
        public async Task<List<Producto>> GetProductosPorMesAsync(int anio, int mes)
        {
            return await _context.Products
                                 .Where(p => p.FechaEntrada.Year == anio && p.FechaEntrada.Month == mes)
                                 .ToListAsync();
        }

        // Método para generar el reporte PDF
        public async Task GenerarReportePDFAsync(int anio, int mes)
        {
            var productos = await GetProductosPorMesAsync(anio, mes);

            if (productos.Count == 0)
            {
                throw new InvalidOperationException("No se encontraron datos para generar el reporte.");
            }

            Document document = new Document();
            Section section = document.AddSection();

            // Título del reporte
            section.AddParagraph("Reporte de Productos").Format.Font.Size = 18;
            section.AddParagraph($"Año: {anio} - Mes: {mes}").Format.Font.Size = 14;
            section.AddParagraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy"));

            // Tabla de datos
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.AddColumn("4cm").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("6cm").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("2cm").Format.Alignment = ParagraphAlignment.Center;

            var headerRow = table.AddRow();
            headerRow.Cells[0].AddParagraph("Código");
            headerRow.Cells[1].AddParagraph("Descripción");
            headerRow.Cells[2].AddParagraph("Piezas");

            foreach (var producto in productos)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(producto.Codigo.ToString());
                row.Cells[1].AddParagraph(producto.Descripcion);
                row.Cells[2].AddParagraph(producto.Pieza.ToString());
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            renderer.RenderDocument();

            string filename = $"Reporte_{anio}_{mes}.pdf";
            renderer.PdfDocument.Save(filename);

            if (File.Exists(filename))
            {
                System.Diagnostics.Process.Start("explorer.exe", filename);
            }

        }
    }
}