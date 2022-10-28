namespace TP_1_BANCO
{
    public class Pago
    {
        public int id { get; }
        public Usuario user { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; }
        public float monto { get; }
        public bool pagado { get; set; }
        public string metodo { get; }

        public Pago (int id, Usuario user, string nombre, float monto,string metodo)
        {
            this.id = id;
            this.user = user;
            this.idUsuario = user.id; 
            this.nombre = nombre;
            this.monto = monto;
            this.pagado = false;
            this.metodo = metodo;
        }

        public Pago (int id, int idUsuario, string nombre, float monto, bool pagado, string metodo)
        {
            this.id = id;
            this.idUsuario=idUsuario;
            this.nombre = nombre;
            this.monto = monto;
            this.pagado = false;
            this.metodo = metodo;
        }


    }
}