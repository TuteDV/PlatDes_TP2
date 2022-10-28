using System;
namespace TP_1_BANCO
{
    public class CajaTitu
    {
        public int idCaja { get; set; }
        public int idUser { get; set; }

        public CajaTitu(int idCaja, int idUser)
        {
            this.idCaja = idCaja;
            this.idUser = idUser;
        }
    }
}

