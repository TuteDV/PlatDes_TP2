using System;
namespace TP_1_BANCO
{
    public class UserPago
    {
        public int idUser { get; set; }
        public int idPago { get; set; }
        public UserPago(int idUser, int idPago)
        {
            this.idUser = idUser;
            this.idPago = idPago;
        }
    }
}

