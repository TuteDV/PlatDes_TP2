using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace TP_1_BANCO
{
    class DAL
    {
        private string connectionString;

        public DAL()
        {
            //Cargo la cadena de conexión desde el archivo de properties
            connectionString = Properties.Resources.ConnectionStr;
        }
        
        //Inicializar Usuarios
        public List<Usuario> inicializarUsuarios()
        {
            List<Usuario> misUsuarios = new List<Usuario>();

            //Defino el string con la consulta que quiero realizar
            string queryString = "SELECT * from dbo.Usuario";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);
               
                try
                {
                    //Abro la conexión
                    connection.Open();
                    //mi objecto DataReader va a obtener los resultados de la consulta, notar que a comando se le pide ExecuteReader()
                    SqlDataReader reader = command.ExecuteReader();
                    Usuario aux;
                    //mientras haya registros/filas en mi DataReader, sigo leyendo
                    while (reader.Read())
                    {
                        aux = new Usuario(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetBoolean(7), reader.GetBoolean(8));
                        misUsuarios.Add(aux);
                    }
                    //En este punto ya recorrí todas las filas del resultado de la query
                    reader.Close();

                    //Ya cargue los usuarios, vinculaciones más adelante
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misUsuarios;
        }

        //Inicializar UserCaja
        public List<UserCaja> inicializarUserCaja()
        {
            List<UserCaja> userCajas = new List<UserCaja>();

            string queryString = "SELECT * from dbo.UsuarioCajaDeAhorro";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    UserCaja aux;
                    while (reader.Read())
                    {
                        aux = new UserCaja(reader.GetInt32(0), reader.GetInt32(1));
                        userCajas.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return userCajas;
        }

        //Inicializar UserPago
        public List<UserPago> inicializarUserPago()
        {
            List<UserPago> userPagos = new List<UserPago>();

            string queryString = "SELECT * from dbo.UsuarioPago";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    UserPago aux;
                    while (reader.Read())
                    {
                        aux = new UserPago(reader.GetInt32(0), reader.GetInt32(1));
                        userPagos.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return userPagos;
        }

        //Inicializar UserPlazo
        public List<UserPlazo> inicializarUserPlazo()
        {
            List<UserPlazo> userPlazos = new List<UserPlazo>();

            string queryString = "SELECT * from dbo.UsuarioPlazoFijo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    UserPlazo aux;
                    while (reader.Read())
                    {
                        aux = new UserPlazo(reader.GetInt32(0), reader.GetInt32(1));
                        userPlazos.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return userPlazos;
        }

        //Inicializar UserTarjetas
        public List<UserTarje> inicializarUserTarjetas()
        {
            List<UserTarje> userTarjetas = new List<UserTarje>();

            string queryString = "SELECT * from dbo.UsuarioTarjeta";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    UserTarje aux;
                    while (reader.Read())
                    {
                        aux = new UserTarje (reader.GetInt32(0), reader.GetInt32(1));
                        userTarjetas.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return userTarjetas;
        }


        //Inicializar CajasDeAhorro
        public List<CajaDeAhorro> inicializarCajasDeAhorro()
        {
            List<CajaDeAhorro> cajas = new List<CajaDeAhorro>();

            string queryString = "SELECT * from dbo.CajaDeAhorro";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CajaDeAhorro aux;
                    while (reader.Read())
                    {
                        aux = new CajaDeAhorro(reader.GetInt32(0), reader.GetInt32(1), (float)reader.GetDouble(2));
                        cajas.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return cajas;
        }

        //Inicializar CajaTitu
        public List<CajaTitu> inicializarCajaTitu()
        {
            List<CajaTitu> cajaTitus = new List<CajaTitu>();

            string queryString = "SELECT * from dbo.CajaDeAhorroTitular";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CajaTitu aux;
                    while (reader.Read())
                    {
                        aux = new CajaTitu (reader.GetInt32(0), reader.GetInt32(1));
                        cajaTitus.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return cajaTitus;
        }

        //Inicializar CajaMov
        public List<CajaMov> inicializarCajaMov()
        {
            List<CajaMov> cajaMovs = new List<CajaMov>();

            string queryString = "SELECT * from dbo.CajaDeAhorroMovimiento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    CajaMov aux;
                    while (reader.Read())
                    {
                        aux = new CajaMov(reader.GetInt32(0), reader.GetInt32(1));
                        cajaMovs.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return cajaMovs;
        }

        //Inicializar PlazosFijos
        public List<PlazoFijo> inicializarPlazosFijos()
        {
            List<PlazoFijo> misPlazos = new List<PlazoFijo>();

            string queryString = "SELECT * from dbo.PlazoFijo";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    PlazoFijo aux;

                    while (reader.Read())
                    {
                        aux = new PlazoFijo(reader.GetInt32(0), reader.GetInt32(1), (float)reader.GetDouble(2), reader.GetDateTime(3), reader.GetDateTime(4), (float)reader.GetDouble(5),reader.GetBoolean(6),reader.GetInt32(7));
                        
                        misPlazos.Add(aux);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misPlazos;
        }

        //Inicializar Tarjetas
        public List<Tarjeta> inicializarTarjetas()
        {
            List<Tarjeta> misTarjetas = new List<Tarjeta>();

            string queryString = "SELECT * from dbo.Tarjeta";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Tarjeta aux;
                    while (reader.Read())
                    {
                        aux = new Tarjeta(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), (float)reader.GetDouble(4), (float)reader.GetDouble(5));
                        misTarjetas.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misTarjetas;
        }

        //Inicializar Pagos
        public List<Pago> inicializarPagos()
        {
            List<Pago> misPagos = new List<Pago>();

            string queryString = "SELECT * from dbo.Pago";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Pago aux;
                    while (reader.Read())
                    {
                        aux = new Pago(reader.GetInt32(0), reader.GetInt32(1),reader.GetString(2), (float)reader.GetDouble(3), reader.GetBoolean(4), reader.GetString(5));
                        misPagos.Add(aux);
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misPagos;
        }

        //Inicializar Movimientos
        public List<Movimiento> inicializarMovimientos()
        {
            List<Movimiento> misMovimientos = new List<Movimiento>();
            string queryString = "SELECT * from dbo.Movimiento";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
               SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Movimiento aux;
                    while (reader.Read())
                    {
                        aux = new Movimiento(reader.GetInt32(0), reader.GetInt32(1),reader.GetString(2), (float)reader.GetDouble(3), reader.GetDateTime(4));
                        misMovimientos.Add(aux);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return misMovimientos;
        }

        //Agregar nuevo usuario: Devuelve el ID del usuario agregado a la base, si algo falla devuelve -1
        public int agregarUsuario(int dni, string nombre, string apellido, string mail, string password, int intentosF, bool bloqueado, bool admin)
        {
            int resultadoQuery;
            int idNuevoUsuario = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[Usuario] ([dni],[nombre],[apellido],[mail],[password],[intentosFallidos],[bloqueado],[admin]) VALUES (@dni,@nombre,@apellido,@mail,@password,@intentosF,@bloqueado,@admin);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@apellido", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@intentosF", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@admin", SqlDbType.Bit));
                command.Parameters["@dni"].Value = dni;
                command.Parameters["@nombre"].Value = nombre;
                command.Parameters["@apellido"].Value = apellido;
                command.Parameters["@mail"].Value = mail;
                command.Parameters["@password"].Value = password;
                command.Parameters["@intentosF"].Value = intentosF;
                command.Parameters["@bloqueado"].Value = bloqueado;
                command.Parameters["@admin"].Value = admin;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();
                    string ConsultaID = "SELECT MAX([id_usuario]) FROM [dbo].[Usuario]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idNuevoUsuario = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idNuevoUsuario;
            }
        }

        //Modificar Nombre del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarNombreUsuario(int id,string nombre)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET nombre=@nombre WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters["@id"].Value = id;
                command.Parameters["@nombre"].Value = nombre;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar Apellido del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarApellidoUsuario(int id, string apellido)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET apellido=@apellido WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@apellido", SqlDbType.NVarChar));
                command.Parameters["@id"].Value = id;
                command.Parameters["@apellido"].Value = apellido;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar Mail del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarMailUsuario(int id, string mail)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET mail=@mail WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                command.Parameters["@id"].Value = id;
                command.Parameters["@mail"].Value = mail;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar Password del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarPasswordUsuario(int id, string pass)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET password=@pass WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@pass", SqlDbType.NVarChar));
                command.Parameters["@id"].Value = id;
                command.Parameters["@pass"].Value = pass;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar Bloqueo del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien) 
        public int modificarBloqueoUsuario(int id, bool block)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET bloqueado=@block WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@block", SqlDbType.Bit));
                command.Parameters["@id"].Value = id;
                command.Parameters["@block"].Value = block;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar Admin del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarAdminUsuario(int id, bool admin)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET admin=@admin WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@admin", SqlDbType.Bit));
                command.Parameters["@id"].Value = id;
                command.Parameters["@admin"].Value = admin;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar intentosFallidos del Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int modificarAdminUsuario(int id, int intF)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Usuario] SET intentosFallidos=@intF WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@intF", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                command.Parameters["@intF"].Value = intF;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Eliminar Usuario: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int eliminarUsuario(int id)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[Usuario] WHERE id_usuario=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Agregar nueva Caja de Ahorro y relaciones: Devuelve el ID de la caja agregada a la base, si algo falla devuelve -1
        public int agregarCajaDeAhorro(int cbu, float saldo, int idUsuario)
        {
            int resultadoQuery;
            int resultadoQuery2;
            int resultadoQuery3;
            int idCajaN = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[CajaDeAhorro] ([cbu],[saldo]) VALUES (@cbu,@saldo);";
            string queryString2 = "INSERT INTO [dbo].[CajaDeAhorroTitular] ([id_cajadeahorro],[id_usuario]) VALUES (@idCaja,@idUser);";
            string queryString3 = "INSERT INTO [dbo].[UsuarioCajaDeAhorro] ([id_usuario],[id_cajadeahorro]) VALUES (@idUser,@idCaja);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                SqlCommand command3 = new SqlCommand(queryString3, connection);
                command.Parameters.Add(new SqlParameter("@cbu", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));
                command.Parameters["@cbu"].Value = cbu;
                command.Parameters["@saldo"].Value = saldo;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();
                    string ConsultaID = "SELECT MAX([id_cajadeahorro]) FROM [dbo].[CajaDeAhorro]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idCajaN = reader.GetInt32(0);
                    reader.Close();

                    //Asigno la relacion entre titular-caja y usuario-caja
                    command2.Parameters.Add(new SqlParameter("@idCaja", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@idUser", SqlDbType.Int));
                    command2.Parameters["@idCaja"].Value = idCajaN;
                    command2.Parameters["@idUser"].Value = idUsuario;
                    resultadoQuery2 = command2.ExecuteNonQuery();

                    command3.Parameters.Add(new SqlParameter("@idUser", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("@idCaja", SqlDbType.Int));
                    command3.Parameters["@idUser"].Value = idUsuario;
                    command3.Parameters["@idCaja"].Value = idCajaN;
                    resultadoQuery3 = command3.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idCajaN;
            }
        }

        //Obtener último CBU
        public int obtenerUltimoCBU()
        {
            int lastCbu=0;
            string ConsultaID = "SELECT MAX([cbu]) FROM [dbo].[CajaDeAhorro]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(ConsultaID, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    lastCbu = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return lastCbu;
        }

        //Eliminar Caja de Ahorro y relaciones con usuario: devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int eliminarCaja(int id)
        {
            int rta = 0;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[CajaDeAhorro] WHERE id_cajadeahorro=@id;";
            string queryString2 = "DELETE FROM [dbo].[CajaDeAhorroTitular] WHERE id_cajadeahorro=@id;";
            string queryString3 = "DELETE FROM [dbo].[UsuarioCajaDeAhorro] WHERE id_cajadeahorro=@id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command2.Parameters["@id"].Value = id;
                SqlCommand command3 = new SqlCommand(queryString3, connection);
                command3.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command3.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    rta = command.ExecuteNonQuery();
                    rta = rta + command2.ExecuteNonQuery();
                    rta = rta + command3.ExecuteNonQuery();
                    return rta;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return rta;
                }
            }
        }

        //Agregar titular a caja de ahorro: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int cajaAgregarTitular(int idCaja, int idUser)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[CajaDeAhorroTitular] ([id_cajadeahorro],[id_usuario]) VALUES (@idCaja,@idUser);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@idCaja", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@idUser", SqlDbType.Int));
                command.Parameters["@idCaja"].Value = idCaja;
                command.Parameters["@idUser"].Value = idUser;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Eliminar titular a caja de ahorro: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int cajaEliminarTitular(int idCaja, int idUser)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[CajaDeAhorroTitular] WHERE id_cajadeahorro=@idCaja AND id_usuario=@idUser;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@idCaja", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@idUser", SqlDbType.Int));
                command.Parameters["@idCaja"].Value = idCaja;
                command.Parameters["@idUser"].Value = idUser;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Update saldo a caja de ahorro: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int cajaUpdateSaldo(int idCaja, float saldo)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[CajaDeAhorro] SET saldo=@saldo WHERE id_cajadeahorro=@idCaja;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@idCaja", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@saldo", SqlDbType.Float));
                command.Parameters["@idCaja"].Value = idCaja;
                command.Parameters["@saldo"].Value = saldo;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Agregar nuevo Pago: Devuelve el ID del Pago agregado a la base, si algo falla devuelve -1
        public int agregarPago(int iduser, string nombre, float monto, bool pagado, string metodo)
        {
            int resultadoQuery;
            int resultadoQuery2;
            int idPagoN = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[Pago] ([id_user],[nombre],[monto],[pagado],[metodo]) VALUES (@iduser,@nombre,@monto,@pagado,@metodo);";
            string queryString2 = "INSERT INTO [dbo].[UsuarioPago] ([id_usuario],[id_pago]) VALUES (@iduser,@idPagoN);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command.Parameters.Add(new SqlParameter("@iduser", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@metodo", SqlDbType.NVarChar));
                command.Parameters["@iduser"].Value = iduser;
                command.Parameters["@nombre"].Value = nombre;
                command.Parameters["@monto"].Value = monto;
                command.Parameters["@pagado"].Value = pagado;
                command.Parameters["@metodo"].Value = metodo;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();

                    string ConsultaID = "SELECT MAX([id_pago]) FROM [dbo].[Pago]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idPagoN = reader.GetInt32(0);
                    reader.Close();

                    //Asigno la relacion entre usuario-pago
                    command2.Parameters.Add(new SqlParameter("@iduser", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@idPagoN", SqlDbType.Int));
                    command2.Parameters["@iduser"].Value = iduser;
                    command2.Parameters["@idPagoN"].Value = idPagoN;
                    resultadoQuery2 = command2.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idPagoN;
                ;
            }
        }

        //Eliminar Pago y relación con usuario: devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int eliminarPago(int id)
        {
            int rta = 0;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[Pago] WHERE id_pago=@id;";
            string queryString2 = "DELETE FROM [dbo].[UsuarioPago] WHERE id_pago=@id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command2.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    rta = command.ExecuteNonQuery();
                    rta = rta + command2.ExecuteNonQuery();
                    return rta;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return rta;
                }
            }
        }

        //Agregar nuevo PlazoFijo: Devuelve el ID del PlazoFijo agregado a la base, si algo falla devuelve -1
        public int agregarPlazoFijo(int iduser, float monto, DateTime fechaIni, DateTime fechaFin, float tasa, bool pagado, int cbuAlta)
        {
            int resultadoQuery;
            int resultadoQuery2;
            int idPFN = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[PlazoFijo] ([id_titular],[monto],[fecha_ini],[fecha_fin],[tasa],[pagado],[cbu_alta]) VALUES (@iduser,@monto,@fechaIni,@fechaFin,@tasa,@pagado,@cbuAlta);";
            string queryString2 = "INSERT INTO [dbo].[UsuarioPlazoFijo] ([id_usuario],[id_plazofijo]) VALUES (@iduser,@idPFN);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command.Parameters.Add(new SqlParameter("@iduser", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@fechaIni", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@fechaFin", SqlDbType.DateTime));
                command.Parameters.Add(new SqlParameter("@tasa", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@pagado", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@cbuAlta", SqlDbType.Int));
                command.Parameters["@iduser"].Value = iduser;
                command.Parameters["@monto"].Value = monto;
                command.Parameters["@fechaIni"].Value = fechaIni;
                command.Parameters["@fechaFin"].Value = fechaFin;
                command.Parameters["@tasa"].Value = tasa;
                command.Parameters["@pagado"].Value = pagado;
                command.Parameters["@cbuAlta"].Value = cbuAlta;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();

                    string ConsultaID = "SELECT MAX([id_plazofijo]) FROM [dbo].[PlazoFijo]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idPFN = reader.GetInt32(0);
                    reader.Close();

                    //Asigno la relacion entre usuario-pago
                    command2.Parameters.Add(new SqlParameter("@iduser", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@idPFN", SqlDbType.Int));
                    command2.Parameters["@iduser"].Value = iduser;
                    command2.Parameters["@idPFN"].Value = idPFN;
                    resultadoQuery2 = command2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idPFN;
                ;
            }
        }

        //Eliminar PlazoFijo y relación con usuario: devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int eliminarPlazoFijo (int id)
        {
            int rta = 0;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[PlazoFijo] WHERE id_plazofijo=@id;";
            string queryString2 = "DELETE FROM [dbo].[UsuarioPlazoFijo] WHERE id_plazofijo=@id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command2.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    rta = command.ExecuteNonQuery();
                    rta = rta + command2.ExecuteNonQuery();
                    return rta;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return rta;
                }
            }
        }

        //Agregar nuevo Movimiento: Devuelve el ID del Moviento agregado a la base, si algo falla devuelve -1
        public int agregarMovimiento(int idcaja, string detalle, float monto, DateTime fecha)
        {
            int resultadoQuery;
            int resultadoQuery2;
            int idMov = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[Movimiento] ([id_caja],[detalle],[monto],[fecha]) VALUES (@idcaja,@detalle,@monto,@fecha);";
            string queryString2 = "INSERT INTO [dbo].[CajaDeAhorroMovimiento] ([id_cajadeahorro],[id_movimiento]) VALUES (@idcaja,@idMov);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command.Parameters.Add(new SqlParameter("@idcaja", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@detalle", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@monto", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime));
                command.Parameters["@idcaja"].Value = idcaja;
                command.Parameters["@detalle"].Value = detalle;
                command.Parameters["@monto"].Value = monto;
                command.Parameters["@fecha"].Value = fecha;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();

                    string ConsultaID = "SELECT MAX([id_movimiento]) FROM [dbo].[Movimiento]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idMov = reader.GetInt32(0);
                    reader.Close();

                    //Asigno la relacion entre usuario-pago
                    command2.Parameters.Add(new SqlParameter("@idcaja", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@idMov", SqlDbType.Int));
                    command2.Parameters["@idcaja"].Value = idcaja;
                    command2.Parameters["@idMov"].Value = idMov;
                    resultadoQuery2 = command2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idMov;
                ;
            }
        }

        public int agregarTarjeta(int idtitu, int numero, int codigoV, float limite, float consumos)
        {
            int resultadoQuery;
            int resultadoQuery2;
            int idTCN = -1;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "INSERT INTO [dbo].[Tarjeta] ([id_titular],[numero],[codigoV],[limite],[consumos]) VALUES (@idtitu,@numero,@codV,@lim,@con);";
            string queryString2 = "INSERT INTO [dbo].[UsuarioTarjeta] ([id_usuario],[id_tarjeta]) VALUES (@idtitu,@idTCN);";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command.Parameters.Add(new SqlParameter("@idtitu", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@numero", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@codV", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@lim", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@con", SqlDbType.Float));
                command.Parameters["@idtitu"].Value = idtitu;
                command.Parameters["@numero"].Value = numero;
                command.Parameters["@codV"].Value = codigoV;
                command.Parameters["@lim"].Value = limite;
                command.Parameters["@con"].Value = consumos;
                try
                {
                    connection.Open();
                    resultadoQuery = command.ExecuteNonQuery();

                    //Ahora hago esta query para obtener el ID
                    string ConsultaID = "SELECT MAX([id_tarjeta]) FROM [dbo].[Tarjeta]";
                    command = new SqlCommand(ConsultaID, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    idTCN = reader.GetInt32(0);
                    reader.Close();

                    //Asigno la relacion entre usuario-pago
                    command2.Parameters.Add(new SqlParameter("@idtitu", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@idTCN", SqlDbType.Int));
                    command2.Parameters["@idtitu"].Value = idtitu;
                    command2.Parameters["@idTCN"].Value = idTCN;
                    resultadoQuery2 = command2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return idTCN;
                ;
            }
        }

        //Eliminar Tarjeta y relación con usuario: devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int eliminarTarjeta(int id)
        {
            int rta = 0;
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "DELETE FROM [dbo].[Tarjeta] WHERE id_tarjeta=@id;";
            string queryString2 = "DELETE FROM [dbo].[UsuarioTarjeta] WHERE id_tarjeta=@id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command2.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    rta = command.ExecuteNonQuery();
                    rta = rta + command2.ExecuteNonQuery();
                    return rta;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return rta;
                }
            }
        }

        //Obtener último numero de Tarjeta
        public int obtenerUltimoNumeroTarjeta()
        {
            int lastTC = -1;
            string ConsultaID = "SELECT MAX([numero]) FROM [dbo].[Tarjeta]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(ConsultaID, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    lastTC = reader.GetInt32(0);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return lastTC;
        }

        //Update limite de tarjeta: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int tarjetaUpdateLimite(int idTarjeta, float limite)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[Tarjeta] SET limite=@limite WHERE id_tarjeta=@idTarjeta;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@idTarjeta", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@limite", SqlDbType.Float));
                command.Parameters["@idTarjeta"].Value = idTarjeta;
                command.Parameters["@limite"].Value = limite;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Modificar PlazoFijo Pagado=True: Devuelve la cantidad de elementos modificados en la base (debería ser 1 si anduvo bien)
        public int plazoFijoSetPagado (int id)
        {
            string connectionString = Properties.Resources.ConnectionStr;
            string queryString = "UPDATE [dbo].[PlazoFijo] SET pagado=1 WHERE id_plazofijo=@id;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters["@id"].Value = id;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

    }
}
