using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace TP_1_BANCO
{
    public partial class FormMain : Form

    {
        public Banco banco;
        string tituloMsgBox;
        public bool adminFlag;
        public TransfDelegadoCerrarSesion TransfEventoCerrarSesion;
        public TransfDelegadoModificarUsuario TransfEventoModificarUsuario;
        public TransfDelegadoDesbloqueoUsuario TransfEventoDesbloquearUsuario;
        public TransfDelegadoSetAdmin TransfEventoSetAdmin;

        public TransfDelegadoAltaCA TransfEventoAltaCA;
        public TransfDelegadoBajaCA TransfEventoBajaCA;
        public TransfDelegadoDepositarCA TransfEventoDepositarCA;
        public TransfDelegadoRetirarCA TransfEventoRetirarCA;
        public TransfDelegadoTransferirCA TransfEventoTransferirCA;
        public TransfDelegadoAltaCoTtitularesCA TransfEventoAltaCoTtitularesCA;
        public TransfDelegadoBajaCoTtitularesCA TransfEventoBajaCoTtitularesCA;
        public TransfDelegadoVerMovimientosCA TransfEventoVerMovimientosCA;

        public TransfDelegadoAltaTC TransfEventoAltaTC;
        public TransfDelegadoBajaTC TransfEventoBajaTC;
        public TransfDelegadoModificaLimiteTC TransfEventoModificaLimiteTC;
        public TransfDelegadoPagaTC TransfEventoPagaTC;

        public TransfDelegadoAltaPago TransfEventoAltaPago;
        public TransfDelegadoPagaPago TransfEventoPagaPago;
        public TransfDelegadoEliminaPago TransfEventoEliminaPago;
        public TransfDelegadoAltaPF TransfEventoAltaPF;
        public TransfDelegadoBajaPF TransfEventoBajaPF;

        private int selectedCAID;
        private int selectedCADestinoID;
        private int selectedTCID;
        private int selectedUserID;
        private int selectedPagoID;
        private int selectedPFID;
        public FormMain(Banco banco3, bool adminFlag)
        {
            this.banco = banco3;
            this.adminFlag = adminFlag;
            tituloMsgBox = "Banco Da Vinci";
            InitializeComponent();
            label2.Text = "Bienvenido " + banco.MostrarUsuarioActualNombre();
            ejectuarVistaIncial();
        }
        private void ejectuarVistaIncial()
        {
            checkBox1.Enabled = false;
            //BLOQUEO LA VISTA ADMIN
            dni_alta.Enabled = false;
            groupBox14.Visible = false;
            groupBox14.Enabled = false;
            label34.Text = "UD NO TIENE LOS PERMISOS PARA ESTA VISTA";
            if (adminFlag == true)
            {
                groupBox14.Visible = true;
                groupBox14.Enabled = true;
                label34.Text = "";
            }
            // muestra el limite de la TC inicial
            textBox7.Text = banco.LimiteInicialTC.ToString();
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            button16.Enabled = false; // desbloquear
            button10.Enabled = false; // ADMIN
            button2.Enabled = false;
            btn_ver_movs.Enabled = false;
            groupBox10.Enabled = false;
            groupBox2.Enabled = false;
            groupBox1.Enabled = false;
            groupBox12.Enabled = false;
            // TASA DE PLAZO FIJO
            label24.Text = "Tasa ofrecida: " + banco.tasaPF.ToString();
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO DEL TAB 1
            refreshVistaCA();
            // ACTUALIZO LA LISTA DE USUARIOS DE BANCO PARA AGREGAR DE TITULAR
            actualizaListaUsuariosBancoAgregarTitulares();
        }
        // USUARIO CERRAR SESION
        private void button4_Click(object sender, EventArgs e)
        {
            this.TransfEventoCerrarSesion();
        }
        // USUARIO SET ADMIN
        private void button10_Click(object sender, EventArgs e)
        {
            this.TransfEventoSetAdmin(selectedUserID);
            refreshVistaUsuarios();
        }
        //USUARIO DESBLOQUEO
        private void button16_Click(object sender, EventArgs e)
        {
            this.TransfEventoDesbloquearUsuario(selectedUserID);
            refreshVistaUsuarios();
        }
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                button11.Enabled = false;
                button8.Enabled = false;
            }
            else
            {
                button11.Enabled = true;
                button8.Enabled = true;

            }
        }
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length == 0 || comboBox7.SelectedIndex == -1)
            {
                button18.Enabled = false;
            }
            else
            {
                button18.Enabled = true;
            }
        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == -1)
            {
                button20.Enabled = false;
            }
            else
            {
                button20.Enabled = true;
            }

        }
        // 

        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == -1)
            {
                button13.Enabled = false;
            }
            else
            {
                button13.Enabled = true;
            }
        }

        // METODOS DE USO INTERNO DEL FORM 
        // ACTUALIZA LISTADO DE USUARIOS DEL COMBO BOX PARA AGREGAR USUARIOS DE LA CAJA DE AHORRO
        private void actualizaListaUsuariosBancoAgregarTitulares()
        {
            button13.Enabled = false;
            comboBox5.Items.Clear();
            comboBox5.ResetText();
            comboBox8.Items.Clear();
            comboBox8.ResetText();
            foreach (Usuario user in banco.MostrarUsuarios())
            {
                string displayValue = user.nombre + " | (" + user.dni + ")";
                int hiddenValue = user.id;
                //comboBox5.Items.Add(user.nombre + " | (" + user.dni + ")");
                comboBox5.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
                comboBox8.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
            }
        }
        // ACTUALIZA LISTADO DE USUARIOS DEL COMBO BOX PARA ELIMINAR USUARIOS DE LA CAJA DE AHORRO
        private void actualizaListaUsuariosCoTitularesDeCuenta(int idCaja)
        {
            button20.Enabled = false;
            comboBox6.Items.Clear();
            comboBox6.ResetText();
            foreach (Usuario user in banco.MostrarUsuariosPorCajaDeAhorro(idCaja))
            {
                string displayValue = user.nombre + " | (" + user.dni + ")";
                int hiddenValue = user.id;
                comboBox6.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
            }
        }
        // ACTUALIZA LOS DATAGRIEDVIEW AL SELECCIONAR CADA TAB DE PRODUCTOS
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
            {
                refreshVistaCA();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])
            {
                refreshVistaTC();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])
            {
                refreshVistaPlazosFijos();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage4"])
            {
                refreshVistaPagoPendientes();
                refreshVistaPagoRealizados();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage6"])
            {
                refreshVistaUsuarios();
            }
        }
        // CAJA DE AHORRO - dataGridView2
        //REFRESH LISTA DE CAJAS DE AHORRO DEL USUARIO E INICIALIZA LOS BOTONES Y CELDAS
        private void refreshVistaCA()
        {
            // SETEA OPCIONES X SI NO ES ADMIN Y SI ES ADMIN LO EJECUTA
            comboBox8.Enabled = false;
            label25.Visible = false;
            button1.Enabled = true;
            if (adminFlag == true)
            {
                comboBox8.Enabled = true;
                label25.Visible = true;
                button1.Enabled = false;
            }
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            groupBox10.Enabled = false;
            groupBox2.Enabled = false;
            button18.Enabled = false;
            btn_ver_movs.Enabled = false;
            textBox2.Text = "";
            textBox6.Text = "";
            comboBox7.Items.Clear();
            comboBox7.ResetText();
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO
            dataGridView2.Rows.Clear();
            foreach (List<string> ca in banco.MostrarCajasDeAhorro())
                dataGridView2.Rows.Add(ca.ToArray());
            // ACTUALIZO LA LISTA DE USUARIOS PARA ALTA DE CA
            comboBox8.Items.Clear();
            comboBox8.ResetText();
            foreach (Usuario user in banco.MostrarUsuarios())
            {
                string displayValue = user.nombre + " | (" + user.dni + ")";
                int hiddenValue = user.id;
                comboBox8.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
            }
            //  MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedCAID > 0)
            {
                label13.Text = "Caja Seleccionada \n ID:  " + selectedCAID;
                label14.Text = "Caja Seleccionada \n ID:  " + selectedCAID;
            }
            else
            {
                label13.Text = "Seleccione una Caja";
                label14.Text = "Seleccione una Caja";
            }
        }
        // CAJA DE AHORRO CAPTURA EL CLICK EN EL DATAGRIDVIEW PARA TOMAR EL ID
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // ASIGNO LA CAJA ID SELECCIONADA
                selectedCAID = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());

                //  ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL BANCO PARA TRANSFERIR 
                comboBox7.Items.Clear();
                comboBox7.ResetText();
                foreach (CajaDeAhorro ca in banco.ObtenerCajasDeAhorroBanco())
                {
                    if (ca.id != selectedCAID)
                    {
                        string displayValue = "CBU: " + ca.cbu.ToString() + " | Saldo: (" + ca.saldo + ")";
                        int hiddenValue = ca.id;
                        comboBox7.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
                    }
                }
                //Habilito los botones
                button2.Enabled = true;
                btn_ver_movs.Enabled = true;
                groupBox10.Enabled = true;
                groupBox2.Enabled = true;
                groupBox1.Enabled = true;
                groupBox12.Enabled = true;
                //habilito el id
                label13.Text = "Caja Seleccionada \n ID:  " + selectedCAID;
                label14.Text = "Caja Seleccionada \n ID:  " + selectedCAID;

                actualizaListaUsuariosCoTitularesDeCuenta(selectedCAID);
            }

        }
        // CAJA DE AHORRO ALTA
        private void button1_Click(object sender, EventArgs e)
        {
            // SI ES ADMIN Y SELECCIONA ALGUN CLIENTE LO ENVIA SINO -1
            selectedUserID = -1;
            if (adminFlag == true)
            {
                selectedUserID = ((ComboBoxItem)comboBox8.SelectedItem).HiddenValue;

            }
            this.TransfEventoAltaCA(selectedUserID);
            refreshVistaCA();
        }
        // CAJA DE AHORRO BAJA
        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("IDselectedCA " + IDselectedCA);
            this.TransfEventoBajaCA(selectedCAID);
            refreshVistaCA();
        }
        // CAJA DE AHORRO DEPOSITO
        private void button8_Click(object sender, EventArgs e)
        {
            float montoD = float.Parse(textBox2.Text);
            this.TransfEventoDepositarCA(selectedCAID, montoD);
            refreshVistaCA();
        }
        // CAJA DE AHORRO RETIRO
        private void button11_Click(object sender, EventArgs e)
        {
            float montoR = float.Parse(textBox2.Text);
            this.TransfEventoRetirarCA(selectedCAID, montoR);
            refreshVistaCA();

        }
        // CAJA DE AHORRO TRANSFERENCIA. LUEGO DE TRANSFERIR (O NO) ACTUALIZA LOS SALDOS Y COMBOBOX
        private void button18_Click(object sender, EventArgs e)
        {
            float montoR = float.Parse(textBox6.Text);
            selectedCADestinoID = ((ComboBoxItem)comboBox7.SelectedItem).HiddenValue;
            this.TransfEventoTransferirCA(selectedCAID, selectedCADestinoID, montoR);
            refreshVistaCA();
        }
        // CAJA DE AHORRO AGREGA TITULARES
        private void button13_Click(object sender, EventArgs e)
        {
            selectedUserID = ((ComboBoxItem)comboBox5.SelectedItem).HiddenValue;
            this.TransfEventoAltaCoTtitularesCA(selectedCAID, selectedUserID);
            actualizaListaUsuariosCoTitularesDeCuenta(selectedCAID);
            actualizaListaUsuariosBancoAgregarTitulares();
        }
        // CAJA DE AHORRO BAJA TITULARES
        private void button20_Click(object sender, EventArgs e)
        {
            selectedUserID = ((ComboBoxItem)comboBox6.SelectedItem).HiddenValue;
            this.TransfEventoBajaCoTtitularesCA(selectedCAID, selectedUserID);
            actualizaListaUsuariosCoTitularesDeCuenta(selectedCAID);
            actualizaListaUsuariosBancoAgregarTitulares();
        }
        // CAJA DE AHORRO MUESTRA TITULARES DE LA CAJA SELECCIONADA
        private void button3_Click(object sender, EventArgs e)
        {
            string displayValue = "";
            foreach (Usuario user in banco.MostrarUsuariosPorCajaDeAhorro(selectedCAID))
            {
                displayValue = displayValue + user.apellido + ", " + user.nombre + " | (DNI: " + user.dni + ") \n";
            }
            MessageBox.Show(displayValue, tituloMsgBox + "Titulares de la CA ID: " + selectedCAID, MessageBoxButtons.OK);

        }
        // CAJA DE AHORRO MOVIMIENTOS (VA HACIA FormMovs)
        private void btn_ver_movs_Click(object sender, EventArgs e)
        {
            this.TransfEventoVerMovimientosCA(selectedCAID);
        }
        private void btn_ver_movs_Click_1(object sender, EventArgs e)
        {
            this.TransfEventoVerMovimientosCA(selectedCAID);
        }

        // TARJETA DE CREDITO - dataGridView3
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length == 0 || comboBox7.SelectedIndex == -1)
            {
                button18.Enabled = false;
            }
            else
            {
                button18.Enabled = true;
            }
        }
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }
        }
        // TC VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                button7.Enabled = false;
            }
            else
            {
                button7.Enabled = true;

            }

        }
        // TC - VALIDACION DE SELECCION DE CLIENTE EN ALTA TC SIENDO ADMIN
        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            button15.Enabled = false;
            if (adminFlag == true && comboBox9.SelectedIndex == -1)
            {
                button15.Enabled = false;
            }
            else
            {
                button15.Enabled = true;
            }
        }

        // REFRESH LISTA DE TARJETAS DEL USUARIO E INICIALIZA LOS BOTONES Y CELDAS
        private void refreshVistaTC()
        {
            // SETEA OPCIONES X SI NO ES ADMIN Y SI ES ADMIN LO EJECUTA
            comboBox9.Enabled = false;
            label26.Visible = false;
            button15.Enabled = true;
            if (adminFlag == true)
            {
                comboBox9.Enabled = true;
                label26.Visible = true;
                button15.Enabled = false;
            }
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            button6.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            textBox1.Text = "";
            button5.Enabled = false;
            button7.Enabled = false;
            button12.Enabled = false;
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO
            dataGridView3.Rows.Clear();
            foreach (List<string> ca in banco.MostrarTarjetas())
                dataGridView3.Rows.Add(ca.ToArray());
            // ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL USUARIO PARA PAGAR
            comboBox2.Items.Clear();
            comboBox2.ResetText();
            // ACTUALIZO LA LISTA DE USUARIOS PARA ALTA DE TARJETAS
            comboBox9.Items.Clear();
            comboBox9.ResetText();
            foreach (Usuario user in banco.MostrarUsuarios())
            {
                string displayValue = user.nombre + " | (" + user.dni + ")";
                int hiddenValue = user.id;
                //comboBox5.Items.Add(user.nombre + " | (" + user.dni + ")");
                comboBox9.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
            }
            // MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedTCID > 0)
            {
                label18.Text = "TC Seleccionada \n ID:  " + selectedTCID;
            }
            else
            {
                label18.Text = "Seleccione una Tarjeta para operar";
            }
        }
        // TARJETA DE CREIDOT CAPTURA EL CLICK EN EL DATAGRIDVIEW PARA TOMAR EL ID
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    // asigno la TC seleccionada
                    selectedTCID = int.Parse(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                    // ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL USUARIO de la tc PARA PAGAR
                    comboBox2.Items.Clear();
                    comboBox2.ResetText();
                    foreach (CajaDeAhorro ca in banco.ObtenerCajasDeAhorroByTarjetaId(selectedTCID))
                    {
                        string displayValue = "CBU: " + ca.cbu.ToString() + " | Saldo: $ " + ca.saldo;
                        int hiddenValue = ca.id;
                        comboBox2.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
                    }
                    //Habilito los botones
                    button6.Enabled = true;
                    groupBox5.Enabled = true;
                    groupBox6.Enabled = true;
                    //habilito el id
                    label18.Text = "TC Seleccionada \n ID:  " + selectedTCID;
                    // ENVIO EL LIMITE DE LA TARJETA SELECCIONADA A LA CELDA DE LIMITE A MODIFICAR
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
                }

            }
        }
        // TARJETA DE CREDITO ALTA
        private void button15_Click(object sender, EventArgs e)
        {
            // SI ES ADMIN Y SELECCIONA ALGUN CLIENTE LO ENVIA SINO -1
            selectedUserID = -1;
            if (adminFlag == true)
            {
                selectedUserID = ((ComboBoxItem)comboBox9.SelectedItem).HiddenValue;

            }
            float limite = banco.LimiteInicialTC;
            this.TransfEventoAltaTC(limite, selectedUserID);
            refreshVistaTC();
        }
        // TARJETA DE CREDITO BAJA
        private void button6_Click(object sender, EventArgs e)
        {
            this.TransfEventoBajaTC(selectedTCID);
            refreshVistaTC();
        }
        // TARJETA DE CREDITO MODIFICA LIMITE
        private void button5_Click(object sender, EventArgs e)
        {
            float limiteN = float.Parse(textBox1.Text);
            this.TransfEventoModificaLimiteTC(selectedTCID, limiteN);
            refreshVistaTC();

        }
        // TARJETA DE CREDITO PAGA CONSUMOS
        private void button7_Click(object sender, EventArgs e)
        {
            selectedCAID = ((ComboBoxItem)comboBox2.SelectedItem).HiddenValue;
            this.TransfEventoPagaTC(selectedTCID, selectedCAID);
            refreshVistaTC();
        }
        // PAGO PENDIENTE - dataGridView5
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0 || textBox4.Text.Length == 0)
            {
                button17.Enabled = false;
            }
            else
            {
                button17.Enabled = true;
            }
        }
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0 || textBox4.Text.Length == 0)
            {
                button17.Enabled = false;
            }
            else
            {
                button17.Enabled = true;
            }
        }
        // VALIDACION DE DATOS COMPLETOS PARA HABILITACION DE BOTONES (PREVIENE ERRORES DE EJECUCION)
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == -1)
            {
                button9.Enabled = false;
            }
            else
            {
                button9.Enabled = true;
            }
        }
        //REFRESH LISTA DE CAJAS DE AHORRO DEL USUARIO E INICIALIZA LOS BOTONES Y CELDAS
        private void refreshVistaPagoPendientes()
        {
            // SETEA OPCIONES X SI NO ES ADMIN Y SI ES ADMIN LO EJECUTA
            comboBox10.Enabled = false;
            button17.Enabled = true;
            label27.Visible = false;
            if (adminFlag == true)
            {
                comboBox10.Enabled = true;
                button17.Enabled = false;
                label27.Visible = true;
                // ACTUALIZO LA LISTA DE USUARIOS PARA ALTA DE PAGOS PENDIENTES
                comboBox10.Items.Clear();
                comboBox10.ResetText();
                foreach (Usuario user in banco.MostrarUsuarios())
                {
                    string displayValue = user.nombre + " | (" + user.dni + ")";
                    int hiddenValue = user.id;
                    comboBox10.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
                }
            }
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            textBox4.Text = "";
            textBox5.Text = "";
            groupBox8.Enabled = false;
            button9.Enabled = false;
            button17.Enabled = false;
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO
            dataGridView5.Rows.Clear();
            foreach (List<string> pap in banco.MostrarPagosPendientes())
                dataGridView5.Rows.Add(pap.ToArray());
            // ACTUALIZO LA LISTA DE CAJAS DE AHORRO Y TC DEL USUARIO PARA PAGAR
            comboBox4.Items.Clear();
            comboBox4.ResetText();
            comboBox4.Enabled = false;

            // MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedPagoID != 0)
            {
                label19.Text = "Pago Seleccionado \n ID:  " + selectedPagoID;
            }
            else
            {
                label19.Text = "Seleccione un Pago";
            }
        }
        // PAGOS PENDIENTES CAPTURA EL CLICK EN EL DATAGRIDVIEW PARA TOMAR EL ID
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedPagoID = int.Parse(dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString());
                // ACTUALIZO LA LISTA DE CAJAS DE AHORRO Y TC DEL USUARIO PARA PAGAR
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.ResetText();
                foreach (CajaDeAhorro ca in banco.ObtenerCajasDeAhorroByPagoId(selectedPagoID))
                {
                    string displayValue = "CBU: " + ca.cbu.ToString() + " | Saldo: (" + ca.saldo + ")";
                    int hiddenValue = ca.id;
                    comboBox4.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
                }
                foreach (Tarjeta tc in banco.ObtenerTarjetaByPagoId(selectedPagoID))
                {
                    string displayValue = "TC ID: " + tc.id.ToString() + " | Disponible: (" + (tc.limite - tc.consumos) + ")";
                    int hiddenValue = tc.id;
                    comboBox4.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.TC));
                }

                //Habilito los botones
                groupBox8.Enabled = true;
                //habilito el id
                label19.Text = "Pago Seleccionada \n ID:  " + selectedPagoID;
                ;
            }
        }
        // PAGO PENDIENTE ALTA
        private void button17_Click(object sender, EventArgs e)
        {
            // SI ES ADMIN Y SELECCIONA ALGUN CLIENTE LO ENVIA SINO -1
            selectedUserID = -1;
            if (adminFlag == true)
            {
                selectedUserID = ((ComboBoxItem)comboBox10.SelectedItem).HiddenValue;

            }
            float monto = float.Parse(textBox4.Text);
            string nombre = textBox5.Text;
            string metodo = textBox5.Text;
            this.TransfEventoAltaPago(nombre, monto, metodo, selectedUserID);
            refreshVistaPagoPendientes();
        }
        // PAGO PENDIENTE PAGO
        private void button9_Click_1(object sender, EventArgs e)
        {
            int? selectedTCID = null;
            int? selectedCAID = null;

            if (((ComboBoxItem)comboBox4.SelectedItem).TipoProd == "TC")
            {
                selectedTCID = ((ComboBoxItem)comboBox4.SelectedItem).HiddenValue;

            }
            else if (((ComboBoxItem)comboBox4.SelectedItem).TipoProd == "CA")
            {
                selectedCAID = ((ComboBoxItem)comboBox4.SelectedItem).HiddenValue; ;

            }
            //MessageBox.Show("IdPAgo" + selectedPagoID + " TC id " + selectedTCID + " CA id " + selectedCAID);
            this.TransfEventoPagaPago(selectedPagoID, selectedTCID, selectedCAID);
            refreshVistaPagoPendientes();
            refreshVistaPagoRealizados();
        }
        // PAGO REALIZADOS - dataGridView6
        //REFRESH LISTA DE PAGOS REALIZADOS DEL USUARIO E INICIALIZA LOS BOTONES Y CELDAS
        private void refreshVistaPagoRealizados()
        {
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            button14.Enabled = false;
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO
            dataGridView6.Rows.Clear();
            foreach (List<string> pap in banco.MostrarPagosPagados())
                dataGridView6.Rows.Add(pap.ToArray());
            // MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedPagoID != 0)
            {
                label22.Text = "Pago Seleccionado \n ID:  " + selectedPagoID;
            }
            else
            {
                label22.Text = "Seleccione un Pago";
            }
        }
        // PAGOS REALIZADOS CAPTURA EL CLICK EN EL DATAGRIDVIEW PARA TOMAR EL ID
        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Habilito los botones
                button14.Enabled = true;
                //habilito el id
                selectedPagoID = int.Parse(dataGridView6.Rows[e.RowIndex].Cells[0].Value.ToString());
                label22.Text = "Pago Seleccionado \n ID:  " + selectedPagoID;
                ;
            }
        }
        // PAGOS REALIZADOS ELIMINAR PAGO
        private void button14_Click(object sender, EventArgs e)
        {
            this.TransfEventoEliminaPago(selectedPagoID);
            refreshVistaPagoRealizados();

        }
        // PLAZO FIJO
        //  PLAZO FIJO VALIDACION
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox3.Text.Length == 0 || comboBox3.SelectedIndex == -1)
            {
                button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;

            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox3.Text.Length == 0 || comboBox3.SelectedIndex == -1)
            {
                button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;
            }
        }
        // PF - VALIDACION SI SE SELECCIONO ALGUN USUARIO PARA ALTA
        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            button12.Enabled = false;
            comboBox3.Enabled = false;

            if (comboBox11.SelectedIndex != -1)
            {
                // ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL USUARIO PARA DEBITAR ALTA PF 
                selectedUserID = ((ComboBoxItem)comboBox11.SelectedItem).HiddenValue;
                comboBox3.Items.Clear();
                comboBox3.ResetText();
                comboBox3.Enabled = true;
                if (banco.ObtenerCajasDeAhorroByUser(selectedUserID) != null)
                    foreach (CajaDeAhorro ca in banco.ObtenerCajasDeAhorroByUser(selectedUserID))
                    {
                        string displayValue = "CBU: " + ca.cbu.ToString() + " | Saldo: (" + ca.saldo + ")";
                        int hiddenValue = ca.id;
                        comboBox3.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
                    }
            }
            if (adminFlag == true && comboBox11.SelectedIndex == -1 && comboBox3.SelectedIndex == -1)
            {
                button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;
            }
        }
        //CHEQUEO 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || textBox3.Text.Length == 0 || comboBox3.SelectedIndex == -1)
            {
                button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;
            }
        }
        // HABILITA EL BOTON DE ALTA CAJA DE AHORRO SOLO SI ES ADMIN Y ELIGIO ALGUN USUARIO DE LA LISTA
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (adminFlag == true && comboBox8.SelectedIndex == -1)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        //REFRESH LISTA DE PLAZOS FIJOS DEL USUARIO E INICIALIZA LOS BOTONES Y CELDAS
        private void refreshVistaPlazosFijos()
        {
            // SETEA OPCIONES X SI NO ES ADMIN Y SI ES ADMIN LO EJECUTA
            comboBox11.Enabled = false;
            label28.Visible = false;
            button12.Enabled = true;
            if (adminFlag == true)
            {
                comboBox11.Enabled = true;
                label28.Visible = true;
                button12.Enabled = false;
            }
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            btn_BajaPF.Enabled = false;
            textBox3.Text = "";
            // ACTUALIZO GRILLA DE PRODUCTOS DISPONIBLES DEL USUARIO
            dataGridView4.Rows.Clear();
            foreach (List<string> pf in banco.MostrarPlazoFijos())
                dataGridView4.Rows.Add(pf.ToArray());
            // ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL USUARIO PARA DEBITAR ALTA PF 
            comboBox3.Items.Clear();
            comboBox3.ResetText();
            comboBox3.Enabled = false;
            // ACTUALIZO LA LISTA DE CAJAS DE AHORRO DEL USUARIO PARA DEBITAR ALTA PF 
            if (adminFlag == false)
            {
                comboBox3.Enabled = true;
                foreach (CajaDeAhorro ca in banco.ObtenerCajasDeAhorro())
                {
                    string displayValue = "CBU: " + ca.cbu.ToString() + " | Saldo: (" + ca.saldo + ")";
                    int hiddenValue = ca.id;
                    comboBox3.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.CA));
                }
            }
            // ACTUALIZO LA LISTA DE PLAZOS DISPONIBLES PARA ALTA PF
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            comboBox1.Items.Add(new ComboBoxItem(" 30 Dias", 30, tipoProducto.PF));
            comboBox1.Items.Add(new ComboBoxItem(" 60 Dias", 60, tipoProducto.PF));
            comboBox1.Items.Add(new ComboBoxItem(" 90 Dias", 90, tipoProducto.PF));
            // ACTUALIZO LA LISTA DE USUARIOS PARA ALTA DE CA
            comboBox11.Items.Clear();
            comboBox11.ResetText();
            foreach (Usuario user in banco.MostrarUsuarios())
            {
                string displayValue = user.nombre + " | (" + user.dni + ")";
                int hiddenValue = user.id;
                comboBox11.Items.Add(new ComboBoxItem(displayValue, hiddenValue, tipoProducto.USUARIO));
            }
            // MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedPFID != 0)
            {
                label23.Text = "Plazo Fijo Seleccionado \n ID:  " + selectedPFID;
            }
            else
            {
                label23.Text = "Seleccione un Plazo Fijo";
            }
        }
        // PLAZOS FIJOS CAPTURA EL CLICK EN EL DATAGRIDVIEW PARA TOMAR EL ID
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // HABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO 
                btn_BajaPF.Enabled = true;
                //habilito el id
                selectedPFID = int.Parse(dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString());
                label23.Text = "Plazo Fijo Seleccionado \n ID:  " + selectedPFID;
                ;
            }
        }
        // PLAZOS FIJOS ALTA 
        private void button12_Click(object sender, EventArgs e)
        {
            // SI ES ADMIN Y SELECCIONA ALGUN CLIENTE LO ENVIA SINO -1
            selectedUserID = -1;
            if (adminFlag == true)
            {
                selectedUserID = ((ComboBoxItem)comboBox11.SelectedItem).HiddenValue;

            }
            float monto = float.Parse(textBox3.Text);
            int plazo = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
            selectedCAID = ((ComboBoxItem)comboBox3.SelectedItem).HiddenValue;
            this.TransfEventoAltaPF(selectedCAID, monto, plazo, selectedUserID);
            refreshVistaPlazosFijos();
        }
        // PLAZOS FIJOS BAJA
        private void btn_BajaPF_Click(object sender, EventArgs e)
        {
            this.TransfEventoBajaPF(selectedPFID);
            refreshVistaPlazosFijos();
        }
        private void refreshVistaUsuarios()
        {
            // DESHABILITA ELEMENTOS QUE IMPLICAN ID DE PRODUCTO PARA QUE EL USUARIO TENGA QUE ELEGIR UN ELEMENTO DE LA GRILLA
            dni_alta.Text = "";
            textBox11.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            checkBox1.Checked = false;
            submit_alta.Enabled = false;
            button10.Enabled = false; // ADMIN
            groupBox13.Enabled = false;
            button16.Enabled = false; // desbloquear
            // ACTUALIZO GRILLA DE USUARIOS DISPONIBLES DEL BANCO
            dataGridView7.Rows.Clear();
            foreach (List<string> u in banco.MostrarUsuariosBanco())
                dataGridView7.Rows.Add(u.ToArray());
            //  MUESTRO EL PRODUCTO SELECCIONADO DE LA GRILLA SI ES QUE LA SELECCIONO
            if (selectedUserID > 0)
            {
                label9.Text = "Usuario Seleccionado ID:  " + selectedUserID;
            }
            else
            {
                label9.Text = "Seleccione un Usuario";
            }
        }
        //------------------------------------------------------------------
        private void submit_alta_Click(object sender, EventArgs e)
        {
            int id = selectedUserID;
            int dni = int.Parse(dni_alta.Text);
            string nombre = textBox8.Text;
            string apellido = textBox9.Text;
            string mail = textBox10.Text;
            string pass = textBox11.Text;
            this.TransfEventoModificarUsuario(id, nombre, apellido, mail, pass);
            refreshVistaUsuarios();
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {


                checkBox1.Checked = false;
                button16.Enabled = false;
                button10.Enabled = false;
                //Habilito los botones
                groupBox13.Enabled = true;
                selectedUserID = int.Parse(dataGridView7.Rows[e.RowIndex].Cells[0].Value.ToString());
                dni_alta.Text = dataGridView7.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox8.Text = dataGridView7.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox9.Text = dataGridView7.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox10.Text = dataGridView7.Rows[e.RowIndex].Cells[4].Value.ToString();
                // if bloqueado
                if (dataGridView7.Rows[e.RowIndex].Cells[5].Value.ToString() == "Yes")
                {
                    button16.Enabled = true;
                };
                // show if user is admin
                if (dataGridView7.Rows[e.RowIndex].Cells[6].Value.ToString() == "Yes")
                {
                    checkBox1.Checked = true;
                };
                if (dataGridView7.Rows[e.RowIndex].Cells[6].Value.ToString() == "No" && adminFlag == true)
                {
                    button10.Enabled = true;
                }

                textBox11.Text = dataGridView7.Rows[e.RowIndex].Cells[7].Value.ToString();
                submit_alta.Enabled = true;
                //habilito el id
                label34.Text = "Usuario seleccionada ID:  " + selectedUserID;
            }

        }


        // METODOS DELEGADOS
        public delegate void TransfDelegadoCerrarSesion();
        public delegate void TransfDelegadoDesbloqueoUsuario(int idUser);
        public delegate void TransfDelegadoSetAdmin(int idUser);

        public delegate void TransfDelegadoAltaCA(int idTitular);
        public delegate void TransfDelegadoBajaCA(int id);
        public delegate void TransfDelegadoDepositarCA(int idCaja, float montoD);
        public delegate void TransfDelegadoRetirarCA(int idCaja, float montoR);
        public delegate void TransfDelegadoTransferirCA(int idCajaO, int idCajaD, float monto);
        public delegate void TransfDelegadoVerMovimientosCA(int idCaja);
        public delegate void TransfDelegadoAltaCoTtitularesCA(int idCaja, int IdUser);
        public delegate void TransfDelegadoBajaCoTtitularesCA(int idCaja, int IdUser);

        public delegate void TransfDelegadoAltaTC(float limite, int idTitular);
        public delegate void TransfDelegadoBajaTC(int idTarjeta);
        public delegate void TransfDelegadoModificaLimiteTC(int idTarjeta, float limiteN);
        public delegate void TransfDelegadoPagaTC(int idTarjeta, int idCaja);

        public delegate void TransfDelegadoAltaPago(string nombre, float monto, string metodo, int idTitular);
        public delegate void TransfDelegadoPagaPago(int idPago, int? idTarjeta, int? idCaja);
        public delegate void TransfDelegadoEliminaPago(int idPago);

        public delegate void TransfDelegadoAltaPF(int idCuenta, float monto, int dias, int idTitular);
        public delegate void TransfDelegadoBajaPF(int idPlazoFijo);
        public delegate void TransfDelegadoModificarUsuario(int idUser, string? nombreN, string? apellidoN, string? mailN, string? passN);


    }
}