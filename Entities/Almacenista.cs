using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Hotel.Entities
{
    internal class Almacenista
    {
        [Key]

        public int PkAlmacenista {  get; set; }

        public string Nombre {  get; set; }

        public string Apellidos { get; set; }

        public string Correo { get; set; }

        public string contrasena { get; set; }




    }
}
