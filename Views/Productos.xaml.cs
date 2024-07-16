using Inventario_Hotel.Entities;
using Inventario_Hotel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Window, INotifyPropertyChanged
    {
        private CrudProducto _crudProducto;
        private ObservableCollection<Producto> _productos;
        private Producto _selectedProducto;

        public ObservableCollection<Producto> ProductosList
        {
            get { return _productos; }
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }

        public Producto SelectedProducto
        {
            get { return _selectedProducto; }
            set
            {
                _selectedProducto = value;
                OnPropertyChanged();
            }
        }

        public Productos()
        {
            InitializeComponent();
            _crudProducto = new CrudProducto();
            BtnUpdate = new RelayCommand<Producto>(async (producto) => await UpdateProductoAsync(producto));
            BtnDelete = new RelayCommand<Producto>(async (producto) => await DeleteProductoAsync(producto));
            DataContext = this;
            CargarProductosAsync();
        }

        public ICommand BtnUpdate { get; }
        public ICommand BtnDelete { get; }

        public async Task CargarProductosAsync()
        {
            try
            {
                List<Producto> productos = await _crudProducto.GetProductosAsync();
                ProductosList = new ObservableCollection<Producto>(productos);
                dataGridProducts.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private async Task UpdateProductoAsync(Producto codigo)
        {
            try
            {
                // Crear una nueva ventana de actualización
                UpdateProducto updateProduct = new UpdateProducto(codigo);

                // Establecer el DataContext de la ventana de actualización con el producto seleccionado
                updateProduct.DataContext = codigo;

                // Mostrar la ventana de actualización de forma modal
                this.Close();
                updateProduct.Show();

                await CargarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}");
            }
        }

        private async Task DeleteProductoAsync(Producto producto)
        {
            MessageBoxResult result = MessageBox.Show($"¿Está seguro que desea eliminar el producto {producto.Descripcion}?",
                                                      "Confirmación",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _crudProducto.DeleteProductoAsync(producto.Id);
                    MessageBox.Show("Producto eliminado exitosamente!");
                    await CargarProductosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar producto: {ex.Message}");
                }
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto agregarproductos = new AgregarProducto();
            agregarproductos.Show();
            Close();
        }

        private void Ver_Click(object sender, RoutedEventArgs e)
        {
            Productos productos = new Productos();
            productos.Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<T, bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }
        }

        private async void tbnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusqueda.Text.Trim()))
            {
                MessageBox.Show("Por favor, ingrese un código de producto.");
                return;
            }

            int codigo;
            if (!int.TryParse(txtBusqueda.Text.Trim(), out codigo))
            {
                MessageBox.Show("El código de producto debe ser numérico.");
                return;
            }

            try
            {
                Producto productoEncontrado = await _crudProducto.GetProductoByCodigoAsync(codigo);

                if (productoEncontrado != null)
                {
                    // Mostrar el producto encontrado en el DataGrid
                    List<Producto> productos = new List<Producto> { productoEncontrado };
                    dataGridProducts.ItemsSource = new ObservableCollection<Producto>(productos);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SacarProducto sacar = new SacarProducto();
            this.Close();
            sacar.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow sesion = new MainWindow();
            sesion.Show();
            Close();
        }
    }

}

   
