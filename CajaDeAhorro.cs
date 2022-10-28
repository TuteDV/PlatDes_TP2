namespace TP_1_BANCO
{
    public class CajaDeAhorro
    {
        public int id { get; }
        public int cbu { get; }
        public float saldo { get; set; }
        public List<Usuario> titulares { get; set; }
        public List<Movimiento> movimientos { get; set; }
    
        public CajaDeAhorro(int id, int cbu, float saldo)
        {
            this.id = id;
            this.cbu = cbu;
            this.saldo = saldo;
            titulares = new List<Usuario>();
            movimientos = new List<Movimiento>();
        }

    }
}