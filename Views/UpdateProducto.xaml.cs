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
    /// Lógica de interacción para UpdateProducto.xaml
    /// </summary>
    public partial class UpdateProducto : Window
    {
        private CrudProducto _crudProducto;
        private Producto _selectedProducto;
        Productos product = new Productos();
        //Producto produc = new Producto();


        public UpdateProducto(Producto selectedProducto)
        {
            InitializeComponent();
            _crudProducto = new CrudProducto();
            _selectedProducto = selectedProducto;
            //DataContext = produc;
            CargarProductosAsync();
        }

        public async Task CargarProductosAsync()
        {
            try
            {
                List<Producto> productos = await _crudProducto.GetProductosAsync();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            product.Show();
            Close();
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
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

                int Codigo = int.Parse(txtCodigo.Text);
                int minPiezas = int.Parse(txtMinPiezas.Text);
                int maxPiezas = int.Parse(txtMaxPiezas.Text);
                int Pieza = int.Parse(txtPieza.Text);

                if (minPiezas > maxPiezas)
                {
                    MessageBox.Show("El stock mínimo no puede ser mayor que el stock máximo.");
                    return;
                }
                else if( maxPiezas < minPiezas)
                {
                    MessageBox.Show("El stock máximo no puede ser menor que el stock mínimo.");
                    return;
                }
                else if( Pieza > maxPiezas)
                {
                    MessageBox.Show("Las piezas no pueden ser mayor que el stock máximo.");
                    return;
                }
                else if (Codigo < 0 || Pieza <= 0 || minPiezas <= 0 || maxPiezas <= 0)
                {
                    MessageBox.Show("No puede haber valores menores o igual a 0.");
                    return;
                }
                _selectedProducto.Codigo = int.Parse(txtCodigo.Text);
                _selectedProducto.Descripcion = txtDescripcion.Text;
                _selectedProducto.Pieza = int.Parse(txtPieza.Text);
                //_selectedProducto.Stock = int.Parse(txtStock.Text); 
                _selectedProducto.MinPiezas = int.Parse(txtMinPiezas.Text);
                _selectedProducto.MaxPiezas = int.Parse(txtMaxPiezas.Text);

                await Task.Run(() => _crudProducto.UpdateProductoAsync(_selectedProducto));
                MessageBox.Show("Producto actualizado exitosamente!");
                await product.CargarProductosAsync();
                this.Close();
                product.Show();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error de Actualización");
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos para las cantidades.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}");
            }
        }
    }
}
