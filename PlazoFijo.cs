using System;

namespace TP_1_BANCO
{
    public class PlazoFijo
    {
        public int id { get; }
        public Usuario? titular { get; set; }
        public int idUsuario { get; set; }
        public float monto { get; }
        public DateTime fechaIni { get; }
        public DateTime fechaFin { get; }
        public float tasa { get;  }
        public bool pagado { get; set; }
        public int cbuAlta { get; }

        public PlazoFijo (int id, Usuario titular,float monto, DateTime fechaIni, DateTime fechaFin, float tasa, int cbuAlta) {
            this.id = id;
            this.titular = titular;
            this.idUsuario = titular.id;
            this.monto = monto;
            this.fechaIni = fechaIni;
            this.fechaFin = fechaFin;
            this.tasa = tasa;
            this.pagado = false;
            this.cbuAlta = cbuAlta;
        }

        public PlazoFijo(int id, int idUsuario, float monto, DateTime fechaIni, DateTime fechaFin, float tasa, Boolean pagado,int cbuAlta)
        {
            this.id = id;
            this.idUsuario = idUsuario;
            this.monto = monto;
            this.fechaIni = fechaIni;
            this.fechaFin = fechaFin;
            this.tasa = tasa;
            this.pagado = pagado;
            this.cbuAlta = cbuAlta;
        }

    }
}