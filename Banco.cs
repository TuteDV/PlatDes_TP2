using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using TP_1_BANCO;

namespace TP_1_BANCO
{
    public class Banco
    {
        private List<Usuario> usuarios;
        private List<UserCaja> userCajas;
        private List<UserPago> userPagos;
        private List<UserPlazo> userPlazos;
        private List<UserTarje> userTarjetas;
        private List<CajaDeAhorro> cajasAhorro;
        private List<CajaTitu> cajasTitus;
        private List<CajaMov> cajasMovs;
        private List<PlazoFijo> plazosFijos;
        private List<Tarjeta> tarjetas;
        private List<Pago> pagos;
        private List<Movimiento> movimientos;
        private Usuario? usuarioActual;
        public float tasaPF { get; }
        public float LimiteInicialTC { get; }
        private DAL DB;


        public Banco()
        {
            DB = new DAL();
            usuarios = DB.inicializarUsuarios();
            userCajas = DB.inicializarUserCaja();
            userPagos = DB.inicializarUserPago();
            userPlazos = DB.inicializarUserPlazo();
            userTarjetas = DB.inicializarUserTarjetas();
            cajasAhorro = DB.inicializarCajasDeAhorro();
            cajasTitus = DB.inicializarCajaTitu();
            cajasMovs = DB.inicializarCajaMov();
            movimientos = DB.inicializarMovimientos();
            plazosFijos = DB.inicializarPlazosFijos();
            tarjetas = DB.inicializarTarjetas();
            pagos = DB.inicializarPagos();
            cargarReferenciasRelacionesEntidades();
            //Tasa para plazo fijo hardcodeada
            tasaPF = 45.75F;
            LimiteInicialTC = 10000.00F;
            ProcesamientoPlazosFijos();
        }

        private void cargarReferenciasRelacionesEntidades()
        {
            //Carga de referencias en memoria de Usuario
            foreach (UserCaja uca in userCajas)
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    foreach (Usuario us in usuarios)
                    {
                        if (uca.idCaja == ca.id && uca.idUser == us.id)
                        {
                            us.cajas.Add(ca);
                        }
                    }
                }
            }

            foreach (UserPago upa in userPagos)
            {
                foreach (Pago pa in pagos)
                {
                    foreach (Usuario us in usuarios)
                    {
                        if (upa.idPago == pa.id && upa.idUser == us.id)
                        {
                            us.pagos.Add(pa);
                            pa.user = us;
                        }
                    }
                }
            }

            foreach (UserTarje uta in userTarjetas)
            {
                foreach (Tarjeta ta in tarjetas)
                {
                    foreach (Usuario us in usuarios)
                    {
                        if (uta.idTarje == ta.id && uta.idUser == us.id)
                        {
                            us.tarjetas.Add(ta);
                            ta.titular = us;
                        }
                    }
                }
            }

            foreach (UserPlazo upf in userPlazos)
            {
                foreach (PlazoFijo pf in plazosFijos)
                {
                    foreach (Usuario us in usuarios)
                    {
                        if (upf.idPlazo == pf.id && upf.idUser == us.id)
                        {
                            us.plazosFijos.Add(pf);
                            pf.titular = us;
                        }
                    }
                }
            }


            //Carga de referencias en memoria de Cajas De Ahorro
            foreach (CajaTitu cat in cajasTitus)
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    foreach (Usuario us in usuarios)
                    {
                        if (cat.idCaja == ca.id && cat.idUser == us.id)
                        {
                            ca.titulares.Add(us);
                        }
                    }
                }
            }

            foreach (CajaMov cmo in cajasMovs)
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    foreach (Movimiento mov in movimientos)
                    {
                        if (cmo.idCaja == ca.id && cmo.idMov == mov.id)
                        {
                            ca.movimientos.Add(mov);
                            mov.caja = ca;
                        }
                    }
                }
            }
        }


        //ABM Clases
        // AltaUsuario
        public bool AltaUsurario(int dni, string nombre, string apellido, string mail, string pass, bool admin)
        {
            //si el DNI ya está registrado, no permite el alta
            if (usuarios.Exists(x => x.dni == dni))
            {
                return false;
            }
            else
            {
                int intF = 0;
                bool block = false;
                int idN = DB.agregarUsuario(dni, nombre, apellido, mail, pass, intF, block, admin);

                if (idN != -1)
                {
                    //Ahora sí lo agrego en la lista
                    Usuario user = new(idN, dni, nombre, apellido, mail, pass, intF, block, admin);
                    usuarios.Add(user);
                    return true;
                }
                else
                {
                    //algo salió mal con la query porque no generó un id válido
                    return false;
                }

            }
        }

        // ModificarUsuario: Permite actualizar Nombre, Apellido, Email, Password, Bloqueado o admin (todas o una) solamente 
        public bool ModificarUsuario(int idUser, string? nombreN, string? apellidoN, string? mailN, string? passN)
        {
            try
            {
                foreach (Usuario user in usuarios)
                {
                    if (user.id == idUser)
                    {
                        if (nombreN != null)
                        {
                            if (DB.modificarNombreUsuario(idUser, nombreN) == 1)
                            {
                                user.nombre = nombreN;
                            }
                        }
                        if (apellidoN != null)
                        {
                            if (DB.modificarApellidoUsuario(idUser, apellidoN) == 1)
                            {
                                user.apellido = apellidoN;
                            }
                        }
                        if (mailN != null)
                        {
                            if (DB.modificarMailUsuario(idUser, mailN) == 1)
                            {
                                user.mail = mailN;
                            }
                        }
                        if (passN != null)
                        {
                            if (DB.modificarPasswordUsuario(idUser, passN) == 1)
                            {
                                user.password = passN;
                            }
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

   
        // DesbloquearUsuario: Permite actualizar Bloqueado = False
        public bool DesbloquearUsuario(int idUser)
        {
            try
            {
                foreach (Usuario user in usuarios)
                {
                    if (user.id == idUser)
                    {

                        if (DB.modificarBloqueoUsuario(idUser, false) == 1)
                        {
                            user.bloqueado = false;
                        }

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // SetAsAdmin: Permite actualizar un usuario como admin = true
        public bool SetUsuarioAsAdmin(int idUser)
        {
            try
            {
                foreach (Usuario user in usuarios)
                {
                    if (user.id == idUser)
                    {

                        if (DB.modificarAdminUsuario(idUser, true) == 1)
                        {
                            user.admin = true;
                        }

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // EliminarUsuario: Elimina los productos (caja/PF/tarjeta/Pago) del usuario. Luego elimina el usuario Usuario.
        public bool EliminarUsuario(int idUser)
        {
            try
            {
                foreach (Usuario user in usuarios)
                {
                    if (user.id == idUser)
                    {
                        foreach (CajaDeAhorro ca in user.cajas)
                        {
                            if (ca.saldo == 0)
                            {
                                if (DB.eliminarCaja(ca.id) == 1)
                                {
                                    cajasAhorro.Remove(ca);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        foreach (Tarjeta ta in user.tarjetas)
                        {
                            if (ta.consumos == 0)
                            {
                                if (DB.eliminarTarjeta(ta.id) == 1)
                                {
                                    tarjetas.Remove(ta);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        foreach (PlazoFijo pf in user.plazosFijos)
                        {
                            if (pf.pagado == true)
                            {
                                if (DB.eliminarPlazoFijo(pf.id) == 1)
                                {
                                    plazosFijos.Remove(pf);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        foreach (Pago pa in user.pagos)
                        {
                            if (pa.pagado == true)
                            {
                                if (DB.eliminarPago(pa.id) == 1)
                                {
                                    pagos.Remove(pa);
                                }
                            }
                            else
                            {
                                return false;
                            }

                        }
                        if (DB.eliminarUsuario(user.id) == 1)
                        {
                            usuarios.Remove(user);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // CrearCajaAhorro(idTitular): Crea una nueva Caja de Ahorro para el usuario logeado.
        public bool CrearCajaAhorro(int idTitular)
        {
            Usuario titular = usuarioActual;

            if (usuarioActual.admin == true)
            {
                foreach (Usuario u in usuarios)
                {
                    if (u.id == idTitular)
                    {
                        titular = u;
                        break;
                    }
                }
            }
            int cbuOld = DB.obtenerUltimoCBU();
            if (cbuOld != 0)
            {
                int cbuN = cbuOld + 1;
                int idN = DB.agregarCajaDeAhorro(cbuN, 0.0F, titular.id);
                if (idN != -1)
                {
                    //Ahora sí lo agrego en la lista
                    CajaDeAhorro caja = new(idN, cbuN, 0.0F);
                    cajasAhorro.Add(caja);
                    titular.cajas.Add(caja);
                    caja.titulares.Add(titular);
                    return true;
                }
                else
                {
                    //algo salió mal con la query porque no generó un id válido
                    return false;
                }
            }
            return false;
        }

        // BajaCajaAhorro (Se agrega revisión que la misma no tenga PF activos
        public bool BajaCajaAhorro(int id)
        {
            foreach (CajaDeAhorro ca in cajasAhorro)
            {
                if (ca.id == id)
                {
                    if (ca.saldo == 0)
                    {
                        foreach (PlazoFijo pf in plazosFijos)
                        {
                            if (pf.pagado == false && pf.cbuAlta == ca.cbu)
                            {
                                return false;
                            }
                        }
                        if (DB.eliminarCaja(id) >= 3)
                        {
                            try
                            {
                                foreach (Usuario u in ca.titulares)
                                {
                                    u.cajas.Remove(ca);
                                }
                                cajasAhorro.Remove(ca);
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            //algo salió mal con la query porque no generó 1 registro
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        // ModificarCajaAhorro (in ID): Modifica datos de la Caja de Ahorro. Permite agregar o quitar un titular,
        // no puede quedar sin titulares. No se pueden modificar los movimientos. No se puede modificar el saldo.
        public bool ModificarCajaAhorroAddTitular(int idCaja, int idNuevoTitular)
        {
            try
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    if (ca.id == idCaja)
                    {
                        foreach (Usuario user in usuarios)
                        {
                            //Reviso que el usuario no sea ya titular
                            if (user.id == idNuevoTitular)
                            {
                                if (!ca.titulares.Contains(user))
                                {
                                    if (DB.cajaAgregarTitular(idCaja, idNuevoTitular) == 1)
                                    {
                                        ca.titulares.Add(user);
                                        user.cajas.Add(ca);
                                        return true;
                                    }
                                }
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ModificarCajaAhorroRemoveTitular(int idCaja, int idRemoveTitular)
        {
            try
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    if (ca.id == idCaja)
                    {
                        foreach (Usuario user in usuarios)
                        {
                            if (user.id == idRemoveTitular)
                            {
                                //Reviso que tengas dos o mas titulares antes de eliminar
                                if (ca.titulares.Count >= 2)
                                {
                                    if (DB.cajaEliminarTitular(idCaja, idRemoveTitular) == 1)
                                    {
                                        ca.titulares.Remove(user);
                                        user.cajas.Remove(ca);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // AltaPago
        public bool AltaPago(string nombre, float monto, string metodo, int idTitular)
        {
            Usuario titular = usuarioActual;

            if (usuarioActual.admin == true)
            {
                foreach (Usuario u in usuarios)
                {
                    if (u.id == idTitular)
                    {
                        titular = u;
                        break;
                    }
                }
            }
            int idN = DB.agregarPago(titular.id, nombre, monto, false, metodo);
            if (idN != -1)
            {
                //Ahora sí lo agrego en la lista
                Pago pago = new(idN, titular, nombre, monto, metodo);
                pagos.Add(pago);
                titular.pagos.Add(pago);
                return true;
            }
            else
            {
                //algo salió mal con la query porque no generó un id válido
                return false;
            }
        }

        // BajaPago: Solo si esta pagado 
        public bool BajaPago(int id)
        {
            try
            {
                foreach (Pago pa in pagos)
                {
                    if (pa.id == id && pa.pagado)
                    {
                        if (DB.eliminarPago(id) >= 2)
                        {
                            pa.user.pagos.Remove(pa);
                            pagos.Remove(pa);
                            return true;
                        }
                        else
                        {
                            //algo salió mal con la query porque no generó 1 registro
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //AltaPlazoFijo
        public bool AltaPlazoFijo(int idCuenta, float monto, int dias, int idTitular)
        {
            Usuario titular = usuarioActual;

            if (usuarioActual.admin == true)
            {
                foreach (Usuario u in usuarios)
                {
                    if (u.id == idTitular)
                    {
                        titular = u;
                        break;
                    }
                }
            }

            //Se valida que el monto sea mínimo $1.000 para poder crearlo
            if (monto >= 1000.0F)
            {
                //Controlo si tengo saldo suficiente en cuenta para crearlo
                foreach (CajaDeAhorro ca in titular.cajas)
                {
                    if (ca.id == idCuenta && ca.saldo >= monto)
                    {
                        DateTime fechaIni = DateTime.Now;
                        DateTime fechaFin = fechaIni.AddDays(dias);

                        int idN = DB.agregarPlazoFijo(titular.id, monto, fechaIni, fechaFin, tasaPF, false, ca.cbu);
                        if (idN != -1)
                        {
                            //Genero movimiento y actualizo saldo en cuenta
                            int idM = DB.agregarMovimiento(ca.id, "Débito para Alta de Plazo Fijo", -monto, fechaIni);
                            float saldoN = ca.saldo - monto;
                            int idC = DB.cajaUpdateSaldo(ca.id, saldoN);

                            if (idM != -1)
                            {
                                //Ahora sí lo agrego en la lista
                                PlazoFijo plazo = new(idN, titular, monto, fechaIni, fechaFin, tasaPF, ca.cbu);
                                Movimiento mov = new Movimiento(idM, ca, "Débito para Alta de Plazo Fijo", -monto);
                                ca.saldo = saldoN;
                                ca.movimientos.Add(mov);
                                movimientos.Add(mov);
                                titular.plazosFijos.Add(plazo);
                                plazosFijos.Add(plazo);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //BajaPlazoFijo (in ID)
        public bool BajaPlazoFijo(int idPlazo)
        {

            foreach (PlazoFijo p in plazosFijos)
            {
                //Se valida que el este pagago y fecha actual es 1 mes posterior a la Fecha Fin
                if (p.id == idPlazo && (DateTime.Now.CompareTo(p.fechaFin.AddDays(-30)) > 0)
                    && p.pagado)
                {
                    // Elimina el Plazo Fijo de la lista del usuario logeado y del banco.
                    if (DB.eliminarPlazoFijo(idPlazo) >= 2)
                    {
                        plazosFijos.Remove(p);
                        p.titular.plazosFijos.Remove(p);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }
            }
            return false;
        }

        //AltaTarjetaCredito: Nueva tarjeta + Asociar usuario y banco
        public bool AltaTarjetaCredito(float limite, int idTitular)
        {
            Usuario titular = usuarioActual;

            if (usuarioActual.admin == true)
            {
                foreach (Usuario u in usuarios)
                {
                    if (u.id == idTitular)
                    {
                        titular = u;
                        break;
                    }
                }
            }

            try
            {
                int numeroOld = DB.obtenerUltimoNumeroTarjeta();
                if (numeroOld != -1)
                {
                    int numN = numeroOld + 1;
                    Random rnd = new Random();
                    int codigoVN = rnd.Next(1000);
                    int idN = DB.agregarTarjeta(titular.id, numN, codigoVN, limite, 0.0F);
                    if (idN != -1)
                    {
                        //Ahora sí lo agrego en la lista
                        Tarjeta tarjeta = new Tarjeta(idN, titular, numN, codigoVN, limite);
                        tarjetas.Add(tarjeta);
                        titular.tarjetas.Add(tarjeta);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó un id válido
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        //BajaTarjetaCredito (in ID)
        public bool BajaTarjetaCredito(int idTarjeta)
        {
            foreach (Tarjeta t in tarjetas)
            {
                //Se valida que la misma tenga $0 en consumos
                if (t.id == idTarjeta && t.consumos == 0.0F)
                {
                    // Elimina el tarjeta de la lista del usuario logeado y del banco.
                    if (DB.eliminarTarjeta(idTarjeta) >= 2)
                    {
                        tarjetas.Remove(t);
                        t.titular.tarjetas.Remove(t);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }
            }
            return false;
        }

        //ModificarTarjetaCredito (in ID): Solo permite modificar el límite
        public bool ModificarTarjetaCredito(int idTarjeta, float limiteN)
        {
            foreach (Tarjeta t in tarjetas)
            {
                if (t.id == idTarjeta && limiteN > 0.0F)
                {
                    if (DB.tarjetaUpdateLimite(idTarjeta, limiteN) == 1)
                    {
                        t.limite = limiteN;
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }
            }
            return false;
        }


        //Mostrar Datos

        //ObtenerCajasDeAhorro(): Devuelve una lista con las cajas de ahorro del usuario actual logueado.
        public List<CajaDeAhorro> ObtenerCajasDeAhorro()
        {
            if (usuarioActual.admin == true)
            {
                return cajasAhorro.ToList();
            }
            return usuarioActual.cajas.ToList();
        }

        //ObtenerCajasDeAhorro(): Devuelve una lista con las cajas de ahorro de todo el banco (para transferencia).
        public List<CajaDeAhorro> ObtenerCajasDeAhorroBanco()
        {
            return this.cajasAhorro.ToList();
        }

        //ObtenerCajasDeAhorroByUser(idUser): Devuelve una lista con las cajas de ahorro de un usuario x.
        public List<CajaDeAhorro> ObtenerCajasDeAhorroByUser(int idUser)
        {
            foreach (Usuario u in usuarios)
            {
                if (u.id==idUser)
                {
                    return u.cajas.ToList();
                }
            }
            return null;
        }

        //ObtenerCajasDeAhorroByTarjetaId(idTarjeta): Devuelve una lista con las cajas de ahorro que puedo usar para pagar una tarjeta de usuario x.
        public List<CajaDeAhorro> ObtenerCajasDeAhorroByTarjetaId(int idTarjeta)
        {
            foreach (Tarjeta tc in tarjetas)
            {
                if (tc.id == idTarjeta)
                {
                    foreach (Usuario u in usuarios)
                    {
                        if (u.id == tc.idUsuario)
                        {
                            return u.cajas.ToList();
                        }
                    }
                }
            }
            return null;
        }

        //ObtenerCajasDeAhorroByPagoId(): Devuelve una lista con las cajas de ahorro que puedo usar para hacer un pago de usuario x.
        public List<CajaDeAhorro> ObtenerCajasDeAhorroByPagoId(int idPago)
        {
            foreach (Pago pago in pagos)
            {
                if (pago.id == idPago)
                {
                    foreach (Usuario u in usuarios)
                    {
                        if (u.id == pago.idUsuario)
                        {
                            return u.cajas.ToList();
                        }
                    }
                }
            }
            return null;
        }

        //MostrarCajasDeAhorro(): Devuelve una lista de strings con los datos a mostrar de las cajas de ahorro del usuario actual logueado.
        public List<List<string>> MostrarCajasDeAhorro()
        {
            List<List<string>> salida = new List<List<string>>();

            if (usuarioActual.admin == true)
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    salida.Add(new List<string>() { ca.id.ToString(), ca.cbu.ToString(), ca.saldo.ToString() });
                }
            }
            else
            {
                foreach (CajaDeAhorro ca in usuarioActual.cajas)
                {
                    salida.Add(new List<string>() { ca.id.ToString(), ca.cbu.ToString(), ca.saldo.ToString() });
                }

            }
            return salida;
        }

        //ObtenerMovimientos (in Caja de Ahorro (id)): Devuelve una lista con los movimientos de la caja de ahorro ingresada.
        public List<Movimiento> ObtenerMovimientos(int idCaja)
        {
            foreach (CajaDeAhorro ca in cajasAhorro)
            {
                if (ca.id == idCaja)
                {
                    return ca.movimientos.ToList();
                }
            }
            return null;
        }

        //MostrarMovimientos (in Caja de Ahorro (id)): Devuelve una lista de strings con los movimientos de la caja de ahorro ingresada.
        public List<List<string>> MostrarMovimientos(int idCaja)
        {
            List<List<string>> salida = new List<List<string>>();

            foreach (CajaDeAhorro ca in cajasAhorro)
            {
                if (ca.id == idCaja)
                {
                    foreach (Movimiento mov in ca.movimientos)
                    {
                        salida.Add(new List<string>() { mov.id.ToString(), mov.detalle, mov.monto.ToString(), mov.fecha.ToString() });
                    }
                    return salida;
                }
            }
            return salida;
        }

        //ObtenerPagos(): Devuelve una lista con los objetos tipo Pago del usuario actual logueado.
        public List<Pago> ObtenerPagos()
        {
            if (usuarioActual.admin == true)
            {
                return pagos.ToList();
            }
            return usuarioActual.pagos.ToList();
        }

        //MostrarPagosPagados(): Devuelve una lista de strings con los pagos pagados del usuario actual logueado.
        public List<List<string>> MostrarPagosPagados()
        {
            List<List<string>> salida = new List<List<string>>();
            if (usuarioActual.admin == true)
            {
                foreach (Pago pa in pagos)
                {
                    if (pa.pagado == true)
                    {
                        salida.Add(new List<string>() { pa.id.ToString(), pa.nombre, pa.monto.ToString(), pa.metodo });
                    }
                }
            }
            else
            {
                foreach (Pago pa in usuarioActual.pagos)
                {
                    if (pa.pagado == true)
                    {
                        salida.Add(new List<string>() { pa.id.ToString(), pa.nombre, pa.monto.ToString(), pa.metodo });
                    }
                }
            }
            return salida;
        }

        //MostrarPagosPendientes(): Devuelve una lista de strings con los pagos pagados del usuario actual logueado.
        public List<List<string>> MostrarPagosPendientes()
        {
            List<List<string>> salida = new List<List<string>>();
            if (usuarioActual.admin == true)
            {
                foreach (Pago pa in pagos)
                {
                    if (pa.pagado == false)
                    {
                        salida.Add(new List<string>() { pa.id.ToString(), pa.nombre, pa.monto.ToString(), pa.metodo });
                    }
                }
            }
            else
            {
                foreach (Pago pa in usuarioActual.pagos)
                {
                    if (pa.pagado == false)
                    {
                        salida.Add(new List<string>() { pa.id.ToString(), pa.nombre, pa.monto.ToString(), pa.metodo });
                    }
                }
            }
            return salida;
        }

        //ObtenerPlazoFijos(): Devuelve una lista con los objetos plazo fijo del usuario actual logueado.
        public List<PlazoFijo> ObtenerPlazoFijos()
        {
            if (usuarioActual.admin == true)
            {
                return plazosFijos.ToList();
            }
            return usuarioActual.plazosFijos.ToList();
        }

        //MostrarPlazoFijos(): Devuelve una lista de strings con los plazo fijos del usuario actual logueado.
        public List<List<string>> MostrarPlazoFijos()
        {
            List<List<string>> salida = new List<List<string>>();
            if (usuarioActual.admin == true)
            {
                foreach (PlazoFijo pf in plazosFijos)
                {
                    if (pf.pagado == true)
                    {
                        salida.Add(new List<string>() { pf.id.ToString(), pf.monto.ToString(), pf.fechaIni.ToString(), pf.fechaFin.ToString(), pf.tasa.ToString(), "Si" });
                    }
                    else
                    {
                        salida.Add(new List<string>() { pf.id.ToString(), pf.monto.ToString(), pf.fechaIni.ToString(), pf.fechaFin.ToString(), pf.tasa.ToString(), "No" });
                    }  
                    
                }
            }
            else
            {
                foreach (PlazoFijo pf in usuarioActual.plazosFijos)
                {
                    if (pf.pagado == true)
                    {
                        salida.Add(new List<string>() { pf.id.ToString(), pf.monto.ToString(), pf.fechaIni.ToString(), pf.fechaFin.ToString(), pf.tasa.ToString(), "Si" });
                    }
                    else
                    {
                        salida.Add(new List<string>() { pf.id.ToString(), pf.monto.ToString(), pf.fechaIni.ToString(), pf.fechaFin.ToString(), pf.tasa.ToString(), "No" });
                    }
                }
            }
            return salida;
        }

        //ObtenerTarjetasDeCredito() : Devuelve una lista con las tarjetas de crédito del usuario actual logueado.
        public List<Tarjeta> ObtenerTarjetasDeCredito()
        {
            if (usuarioActual.admin == true)
            {
                return tarjetas.ToList();
            }
            return usuarioActual.tarjetas.ToList();
        }

        //ObtenerTarjetasByPagoId(): Devuelve una lista con las cajas de ahorro que puedo usar para hacer un pago de usuario x.
        public List<Tarjeta> ObtenerTarjetaByPagoId(int idPago)
        {
            foreach (Pago pago in pagos)
            {
                if (pago.id == idPago)
                {
                    foreach (Usuario u in usuarios)
                    {
                        if (u.id == pago.idUsuario)
                        {
                            return u.tarjetas.ToList();
                        }
                    }
                }
            }
            return null;
        }

        //MostrarTarjetas(): Devuelve una lista de strings con las tarjetas del usuario actual logueado.
        public List<List<string>> MostrarTarjetas()
        {
            List<List<string>> salida = new List<List<string>>();
            if (usuarioActual.admin == true)
            {
                foreach (Tarjeta tc in tarjetas)
                {
                    salida.Add(new List<string>() { tc.id.ToString(), tc.numero.ToString(), tc.codigoV.ToString(), tc.limite.ToString(), tc.consumos.ToString() });
                }
            }
            else
            {
                foreach (Tarjeta tc in usuarioActual.tarjetas)
                {
                    salida.Add(new List<string>() { tc.id.ToString(), tc.numero.ToString(), tc.codigoV.ToString(), tc.limite.ToString(), tc.consumos.ToString() });
                }
            }
            return salida;
        }


        //Operaciones de usuarios
        //MostrarUsuarios(): Se crea para mostrar la lista de usuarios (en ABM y agregar co titulares)
        public List<Usuario> MostrarUsuarios()
        {
            return this.usuarios.ToList();
        }

        //MostrarUsuariosBanco(): Devuelve una lista de strings con los usuarios del banco.
        public List<List<string>> MostrarUsuariosBanco()
        {
            List<List<string>> salida = new List<List<string>>();
            if (usuarioActual.admin == true)
            {
                foreach (Usuario u in usuarios)
                {
                    string string_bloq = "No";
                    string string_admin = "No";
                    if (u.bloqueado == true) { string_bloq = "Yes"; }
                    if (u.admin == true) { string_admin = "Yes"; }
                    salida.Add(new List<string>() { u.id.ToString(), u.dni.ToString(), u.nombre.ToString(), u.apellido.ToString(), u.mail.ToString(), string_bloq, string_admin, u.password.ToString() });
                }
            }
            return salida;
        }
        
        
        //MostrarUsuariosPorCajaDeAhorro(): Se crea para mostrar la lista de usuarios de una caja (para eliminar co titulares)
        public List<Usuario> MostrarUsuariosPorCajaDeAhorro(int idCaja)
        {
            foreach (CajaDeAhorro ca in cajasAhorro)
            {
                if (ca.id == idCaja)
                {
                    return ca.titulares.ToList();
                }
            }
            return null;
        }

        // MostrarUsuarioActualNombre: para mostrar el nombre usuario en forms desde lista banco
        public string MostrarUsuarioActualNombre()
        {
            return usuarioActual.nombre.ToString();

        }

        // MostrarUsuarioEstadoPorDNI: para mostrar si esta bloqueado o no
        public bool MostrarUsuarioEstadoPorDNI(int dni)
        {
            foreach (Usuario user in usuarios)
            {
                if (user.dni == dni)
                {
                    return user.bloqueado;
                }
            }
            return false;
        }

        // MostrarUsuarioIntentosFallidosPorDNI: para mostrar cantidad de intentos fallidos
        public int MostrarUsuarioIntentosFallidosPorDNI(int dni)
        {
            foreach (Usuario user in usuarios)
            {
                if (user.dni == dni)
                {
                    return user.intentosFallidos;
                }
            }
            return -1;
        }

        //IniciarSesion(in int DNI, in string Contraseña)
        public bool IniciarSesion(int dni, string pass)
        {
            foreach (Usuario user in usuarios)
            {
                if (user.dni == dni && user.bloqueado == false)
                {
                    if (user.password.Equals(pass))
                    {
                        usuarioActual = user;
                        return true;
                    }
                    else
                    {
                        user.intentosFallidos++;
                        if (user.intentosFallidos >= 3)
                        {
                            user.bloqueado = true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        //EsAdmin: Devuelve si el usuarioActual es admin o no
        public bool EsAdmin()
        {
            if (usuarioActual.admin == true)
            {
                return true;
            }
            return false;
        }

        //CerrarSesion():UsuarioActual pasa a ser nulo.
        public bool CerrarSesion()
        {
            usuarioActual = null;
            return true;
        }

        //Depositar (in Caja de Ahorro, float monto): Deposita el monto ingresado en la Caja de Ahorro seleccionada
        public bool Depositar(int idCaja, float montoD)
        {
            try
            {
                if (montoD > 0.0F)
                {
                    foreach (CajaDeAhorro ca in cajasAhorro)
                    {
                        if (ca.id == idCaja)
                        {
                            float saldoN = ca.saldo + montoD;
                            DateTime fecha = DateTime.Now;
                            if (DB.cajaUpdateSaldo(idCaja, saldoN) == 1)
                            {
                                //Genero movimiento y actualizo saldo en cuenta
                                int idM = DB.agregarMovimiento(ca.id, "Deposito (Crédito)", montoD, fecha);

                                if (idM != -1)
                                {
                                    ca.saldo = saldoN;
                                    Movimiento mov = new Movimiento(idM, ca, "Deposito (Crédito)", montoD);
                                    ca.movimientos.Add(mov);
                                    movimientos.Add(mov);
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //Retirar (in Caja de Ahorro (id), float monto): Retira el monto ingresado en la Caja de Ahorro seleccionada
        //siempre que cuente con saldo suficiente, caso contrario devuelve un error.
        public bool Retirar(int idCaja, float montoR)
        {
            try
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    if (ca.id == idCaja)
                    {
                        if (ca.saldo >= montoR)
                        {
                            float saldoN = ca.saldo - montoR;
                            DateTime fecha = DateTime.Now;
                            if (DB.cajaUpdateSaldo(idCaja, saldoN) == 1)
                            {
                                //Genero movimiento y actualizo saldo en cuenta
                                int idM = DB.agregarMovimiento(ca.id, "Retiro (Débito)", -montoR, fecha);

                                if (idM != -1)
                                {
                                    ca.saldo = saldoN;
                                    Movimiento mov = new Movimiento(idM, ca, "Retiro (Débito)", -montoR);
                                    ca.movimientos.Add(mov);
                                    movimientos.Add(mov);
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Transferir(in Caja de Ahorro origen (id), in Caja de Ahorro destino (id), float Monto) : Transfiere el monto
        //indicado de la caja de ahorro seleccionada (origen) a otra caja de ahorro, siempre que el saldo de origen sea suficiente para realizar
        //la operación caso contrario genera un error.
        public bool Transferir(int idCajaO, int idCajaD, float montoT)
        {
            try
            {
                foreach (CajaDeAhorro caO in cajasAhorro)
                {
                    if (caO.id == idCajaO)
                    {
                        if (caO.saldo >= montoT)
                        {
                            foreach (CajaDeAhorro caD in cajasAhorro)
                            {
                                if (caD.id == idCajaD)
                                {
                                    float saldoNcaO = caO.saldo - montoT;
                                    float saldoNcaD = caD.saldo + montoT;
                                    DateTime fecha = DateTime.Now;
                                    if (DB.cajaUpdateSaldo(idCajaO, saldoNcaO) == 1 && DB.cajaUpdateSaldo(idCajaD, saldoNcaD) == 1)
                                    {
                                        //Genero movimiento y actualizo saldo en las cuentas
                                        int idMO = DB.agregarMovimiento(idCajaO, "Se transfieren fondos hacia otra cuenta (Débito)", -montoT, fecha);
                                        int idMD = DB.agregarMovimiento(idCajaD, "Se reciben fondos desde otra cuenta (Crédito)", montoT, fecha);

                                        if (idMO != -1 && idMD != -1)
                                        {
                                            caO.saldo = saldoNcaO;

                                            Movimiento mov1 = new Movimiento(idMO, caO, "Se transfieren fondos hacia otra cuenta (Débito)", -montoT);
                                            caO.movimientos.Add(mov1);
                                            movimientos.Add(mov1);
                                            caD.saldo = saldoNcaD;

                                            Movimiento mov2 = new Movimiento(idMD, caD, "Se reciben fondos desde otra cuenta (Crédito)", montoT);
                                            caD.movimientos.Add(mov2);
                                            movimientos.Add(mov2);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //BuscarMovimientos(in Caja de Ahorro(id), in string Detalle, in date Fecha, in float Monto):
        //Devuelve una lista de movimientos de la caja de ahorro filtrada por al menos uno de los
        //parámetros (el usuario puede ingresar los 3 parámetros, 2 o 1) Detalle, Fecha y Monto.
        public List<List<string>> BuscarMovimiento(int idCaja, string detalleB, DateTime? fechaB, float? montoB)
        {
            List<List<string>> resultado = new List<List<string>>();
            try
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    if (ca.id == idCaja)
                    {
                        foreach (Movimiento mov in ca.movimientos)
                        {
                            if (mov.detalle.Equals(detalleB, StringComparison.OrdinalIgnoreCase) || mov.fecha.CompareTo(fechaB) == 0 || mov.monto == montoB)
                            {
                                resultado.Add(new List<string>() { mov.id.ToString(), mov.detalle, mov.monto.ToString(), mov.fecha.ToString() }); ;
                            }
                        }
                        return resultado;
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return resultado;
            }
        }

        //PagarTarjeta(in Tarjeta (id), in Caja de Ahorro (id)): Descuenta el saldo total de consumos de la tarjeta
        //del saldo de la caja de ahorro (generando el movimiento correspondiente) siempre que el saldo
        //sea suficiente, caso contrario, no permite operar.
        public bool PagarTarjeta(int idTarjeta, int idCaja)
        {
            try
            {
                foreach (CajaDeAhorro ca in cajasAhorro)
                {
                    if (ca.id == idCaja)
                    {
                        foreach (Tarjeta ta in tarjetas)
                        {
                            if (ta.id == idTarjeta)
                            {
                                if (ca.saldo >= ta.consumos)
                                {
                                    ca.saldo = ca.saldo - ta.consumos;
                                    int idM = movimientos.Count + 1;
                                    Movimiento mov = new Movimiento(idM, ca, "Pago de tarjeta (Débito)", -ta.consumos);
                                    ca.movimientos.Add(mov);
                                    movimientos.Add(mov);
                                    ta.consumos = 0.0F;
                                    return true;
                                }

                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //PagarPago (in Pago (id), Tarjeta (id), in Caja de Ahorro (id)): Para pagar el pago, se le permite al usuario
        //elegir en una lista de string donde figuran sus cuentas (CBU) y sus tarjetas (número). Al seleccionar pagar
        //se verifica que el instrumento seleccionado tenga saldo/límite disponible según corresponda, si es así se
        //descuenta el pago del medio y se genera un movimiento si es Caja de Ahorro o se aumentan los consumos si es tarjeta.
        //De lo contrario, no se permite operar por falta de saldo.
        public bool PagarPago(int idPago, int? idTarjeta, int? idCaja)
        {
            try
            {
                foreach (Pago pa in pagos)
                {
                    if (pa.id == idPago)
                    {
                        if (idTarjeta != null)
                        {
                            foreach (Tarjeta ta in tarjetas)
                            {
                                if (ta.id == idTarjeta)
                                {
                                    float limDis = ta.limite - ta.consumos;
                                    if (limDis >= pa.monto)
                                    {
                                        ta.consumos = ta.consumos + pa.monto;
                                        pa.pagado = true;
                                        return true;
                                    }

                                }
                            }
                        }
                        if (idCaja != null)
                        {
                            foreach (CajaDeAhorro ca in cajasAhorro)
                            {
                                if (ca.id == idCaja)
                                {
                                    if (ca.saldo >= pa.monto)
                                    {
                                        ca.saldo = ca.saldo - pa.monto;
                                        int idM = movimientos.Count + 1;
                                        Movimiento mov = new Movimiento(idM, ca, "Pago de Pago (Débito)", -pa.monto);
                                        ca.movimientos.Add(mov);
                                        movimientos.Add(mov);
                                        pa.pagado = true;
                                        return true;
                                    }

                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Extras
        //OBS: Al iniciar, el sistema DEBE recorrer la lista de plazo fijos, si alguno tiene FechaFin igual
        //a la fecha de hoy, entonces debe proceder a depositar el monto final, calcular monto ingresado +
        //interés (monto * (tasa / 365) * cantidad de días) y marcar el plazo fijo como pagado.
        public void ProcesamientoPlazosFijos()
        {
            foreach (PlazoFijo pf in plazosFijos)
            {
                if (pf.fechaFin.CompareTo(DateTime.Now) <= 0)
                {
                    int days = pf.fechaFin.Subtract(pf.fechaIni).Days;
                    float montoF = pf.monto + (pf.monto * (pf.tasa / 365.0F) * days);
                    foreach (CajaDeAhorro ca in cajasAhorro)
                    {
                        if (ca.cbu == pf.cbuAlta)
                        {
                            float saldoF = ca.saldo + montoF;
                            DateTime fecha = DateTime.Now;
                            if (DB.cajaUpdateSaldo(ca.id, saldoF) == 1 && DB.plazoFijoSetPagado(pf.id) == 1)
                            {
                                int idM = DB.agregarMovimiento(ca.id, "Acreditacion de pago de Plazo fijo (Crédito)", montoF, fecha);

                                if (idM != -1)
                                {
                                    ca.saldo = ca.saldo + montoF;
                                    Movimiento mov = new Movimiento(idM, ca, "Acreditacion de pago de Plazo fijo (Crédito)", montoF);
                                    ca.movimientos.Add(mov);
                                    movimientos.Add(mov);
                                    pf.pagado = true;
                                }
                            }
                        }
                    }
                }
            }

        }

    }
}