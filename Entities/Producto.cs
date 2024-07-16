using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Hotel.Entities
{
    public class Producto : INotifyPropertyChanged
    {
        private int _pieza;
        private int _minPiezas;
        private int _maxPiezas;

        [Key]
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int Pieza
        {
            get => _pieza;
            set
            {
                if (_pieza != value)
                {
                    _pieza = value;
                    OnPropertyChanged();
                }
            }
        }
        public int MinPiezas
        {
            get => _minPiezas;
            set
            {
                if (_minPiezas != value)
                {
                    _minPiezas = value;
                    OnPropertyChanged();
                }
            }
        }
        public int MaxPiezas
        {
            get => _maxPiezas;
            set
            {
                if (_maxPiezas != value)
                {
                    _maxPiezas = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}