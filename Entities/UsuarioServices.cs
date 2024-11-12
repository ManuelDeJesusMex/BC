    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Hotel.Entities
{
    public class UsuarioServices
    {
        public Response LoginWithPredefinedPassword()
        {
            // Simulación de una respuesta de login
            return new Response
            {
                Papel = new Papel
                {
                    Nombre = "Super Administrador" // O "Administrador" según lo necesites para las pruebas
                }
            };
        }
    }

    public class Response
    {
        public Papel Papel { get; set; }
    }

    public class Papel
    {
        public string Nombre { get; set; }
    }
}
