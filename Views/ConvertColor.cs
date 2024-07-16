using Inventario_Hotel.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Inventario_Hotel.Views
{
    public class ConvertColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var producto = value as Producto;
            if (producto == null)
                return Brushes.Transparent;

            if (producto.Pieza <= producto.MinPiezas)
                return Brushes.Red;
            else if (producto.Pieza == producto.MaxPiezas)
                return Brushes.Green;
            else if (producto.Pieza > producto.MinPiezas || producto.Pieza < producto.MaxPiezas)
                return Brushes.Transparent;
            else
                return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
