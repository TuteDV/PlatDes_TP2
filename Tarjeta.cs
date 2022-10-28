namespace TP_1_BANCO
{
    public class Tarjeta
    {
        public int id { get; }
        public Usuario? titular { get; set; }
        public int idUsuario { get; set; }
        public int numero { get; }
        public int codigoV { get; }
        public float limite { get; set; }
        public float consumos { get; set; }

        public Tarjeta (int id, Usuario user, int numero, int codigoV, float limite) {
            this.id = id;
            this.titular = user;
            this.idUsuario = user.id;
            this.numero = numero;
            this.codigoV = codigoV;
            this.limite = limite;
            consumos = 0.0F;
        }
        public Tarjeta(int id, int idUsuario, int numero, int codigoV, float limite, float consumos)
        {
            this.id = id;
            this.idUsuario = idUsuario;
            this.numero = numero;
            this.codigoV = codigoV;
            this.limite = limite;
            this.consumos = consumos;
        }

    }
}