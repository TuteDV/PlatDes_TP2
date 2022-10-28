using System;
namespace TP_1_BANCO
{
    public class UserCaja
    {
        public int idUser { get; set; }
        public int idCaja { get; set; }
        public UserCaja(int idUser, int idCaja)
        {
            this.idUser = idUser;
            this.idCaja = idCaja;
        }
    }
}

