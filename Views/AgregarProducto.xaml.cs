using Inventario_Hotel.Entities;
using Inventario_Hotel.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Inventario_Hotel.Context.AplicationDbContext;

namespace Inventario_Hotel.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : Window
    {
        private CrudProducto _crudProducto;
        Productos product = new Productos();

        public AgregarProducto()
        {
            InitializeComponent();
            _crudProducto = new CrudProducto();
            DataContext = this;
            product.CargarProductosAsync();
        }           

        private async void btnAdd_Click_1(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtCodigo.Text) ||
                string.IsNullOrEmpty(txtDescripcion.Text) ||
                string.IsNullOrEmpty(txtPieza.Text) ||
                //string.IsNullOrEmpty(txtStock.Text) ||
                string.IsNullOrEmpty(txtMinPiezas.Text) ||
                string.IsNullOrEmpty(txtMaxPiezas.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            try
            {

                int minPiezas = int.Parse(txtMinPiezas.Text);
                int maxPiezas = int.Parse(txtMaxPiezas.Text);
                int Pieza = int.Parse(txtPieza.Text);
                int Codigo = int.Parse(txtCodigo.Text);

                if (minPiezas > maxPiezas)
                {
                    MessageBox.Show("El stock mínimo no puede ser mayor que el stock máximo.");
                    return;
                }
                else if (maxPiezas < minPiezas)
                {
                    MessageBox.Show("El stock máximo no puede ser menor que el stock mínimo.");
                    return;
                }
                else if (Pieza > maxPiezas)
                {
                    MessageBox.Show("Las piezas no pueden ser mayor que el stock máximo.");
                    return;
                }
                else if (Codigo < 0 || Pieza <= 0 || minPiezas <= 0 || maxPiezas <= 0)
                {
                    MessageBox.Show("No puede haber valores menores o igual a 0.");
                    return;
                }

                var producto = new Producto
                {
                    Codigo = int.Parse(txtCodigo.Text),
                    Descripcion = txtDescripcion.Text,
                    Pieza = int.Parse(txtPieza.Text),
                    MinPiezas = int.Parse(txtMinPiezas.Text),
                    MaxPiezas = int.Parse(txtMaxPiezas.Text),
                    FechaEntrada = DateTime.Now
                };
                await Task.Run(() => _crudProducto.AddProductoAsync(producto));
                MessageBox.Show("Producto agregado exitosamente!");
                await product.CargarProductosAsync();
                this.Close();
                product.Show();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos para las cantidades.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}");
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            product.Show();
        }
    }

}