using System;

namespace TP_1_BANCO
{
    public class Movimiento
    {
        public int id { get; }
        public CajaDeAhorro? caja { get; set; }
        public int idCaja { get; set; }
        public string detalle { get; }
        public float monto { get; }
        public DateTime fecha { get; }

        public Movimiento (int id, CajaDeAhorro caja, string detalle, float monto) {
            this.id = id;
            this.caja = caja;
            this.idCaja = caja.id;
            this.detalle = detalle;
            this.monto = monto;
            fecha = DateTime.Now;

        }
        public Movimiento(int id, int idCaja, string detalle, float monto,DateTime fecha)
        {
            this.id = id;
            this.idCaja = idCaja;
            this.detalle = detalle;
            this.monto = monto;
            this.fecha = fecha;

        }

    }
}