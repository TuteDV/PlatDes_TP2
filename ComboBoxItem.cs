using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1_BANCO
{
    public enum tipoProducto { TC, CA , PF , USUARIO};
    internal class ComboBoxItem
    {
        string displayValue;
        int hiddenValue;
        tipoProducto productoTipo;
        // Constructor
        public ComboBoxItem(string d, int h , tipoProducto pt)
        {
            this.displayValue = d;
            this.hiddenValue = h;
            this.productoTipo = pt;
        }

        // Accessor
        public int HiddenValue
        {
            get
            {
                return hiddenValue;
            }
        }
        public string TipoProd
        {
            get
            {
                return productoTipo.ToString();
            }
        }
        // Override ToString method
        public override string ToString()
        {
            return displayValue;
        }


    }
}
