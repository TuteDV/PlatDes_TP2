using System;
namespace TP_1_BANCO
{
    public class UserTarje
    {
        public int idUser { get; set; }
        public int idTarje { get; set; }

        public UserTarje(int idUser, int idTarje)
        {
            this.idUser = idUser;
            this.idTarje = idTarje;
        }
    }
}

