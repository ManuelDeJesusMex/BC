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
    /// Lógica de interacción para GenerarReporte.xaml
    /// </summary>
    public partial class GenerarReporte : Window
    {

        private readonly CrudProducto _crudProducto;
        public GenerarReporte()
        {
            InitializeComponent();
            _crudProducto = new CrudProducto();
            CargarAnios();
        }

        private async void CargarAnios()
        {
            var anios = await _crudProducto.GetAniosDisponiblesAsync();
            cbAnios.ItemsSource = anios;
        }

        private async void cbAnios_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            lbMeses.Items.Clear();
            if (cbAnios.SelectedItem != null)
            {
                int anioSeleccionado = (int)cbAnios.SelectedItem;
                var meses = await _crudProducto.GetMesesPorAnioAsync(anioSeleccionado);
                lbMeses.ItemsSource = meses;
            }
        }

        private async void GenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            if (cbAnios.SelectedItem != null && lbMeses.SelectedItem != null)
            {
                int anio = (int)cbAnios.SelectedItem;
                int mes = (int)lbMeses.SelectedItem;

                try
                {
                    await _crudProducto.GenerarReportePDFAsync(anio, mes);
                    MessageBox.Show("Reporte generado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar el reporte: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un año y un mes.");
            }
        }
    }
}
