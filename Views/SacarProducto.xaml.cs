using Inventario_Hotel.Entities;
using Inventario_Hotel.Services;
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

namespace Inventario_Hotel.Views
{
    /// <summary>
    /// Lógica de interacción para SacarProducto.xaml
    /// </summary>
    public partial class SacarProducto : Window
    {
        private Producto _selectedProducto;
        private CrudProducto _crudProducto;
        Productos product = new Productos();

        public SacarProducto()
        {
            InitializeComponent();
            _crudProducto = new CrudProducto();
        }

        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoBusqueda.Text))
            {
                MessageBox.Show("Por favor, ingrese un código de producto.");
                return;
            }

            if (!int.TryParse(txtCodigoBusqueda.Text, out int codigo))
            {
                MessageBox.Show("El código de producto debe ser numérico.");
                return;
            }

            try
            {
                _selectedProducto = await _crudProducto.GetProductoByCodigoAsync(codigo);

                if (_selectedProducto != null)
                {
                    txtCodigo.Text = _selectedProducto.Codigo.ToString();
                    txtDescripcion.Text = _selectedProducto.Descripcion;
                    txtPieza.Text = _selectedProducto.Pieza.ToString();
                }
                else
                {
                    MessageBox.Show($"Producto con código {codigo} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar producto: {ex.Message}");
            }
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProducto == null)
            {
                MessageBox.Show("Por favor, busque y seleccione un producto primero.");
                return;
            }

            if (string.IsNullOrEmpty(txtCantidadSacar.Text))
            {
                MessageBox.Show("Por favor, ingrese la cantidad a sacar.");
                return;
            }

            if (!int.TryParse(txtCantidadSacar.Text, out int cantidadSacar))
            {
                MessageBox.Show("La cantidad a sacar debe ser numérica.");
                return;
            }

            if (cantidadSacar <= 0)
            {
                MessageBox.Show("La cantidad a sacar debe ser mayor que cero.");
                return;
            }

            if (_selectedProducto.Pieza < cantidadSacar)
            {
                MessageBox.Show("La cantidad a sacar no puede ser mayor que la cantidad en inventario.");
                return;
            }

            try
            {
                _selectedProducto.Pieza -= cantidadSacar;
                _selectedProducto.FechaSalida = DateTime.Now;

                await _crudProducto.UpdateProductoAsync(_selectedProducto);

                MessageBox.Show("Producto actualizado exitosamente!");
                await product.CargarProductosAsync();
                this.Close();
                product.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}");
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            product.Show();
        }
    }
}