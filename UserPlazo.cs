using System;
namespace TP_1_BANCO
{
    public class UserPlazo
    {
        public int idUser { get; set; }
        public int idPlazo { get; set; }
        public UserPlazo(int idUser, int idPlazo)
        {
            this.idUser = idUser;
            this.idPlazo = idPlazo;
        }
    }
}

