using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Threading;
using static TP_1_BANCO.FormMain;

namespace TP_1_BANCO
{
    public partial class FormParent : Form
    {
        Banco banco;
        string tituloMsgBox;
        bool adminFlag;
        //string pass;
        //bool logued;
        //int dni;
        //string nombre;
        //string apellido;
        //string mail;


        FormLogin hijoLogin;
        FormMain hijoMain;
        FormMovs hijoMovs;


        public FormParent()
        {
            InitializeComponent();
            banco = new Banco();
            tituloMsgBox = "Banco Da Vinci";
            LoadHijoLogin(banco);


        }
        // METODOS INTERNOS PARA CARGA DE CADA FORM (parametria inicial)
        // hijoMain = - FormMain  - MAIN PRODUCTOS
        private void LoadHijoMain(Banco banco, bool adminFlag) {
            hijoMain = new FormMain(banco, adminFlag);
            hijoMain.MdiParent = this;
            hijoMain.TransfEventoCerrarSesion += TransfDelegadoCerrarSesion;
            hijoMain.TransfEventoModificarUsuario += TransfDelegadoModificarUsuario;
            hijoMain.TransfEventoDesbloquearUsuario += TransfDelegadoDesbloqueoUsuario;
            hijoMain.TransfEventoSetAdmin += TransfDelegadoSetAdmin;

            hijoMain.TransfEventoAltaCA += TransfDelegadoAltaCA;
            hijoMain.TransfEventoBajaCA += TransfDelegadoBajaCA;
            hijoMain.TransfEventoDepositarCA += TransfDelegadoDepositarCA;
            hijoMain.TransfEventoRetirarCA += TransfDelegadoRetirarCA;
            hijoMain.TransfEventoTransferirCA += TransfDelegadoTransferirCA;
            hijoMain.TransfEventoAltaCoTtitularesCA += TransfDelegadoAltaCoTtitularesCA;
            hijoMain.TransfEventoBajaCoTtitularesCA += TransfDelegadoBajaCoTtitularesCA;
            hijoMain.TransfEventoVerMovimientosCA += TransfDelegadoVerMovimientosCA;

            hijoMain.TransfEventoAltaTC += TransfDelegadoAltaTC;
            hijoMain.TransfEventoBajaTC += TransfDelegadoBajaTC;
            hijoMain.TransfEventoModificaLimiteTC += TransfDelegadoModificaLimiteTC;
            hijoMain.TransfEventoPagaTC += TransfDelegadoPagaTC;

            hijoMain.TransfEventoAltaPago += TransfDelegadoAltaPago;
            hijoMain.TransfEventoPagaPago += TransfDelegadoPagaPago;
            hijoMain.TransfEventoEliminaPago += TransfDelegadoEliminaPago ;

            hijoMain.TransfEventoAltaPF += TransfDelegadoAltaPF ;
            hijoMain.TransfEventoBajaPF += TransfDelegadoBajaPF ;

            hijoMain.Show();
            hijoMain.WindowState = FormWindowState.Maximized;

        }
        // hijoLogin = - FormLogin - LOGIN
        private void LoadHijoLogin(Banco banco)
        {
            hijoLogin = new FormLogin(banco);
            hijoLogin.MdiParent = this;
            hijoLogin.TransfEventoLogin += TransfDelegadoLogin;
            hijoLogin.TransfEventoAlta += TransfDelegadoAlta;
            hijoLogin.Show();
            hijoLogin.WindowState = FormWindowState.Maximized;
        }
        // hijoMovs = - FormLogin - LOGIN
        private void LoadHijoMovimientos(int idCaja) {
            hijoMovs = new FormMovs(banco, idCaja);
            hijoMovs.MdiParent = this;
            hijoMovs.TransfEventoVolverAMain += TransfDelegadoVolverAMain;
            hijoMovs.Show();
            hijoMovs.WindowState = FormWindowState.Maximized;
        }
        // METODOS DELEGADOS - FormLogin - LOGIN / ALTA
        // USUARIO LOGIN
        private void TransfDelegadoLogin(int dni, string pass)
        {
            if (dni > 0 && dni < 99999999 && pass.Length > 0)
            {
                if (banco.IniciarSesion(dni, pass))
                {
                    MessageBox.Show("Ingreso correcto, Usuario: " + banco.MostrarUsuarioActualNombre(), tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hijoLogin.Close();
                    adminFlag = banco.EsAdmin();
                    LoadHijoMain(banco, adminFlag);
                }
                else
                {
                    if (banco.MostrarUsuarioEstadoPorDNI(dni))
                    {
                        MessageBox.Show("Su usuario se encuentra bloqueado", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (banco.MostrarUsuarioIntentosFallidosPorDNI(dni) == -1)
                    {
                        MessageBox.Show("Credenciales Incorrectas", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Datos Incorrectos. \n Intentos: " + banco.MostrarUsuarioIntentosFallidosPorDNI(dni), tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar un usuario y contraseña validos!");
            }
        }
        // USUARIO ALTA
        private void TransfDelegadoAlta(int dni, string nombre, string apellido, string mail, string pass, bool admin)
        {
            if (dni > 0 && dni < 99999999 && pass.Length > 3 && nombre.Length > 3 && apellido.Length > 3 && mail.Length > 3)
            {
                if (banco.AltaUsurario(dni, nombre, apellido, mail, pass, admin))
                {
                    hijoLogin.deleteSelectionsAlta();
                    MessageBox.Show("Usuario de datos: " + dni + " | " + nombre + " | " + apellido + " | " + mail + "\n Dado de alta", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario de datos: " + dni + " | " + nombre + " | " + apellido + " | " + mail + "\n ERROR en alta", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Error en el ingreso de datos, por favor revise y vuelva a intentar.");

        }
        // USUARIO ADMIN DESBLOQUEO USUARIOS 
        private void TransfDelegadoDesbloqueoUsuario(int idUser) 
        {
            if (banco.DesbloquearUsuario(idUser))
            {
                MessageBox.Show("Usuario de ID: " + idUser + " Desbloqueado con exito ", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR Usuario de ID: " + idUser + " NO Desbloqueado", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // USUARIO ADMIN seteo un admin
        private void TransfDelegadoSetAdmin(int idUser)
        {
            if (banco.SetUsuarioAsAdmin(idUser))
            {
                MessageBox.Show("Usuario de ID: " + idUser + " se seteo Admin con exito ", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR Usuario de ID: " + idUser + " NO se seteo Admin", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // METODOS DELEGADOS - FormMain  - MAIN PRODUCTOS
        // CERRAR SESION 
        private void TransfDelegadoCerrarSesion()
        {
            if (banco.CerrarSesion())
            {
                MessageBox.Show("Sesion Cerrada con EXITO", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                hijoMain.Close();
                LoadHijoLogin(banco);
            }
            else
            {
                MessageBox.Show("ERROR Sesion NO Cerrada", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    // MODIFICACION DE USUARIO / DESBLOQUEO
        private void TransfDelegadoModificarUsuario(int idUser, string? nombreN, string? apellidoN, string? mailN, string? passN) 
        {
            if (banco.ModificarUsuario(idUser, nombreN, apellidoN, mailN, passN))
            {
                MessageBox.Show("Usuario modificado con exito ", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show("ERROR Usuario NO modificado", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO ------
        // CAJA DE AHORRO ALTA
        private void TransfDelegadoAltaCA(int idTitular)
        {
            if (banco.CrearCajaAhorro(idTitular))
            {
                MessageBox.Show("CA de Ahorro dada de alta existosamente", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR CA de Ahorro NO dada de alta.", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO BAJA
        private void TransfDelegadoBajaCA(int id)
        {
            if (banco.BajaCajaAhorro(id))
            {
                MessageBox.Show("CA de Ahorro dada de Baja exitosamente", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR CA de Ahorro NO dada de baja", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO DEPOSITO
        private void TransfDelegadoDepositarCA(int idCaja, float montoD)
        {
            if (banco.Depositar(idCaja, montoD))
            {
                MessageBox.Show("Deposito exitoso", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR en el deposito", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO RETIRO
        private void TransfDelegadoRetirarCA(int idCaja, float montoR)
        {
            if (banco.Retirar(idCaja, montoR))
            {
                MessageBox.Show("Retiro exitoso", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR en el retiro", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO TRANSFERENCIA
        private void TransfDelegadoTransferirCA(int idCajaO, int idCajaD, float monto)
        {
            if (banco.Transferir(idCajaO, idCajaD, monto))
            {
                MessageBox.Show("Transferencia exitosa", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR en la transferencia. No se realizo", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO AGREGA TITULARES
        private void TransfDelegadoAltaCoTtitularesCA(int idCaja, int IdUser)
        {
            if (banco.ModificarCajaAhorroAddTitular(idCaja, IdUser))
            {
                MessageBox.Show("Agregado de Titular exitoso", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR en el agredo de Titular. No se realizo", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO BAJA TITULARES
        private void TransfDelegadoBajaCoTtitularesCA(int idCaja, int IdUser)
        {
            if (banco.ModificarCajaAhorroRemoveTitular(idCaja, IdUser))
            {
                MessageBox.Show("Eliminacion de Titular exitoso", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR en la Baja de Titular. No se realizo", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // CAJA DE AHORRO MOVIMIENTOS
        private void TransfDelegadoVerMovimientosCA(int idCaja)
        {
                hijoMain.Close();
                LoadHijoMovimientos(idCaja);
        }
        // TARJETA DE CREDITO ------
        // TARJETA DE CREDITO ALTA
        private void TransfDelegadoAltaTC(float limite, int idTitular)
        {
            if (banco.AltaTarjetaCredito(limite, idTitular))
            {
                MessageBox.Show("TC dada de alta con limite de:" + limite + " para el cliente " + idTitular, tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR TC NO dada de alta", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // TARJETA DE CREDITO BAJA
        private void TransfDelegadoBajaTC(int idTarjeta)
        {
            if (idTarjeta > 0)
            {
                if (banco.BajaTarjetaCredito(idTarjeta))
                {
                    MessageBox.Show("TC " + idTarjeta + " dada de BAJA correctamente", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR TC NO dada de baja", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ERROR en los datos de ID TC seleccionados", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // TARJETA DE CREDITO MODIFICA LIMITE
        private void TransfDelegadoModificaLimiteTC(int idTarjeta, float limiteN)
        {
            if (idTarjeta > 0 && limiteN > 0)
            {
                if (banco.ModificarTarjetaCredito(idTarjeta, limiteN))
                {
                    MessageBox.Show("Para la TC " + idTarjeta + " el nuevo limite es " + limiteN, tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR TC se pudo cambiar el limite", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ERROR en los datos de ID TC o limite nuevo seleccionados", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // TARJETA DE CREDITO PAGA CONSUMOS
        private void TransfDelegadoPagaTC(int idTarjeta, int idCaja)
        {
            if (idTarjeta > 0 && idCaja > 0)
            {
                if (banco.PagarTarjeta(idTarjeta, idCaja))
                {
                    MessageBox.Show("La TC " + idTarjeta + " se ha abondado desde la CA " + idCaja, tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR TC no se pudo pagar", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ERROR en los datos de ID TC o ID Caja seleccinados", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // PAGO PENDIENTE
        // PAGO PENDIENTE ALTA
        private void TransfDelegadoAltaPago(string nombre, float monto, string metodo, int idTitular)
        {
            if (!string.IsNullOrEmpty(nombre) || monto > 0)
            {
                if (banco.AltaPago(nombre, monto, metodo, idTitular))
                {
                    MessageBox.Show("Pago de " + nombre + " dado alta por $ " + monto, tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR Pago NO dada de alta", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ERROR en los datos de Nombre o monto de pago seleccionados", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // PAGO PENDIENTE PAGO
        private void TransfDelegadoPagaPago(int idPago, int? idTarjeta, int? idCaja)
        {
            if (idTarjeta !=null || idCaja !=null)
            {
                if (banco.PagarPago(idPago, idTarjeta, idCaja))
                {
                    MessageBox.Show("Pago ID " + idPago + " se pago correctamente." , tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR El Pago NO pudo efectuarse", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ERROR en los datos Producto a debitar el pago seleccionados", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // PAGO REALIZADO
        // PAGO REALIZADO BAJA
        private void TransfDelegadoEliminaPago(int idPago)
        {
                if (banco.BajaPago(idPago)) 
                {
                    MessageBox.Show("Pago de " + idPago + " dado de baja ", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR Pago NO dada de baja", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        // PLAZO FIJO
        // PLAZO FIJO ALTA
        private void TransfDelegadoAltaPF(int idCuenta, float monto, int dias, int idTitular)
        {
                if (banco.AltaPlazoFijo(idCuenta,monto, dias, idTitular))
                {
                    MessageBox.Show("Plazo Fijo por $ " + monto + " dado alta por " + dias + "dias \n Debitado desde: la cuenta ID: " + idCuenta + " y Titular " + idTitular, tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ERROR Plazo Fijo NO dada de alta", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

    private void TransfDelegadoBajaPF(int idPlazoFijo)
    {
        if (banco.BajaPlazoFijo(idPlazoFijo))
        {
            MessageBox.Show("Plazo Fijo ID " + idPlazoFijo + "dado de baja Correctamente", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("ERROR Plazo Fijo NO dada de baja", tituloMsgBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
        


// METODOS DELEGADOS - FormMovs - VER MOVIMIENTOS
// VOLVER AL MAIN
private void TransfDelegadoVolverAMain(Banco banco)
        {
            hijoMovs.Close();
            LoadHijoMain(banco , this.adminFlag);
        }
    }
}