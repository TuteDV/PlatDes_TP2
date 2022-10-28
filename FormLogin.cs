using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace TP_1_BANCO
{
    public partial class FormLogin : Form
    {
        public bool logued;
        public string usuario;
        public string pass;
        int dni;
        string nombre;
        string apellido;
        string mail;
        public Banco miBanco;
        public bool admin;

        public TransfDelegadoLogin TransfEventoLogin;
        public TransfDelegadoAlta TransfEventoAlta;
        
        public FormLogin(Banco b)
        {
            InitializeComponent();
            miBanco = b;           
        }
        // USUARIO ALTA LIMPIA EL FORMULARIO 
        internal void deleteSelectionsAlta()
        {
            dni_alta.Text = "";
            pass_alta.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }
        // USUARIO LOGIN 
        private void submit_login_Click(object sender, EventArgs e)
        {
            dni = int.Parse(dni_login.Text);
            pass = pass_login.Text;
                this.TransfEventoLogin(dni, pass);
        }
        // USUARIO ALTA SOLO DEJA ESCRIBIR NUMEROS EN CAMPO DNI
        private void dni_alta_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(dni_alta.Text, "[^0-9]"))
            {
                dni_alta.Text = dni_alta.Text.Remove(dni_alta.Text.Length - 1);
            }
        }
        // ALTA USUARIO
        private void submit_alta_Click(object sender, EventArgs e)
        {
            dni = int.Parse(dni_alta.Text);
            pass = pass_alta.Text;
            nombre = textBox4.Text;
            apellido = textBox2.Text;
            mail = textBox1.Text;
            if (checkBox1.Checked == false)
            {
                admin = false;
            }
            else
            {
                admin = true;
            }
            /*this.TransfEventoAlta(dni, nombre, apellido, mail, pass);*/
            this.TransfEventoAlta(dni, nombre, apellido, mail, pass, admin);
        }
        // LOGIN USUARIO SOLO DEJA ESCRIBIR NUMEROS
        private void dni_login_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(dni_login.Text, "[^0-9]"))
            {
                dni_login.Text = dni_login.Text.Remove(dni_login.Text.Length - 1);
            }
        }
    }
    public delegate void TransfDelegadoLogin(int dni, string pass);
    public delegate void TransfDelegadoAlta(int dni, string nombre, string apellido, string mail, string pass, bool admin);

}