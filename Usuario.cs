using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TP_1_BANCO
{
    public class Usuario
    {
        public int id { get; }
        public int dni { get; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public int intentosFallidos { get; set; }
        public bool bloqueado { get; set; }
        public bool admin { get; set; }
        public List<CajaDeAhorro> cajas { get; set; }
        public List<PlazoFijo> plazosFijos { get; set; }
        public List<Tarjeta> tarjetas { get; set; }
        public List<Pago> pagos { get; set; }

        public Usuario(int id, int dni, string nombre, string apellido, string mail, string pass, int intentosFallidos, bool bloqueado, bool admin)
        {
            this.id = id;
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.mail = mail;
            this.password = pass;
            this.admin = admin;
            cajas = new List<CajaDeAhorro>();
            plazosFijos = new List<PlazoFijo>();
            tarjetas = new List<Tarjeta>();
            pagos = new List<Pago>();
            this.bloqueado = bloqueado;
            this.intentosFallidos = intentosFallidos;

        }

        public string[] toArray()
        {
            return new string[] { nombre, password };
        }

        public void agregarCaja(CajaDeAhorro ca)
        {
            cajas.Add(ca);
        }

        public void quitarCaja(CajaDeAhorro ca)
        {
            cajas.Remove(ca);
        }

        public void agregarPlazoFijo(PlazoFijo pf)
        {
            plazosFijos.Add(pf);
        }
        public void quitarCaja(PlazoFijo pf)
        {
            plazosFijos.Remove(pf);
        }
        public void agregarPago(Pago pa)
        {
            pagos.Add(pa);
        }
        public void quitarPago(Pago pa)
        {
            pagos.Remove(pa);
        }
        public void agregarTarjeta(Tarjeta ta)
        {
            tarjetas.Add(ta);
        }
        public void quitarTarjeta(Tarjeta ta)
        {
           tarjetas.Remove(ta);
        }
    }
}
