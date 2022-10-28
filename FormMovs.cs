using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static TP_1_BANCO.FormMain;

namespace TP_1_BANCO
{
    public partial class FormMovs : Form
    {
        String detalleB;
        float? montoB;
        DateTime? fechaB;
        int idCaja;
        string usuario;
        Banco banco;
        public TransfDelegadoVolverAMain TransfEventoVolverAMain;
        public FormMovs(Banco banco, int idCaja)
        {
            InitializeComponent();
            this.banco = banco; 
            this.usuario = this.banco.MostrarUsuarioActualNombre();
            this.idCaja = idCaja;   
            label2.Text = "Cliente:  " + this.usuario;
            label4.Text = "Caja de Ahorro ID: " + this.idCaja;
            refreshVistaMovs(this.idCaja);
        }
        //REFRESH LISTA DE MOVIMIENTOS
        private void refreshVistaMovs(int idCaja)
        {
            textBox6.Enabled = false;
            dtDesdeMovsCA.Enabled = false;
            textBox8.Enabled = false;
            dataGridView1.Rows.Clear();
            foreach (List<string> movs in banco.MostrarMovimientos(idCaja))
                dataGridView1.Rows.Add(movs.ToArray());
        }
        // MUESTRA LOS MOVIMIENTOS (SIN FILTRO)
        private void button2_Click(object sender, EventArgs e)
        {
            refreshVistaMovs(this.idCaja);
        }
        // MUESTRA LOS MOVIMIENTOS AJUSTADO A LOS FILTROS
        private void refreshVistaCAConFiltro(int idCaja, string detalleB, DateTime? fechaB, float? montoB)
        {
            dataGridView1.Rows.Clear();
            foreach (List<string> movs in banco.BuscarMovimiento(idCaja, detalleB, fechaB, montoB))
                dataGridView1.Rows.Add(movs.ToArray());
        }
        // BLOQUEA EL INPUT SI NO SE VA A FILTRAR POR ESTE VALOR
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox6.Enabled = true;
            }
            else {
                textBox6.Enabled = false;
            }

        }
        // BLOQUEA EL INPUT SI NO SE VA A FILTRAR POR ESTE VALOR
        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                dtDesdeMovsCA.Enabled = true;
            }
            else
            {
                dtDesdeMovsCA.Enabled = false;
            }

        }
        // BLOQUEA EL INPUT SI NO SE VA A FILTRAR POR ESTE VALOR
        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox8.Enabled = true;
            }
            else
            {
                textBox8.Enabled = false;
            }

        }
        // BUSCA LOS MOVIMIENTOS POR LOS FILTROS SELECCIONADOS MEDIANTE LOS CHECKBOX
        // SI LOS CHECKBOX ESTAN CLICKEADOS [TRUE] EL FILTRO APLICA
        // SI LOS CHECKBOX NO ESTAN CLICKEADOS [FALSE] EL FILTRO NO APLICA Y SE GENERA VALOR NULL O VACIO PARA EL INPUT
        private void button18_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == false) {
                detalleB = "";
            } else {
                detalleB = textBox6.Text;
            }

            if (checkBox2.Checked == false)
            {
                fechaB = null;
            }
            else
            {
                fechaB = dtDesdeMovsCA.Value;
            }
            if (checkBox3.Checked == false)
            {
                montoB = null;
            }
            else
            {
                montoB = float.Parse(textBox8.Text);
            }
            refreshVistaCAConFiltro(idCaja, detalleB, fechaB, montoB);
        }
        // VUELVE AL FORM MAIN DE PRODUCTOS
        private void button1_Click(object sender, EventArgs e)
        {
            this.TransfEventoVolverAMain(banco);
        }
        public delegate void TransfDelegadoVolverAMain(Banco banco);

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
