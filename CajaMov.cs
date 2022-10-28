using System;
namespace TP_1_BANCO
{
        public class CajaMov
        {
            public int idCaja { get; set; }
            public int idMov { get; set; }
           
            public CajaMov (int idCaja, int idMov)
            {
                this.idCaja = idCaja;
                this.idMov = idMov; 
            }
        }
}

