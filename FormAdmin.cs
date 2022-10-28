using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TP_1_BANCO.FormMain;

namespace TP_1_BANCO
{
    public partial class FormAdmin : Form
    {
        string pass;
        int dni;
        string nombre;
        string apellido;
        string mail;

        private int selectedUsID;
        public Banco banco;
        string tituloMsgBox;
        public bool adminFlag;
        public TransfDelegadoCerrarSesion TransfEventoCerrarSesion;

        public FormAdmin(Banco banco3)
        {
            this.banco = banco3;
            tituloMsgBox = "Banco Da Vinci";
            InitializeComponent();
            label1.Text = "Bienvenido " + banco.MostrarUsuarioActualNombre();
            refreshVistaUsuarios();
        }

        private void refreshVistaUsuarios()
        {
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            // ACTUALIZO GRILLA DE USUARIOS DISPONIBLES DEL BANCO
            dataGridView1.Rows.Clear();
            foreach (List<string> u in banco.MostrarUsuariosBanco())
                dataGridView1.Rows.Add(u.ToArray());
            //  MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedUsID > 0)
            {
                label9.Text = "Usuario Seleccionado ID:  " + selectedUsID;
            }
            else
            {
                label9.Text = "Seleccione un Usuario";
            }
        }

        private void submit_alta_Click(object sender, EventArgs e)
        {
            //dni = int.Parse(dni_alta.Text);
            //pass = pass_alta.Text;
            //nombre = textBox4.Text;
            //apellido = textBox2.Text;
            //mail = textBox1.Text;
            //if (checkBox1.Checked == false)
            //{
            //    admin = false;
            //}
            //else
            //{
            //    admin = true;
            //}
            //this.TransfEventoModificacion(dni, nombre, apellido, mail, pass, admin);
        }
    }
}
