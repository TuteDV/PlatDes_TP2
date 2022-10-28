USE [master]
GO

/****** Object:  Database [banco]    Script Date: 19/10/2022 17:11:21 ******/
DROP DATABASE IF EXISTS [banco]
GO

/****** Object:  Database [banco]    Script Date: 19/10/2022 17:11:21 ******/
CREATE DATABASE [banco]
 
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [banco].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [banco] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [banco] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [banco] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [banco] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [banco] SET ARITHABORT OFF 
GO

ALTER DATABASE [banco] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [banco] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [banco] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [banco] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [banco] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [banco] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [banco] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [banco] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [banco] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [banco] SET  DISABLE_BROKER 
GO

ALTER DATABASE [banco] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [banco] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [banco] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [banco] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [banco] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [banco] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [banco] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [banco] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [banco] SET  MULTI_USER 
GO

ALTER DATABASE [banco] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [banco] SET DB_CHAINING OFF 
GO

ALTER DATABASE [banco] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [banco] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [banco] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [banco] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [banco] SET QUERY_STORE = OFF
GO

ALTER DATABASE [banco] SET  READ_WRITE 
GO

--- Tablas
USE [banco]
GO

/****** Object:  Table [dbo].[CajaDeAhorro]    Script Date: 19/10/2022 17:21:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CajaDeAhorro](
	[id_cajadeahorro] [int] IDENTITY(1,1) NOT NULL,
	[cbu] [int] NOT NULL,
	[saldo] [float] NOT NULL,
 CONSTRAINT [PK_CajaDeAhorro] PRIMARY KEY CLUSTERED 
(
	[id_cajadeahorro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CajaDeAhorroMovimiento]    Script Date: 19/10/2022 17:22:12 ******/
CREATE TABLE [dbo].[CajaDeAhorroMovimiento](
	[id_cajadeahorro] [int] NOT NULL,
	[id_movimiento] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CajaDeAhorroTitular]    Script Date: 19/10/2022 17:22:28 ******/
CREATE TABLE [dbo].[CajaDeAhorroTitular](
	[id_cajadeahorro] [int] NOT NULL,
	[id_usuario] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Movimiento]    Script Date: 19/10/2022 17:22:52 ******/
CREATE TABLE [dbo].[Movimiento](
	[id_movimiento] [int] IDENTITY(1,1) NOT NULL,
	[id_caja] [int] NOT NULL,
	[detalle] [nvarchar](100) NOT NULL,
	[monto] [float] NOT NULL,
	[fecha] [date] NOT NULL,
 CONSTRAINT [PK_Movimiento] PRIMARY KEY CLUSTERED 
(
	[id_movimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Pago]    Script Date: 19/10/2022 17:23:23 ******/
CREATE TABLE [dbo].[Pago](
	[id_pago] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[monto] [float] NOT NULL,
	[pagado] [bit] NOT NULL,
	[metodo] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_pago] PRIMARY KEY CLUSTERED 
(
	[id_pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PlazoFijo]    Script Date: 19/10/2022 17:23:43 ******/
CREATE TABLE [dbo].[PlazoFijo](
	[id_plazofijo] [int] IDENTITY(1,1) NOT NULL,
	[id_titular] [int] NOT NULL,
	[monto] [float] NOT NULL,
	[fecha_ini] [date] NOT NULL,
	[fecha_fin] [date] NOT NULL,
	[tasa] [float] NOT NULL,
	[pagado] [bit] NOT NULL,
	[cbu_alta] [int] NOT NULL,
 CONSTRAINT [PK_plazofijo] PRIMARY KEY CLUSTERED 
(
	[id_plazofijo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Tarjeta]    Script Date: 19/10/2022 17:23:56 ******/
CREATE TABLE [dbo].[Tarjeta](
	[id_tarjeta] [int] IDENTITY(1,1) NOT NULL,
	[id_titular] [int] NOT NULL,
	[numero] [int] NOT NULL,
	[codigoV] [int] NOT NULL,
	[limite] [float] NOT NULL,
	[consumos] [float] NOT NULL,
 CONSTRAINT [PK_Tarjeta] PRIMARY KEY CLUSTERED 
(
	[id_tarjeta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Usuario]    Script Date: 19/10/2022 17:24:17 ******/
CREATE TABLE [dbo].[Usuario](
	[id_usuario] [int] IDENTITY(1,1) NOT NULL,
	[dni] [int] NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[apellido] [nvarchar](100) NOT NULL,
	[mail] [nvarchar](200) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[intentosFallidos] [int] NOT NULL,
	[bloqueado] [bit] NOT NULL,
	[admin] [bit] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UsuarioCajaDeAhorro]    Script Date: 19/10/2022 17:24:32 ******/
CREATE TABLE [dbo].[UsuarioCajaDeAhorro](
	[id_usuario] [int] NOT NULL,
	[id_cajadeahorro] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UsuarioPago]    Script Date: 19/10/2022 17:24:54 ******/
CREATE TABLE [dbo].[UsuarioPago](
	[id_usuario] [int] NOT NULL,
	[id_pago] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UsuarioPlazoFijo]    Script Date: 19/10/2022 17:25:11 ******/
CREATE TABLE [dbo].[UsuarioPlazoFijo](
	[id_usuario] [int] NOT NULL,
	[id_plazofijo] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UsuarioTarjeta]    Script Date: 19/10/2022 17:25:27 ******/
CREATE TABLE [dbo].[UsuarioTarjeta](
	[id_usuario] [int] NOT NULL,
	[id_tarjeta] [int] NOT NULL
) ON [PRIMARY]
GO

-- Insert test data
USE [banco]
GO

INSERT INTO [dbo].[Usuario] VALUES (111,N'Admin',N'Admin',N'admin@banquito.com.ar',N'1234',0,0,1);
INSERT INTO [dbo].[Usuario] VALUES (222,N'Usernomal',N'Usernormal',N'user@banquito.com.ar',N'1234',0,0,0);
INSERT INTO [dbo].[Usuario] VALUES (33443462,N'Eliana',N'Alberti',N'eliana.alberti@davinci.edu.ar',N'1234',0,0,0);
INSERT INTO [dbo].[Usuario] VALUES (26901405,N'Gustavo',N'Beer',N'gustavo.beer@davinci.edu.ar',N'1234',0,0,0);
INSERT INTO [dbo].[Usuario] VALUES (36173485,N'Cristian',N'Manini',N'cristian.manini@davinci.edu.ar',N'1234',0,0,0);

INSERT INTO [dbo].[CajaDeAhorro] VALUES (111111,2458.00);
INSERT INTO [dbo].[CajaDeAhorro] VALUES (222222,4355.00);
INSERT INTO [dbo].[CajaDeAhorro] VALUES (333333,232.00);
INSERT INTO [dbo].[CajaDeAhorro] VALUES (444444,2323.00);
INSERT INTO [dbo].[CajaDeAhorro] VALUES (555555,2323.00);
INSERT INTO [dbo].[CajaDeAhorro] VALUES (666666,2323.00);

INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (1,2);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (1,3);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (2,4);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (3,2);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (4,3);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (5,4);
INSERT INTO [dbo].[CajaDeAhorroTitular] VALUES (6,2);

INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (2,1);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (3,1);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (4,2);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (2,3);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (3,4);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (4,5);
INSERT INTO [dbo].[UsuarioCajaDeAhorro] VALUES (2,6);

INSERT INTO [dbo].[Tarjeta] VALUES (2,123456,123,900000.00,0.00);
INSERT INTO [dbo].[Tarjeta] VALUES (3,123457,123,400000.00,0.00);
INSERT INTO [dbo].[Tarjeta] VALUES (4,123458,123,500000.00,0.00);
INSERT INTO [dbo].[Tarjeta] VALUES (2,123459,123,200000.00,0.00);

INSERT INTO [dbo].[UsuarioTarjeta] VALUES (2,1);
INSERT INTO [dbo].[UsuarioTarjeta] VALUES (3,2);
INSERT INTO [dbo].[UsuarioTarjeta] VALUES (4,3);
INSERT INTO [dbo].[UsuarioTarjeta] VALUES (2,4);

INSERT INTO [dbo].[PlazoFijo] VALUES (2,1000.00,'2022-03-01','2022-06-18',45.75,1,111111);
INSERT INTO [dbo].[PlazoFijo] VALUES (2,1000.00,'2022-10-01','2022-10-18',45.75,0,111111);
INSERT INTO [dbo].[PlazoFijo] VALUES (3,2000.00,'2022-09-18','2022-12-20',45.75,0,444444);

INSERT INTO [dbo].[Movimiento] VALUES (1,N'Deposito (Cr�dito)',2344.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (2,N'Deposito (Cr�dito)',4355.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (3,N'Deposito (Cr�dito)',232.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (4,N'Deposito (Cr�dito)',2323.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (5,N'Deposito (Cr�dito)',2323.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (6,N'Deposito (Cr�dito)',2323.00,'2022-01-01');
INSERT INTO [dbo].[Movimiento] VALUES (1,N'D�bito para Alta de Plazo Fijo',-1000.00,'2022-03-01');
INSERT INTO [dbo].[Movimiento] VALUES (1,N'Acreditacion de pago de Plazo fijo (Cr�dito)',1114.00,'2022-06-18');
INSERT INTO [dbo].[Movimiento] VALUES (1,N'D�bito para Alta de Plazo Fijo',-1000.00,'2022-10-01');
INSERT INTO [dbo].[Movimiento] VALUES (4,N'D�bito para Alta de Plazo Fijo',-2000.00,'2022-09-18');

INSERT INTO [dbo].[UsuarioPlazoFijo] VALUES (2,1);
INSERT INTO [dbo].[UsuarioPlazoFijo] VALUES (2,2);
INSERT INTO [dbo].[UsuarioPlazoFijo] VALUES (3,3);

INSERT INTO [dbo].[CajaDeAhorroMovimiento] VALUES (1,1);
INSERT INTO [dbo].[CajaDeAhorroMovimiento] VALUES (1,2);
INSERT INTO [dbo].[CajaDeAhorroMovimiento] VALUES (1,3);
INSERT INTO [dbo].[CajaDeAhorroMovimiento] VALUES (4,4);
