USE [master]
GO
/****** Object:  Database [BancoDB]    Script Date: 3/06/2025 2:10:56 a. m. ******/
CREATE DATABASE [BancoDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BancoDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BancoDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BancoDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BancoDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BancoDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BancoDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BancoDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BancoDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BancoDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BancoDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BancoDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BancoDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BancoDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BancoDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BancoDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BancoDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BancoDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BancoDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BancoDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BancoDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BancoDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BancoDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BancoDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BancoDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BancoDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BancoDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BancoDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BancoDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BancoDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BancoDB] SET  MULTI_USER 
GO
ALTER DATABASE [BancoDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BancoDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BancoDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BancoDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BancoDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BancoDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BancoDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [BancoDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BancoDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuenta]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuenta](
	[NumeroCuenta] [nvarchar](20) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[Saldo] [decimal](18, 2) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[Confirmada] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NumeroCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[Id] [uniqueidentifier] NOT NULL,
	[NumeroCuenta] [nvarchar](max) NOT NULL,
	[Saldo] [decimal](18, 2) NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[Verificada] [bit] NOT NULL,
	[UsuarioId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResetPasswordTokens]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResetPasswordTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Identificacion] [nvarchar](max) NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[Expiracion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ResetPasswordTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaccion]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaccion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [uniqueidentifier] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[CuentaOrigen] [nvarchar](20) NOT NULL,
	[CuentaDestino] [nvarchar](20) NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Tipo] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transacciones]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transacciones](
	[Id] [uniqueidentifier] NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Tipo] [nvarchar](max) NOT NULL,
	[CuentaOrigenId] [uniqueidentifier] NOT NULL,
	[CuentaDestinoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Transacciones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransaccionesInterbancarias]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransaccionesInterbancarias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumeroTransaccion] [nvarchar](max) NOT NULL,
	[TipoDocumento] [nvarchar](max) NOT NULL,
	[NumeroDocumento] [nvarchar](max) NOT NULL,
	[NumeroCuentaDestino] [nvarchar](max) NOT NULL,
	[BancoDestino] [nvarchar](max) NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Moneda] [nvarchar](max) NOT NULL,
	[MontoConvertidoCOP] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[CuentaOrigenId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TransaccionesInterbancarias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [nvarchar](20) NOT NULL,
	[TipoUsuario] [nvarchar](50) NOT NULL,
	[Nombres] [nvarchar](100) NOT NULL,
	[Apellidos] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](10) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[ContrasenaHash] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Telefono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 3/06/2025 2:10:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [uniqueidentifier] NOT NULL,
	[Identificacion] [nvarchar](max) NOT NULL,
	[TipoUsuario] [nvarchar](max) NOT NULL,
	[Nombres] [nvarchar](max) NOT NULL,
	[Apellidos] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[FechaNacimiento] [datetime2](7) NOT NULL,
	[ContrasenaHash] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Cuentas_UsuarioId]    Script Date: 3/06/2025 2:10:56 a. m. ******/
CREATE NONCLUSTERED INDEX [IX_Cuentas_UsuarioId] ON [dbo].[Cuentas]
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transacciones_CuentaDestinoId]    Script Date: 3/06/2025 2:10:56 a. m. ******/
CREATE NONCLUSTERED INDEX [IX_Transacciones_CuentaDestinoId] ON [dbo].[Transacciones]
(
	[CuentaDestinoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transacciones_CuentaOrigenId]    Script Date: 3/06/2025 2:10:56 a. m. ******/
CREATE NONCLUSTERED INDEX [IX_Transacciones_CuentaOrigenId] ON [dbo].[Transacciones]
(
	[CuentaOrigenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransaccionesInterbancarias_CuentaOrigenId]    Script Date: 3/06/2025 2:10:56 a. m. ******/
CREATE NONCLUSTERED INDEX [IX_TransaccionesInterbancarias_CuentaOrigenId] ON [dbo].[TransaccionesInterbancarias]
(
	[CuentaOrigenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cuenta] ADD  DEFAULT ((0)) FOR [Saldo]
GO
ALTER TABLE [dbo].[Cuenta] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Cuenta] ADD  DEFAULT ((0)) FOR [Confirmada]
GO
ALTER TABLE [dbo].[Transaccion] ADD  DEFAULT (newid()) FOR [Numero]
GO
ALTER TABLE [dbo].[Transaccion] ADD  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[Cuenta]  WITH CHECK ADD  CONSTRAINT [FK_Cuenta_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Cuenta] CHECK CONSTRAINT [FK_Cuenta_Usuario]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Cuentas_Usuarios_UsuarioId]
GO
ALTER TABLE [dbo].[Transaccion]  WITH CHECK ADD  CONSTRAINT [FK_Transaccion_CuentaDestino] FOREIGN KEY([CuentaDestino])
REFERENCES [dbo].[Cuenta] ([NumeroCuenta])
GO
ALTER TABLE [dbo].[Transaccion] CHECK CONSTRAINT [FK_Transaccion_CuentaDestino]
GO
ALTER TABLE [dbo].[Transaccion]  WITH CHECK ADD  CONSTRAINT [FK_Transaccion_CuentaOrigen] FOREIGN KEY([CuentaOrigen])
REFERENCES [dbo].[Cuenta] ([NumeroCuenta])
GO
ALTER TABLE [dbo].[Transaccion] CHECK CONSTRAINT [FK_Transaccion_CuentaOrigen]
GO
ALTER TABLE [dbo].[Transacciones]  WITH CHECK ADD  CONSTRAINT [FK_Transacciones_Cuentas_CuentaDestinoId] FOREIGN KEY([CuentaDestinoId])
REFERENCES [dbo].[Cuentas] ([Id])
GO
ALTER TABLE [dbo].[Transacciones] CHECK CONSTRAINT [FK_Transacciones_Cuentas_CuentaDestinoId]
GO
ALTER TABLE [dbo].[Transacciones]  WITH CHECK ADD  CONSTRAINT [FK_Transacciones_Cuentas_CuentaOrigenId] FOREIGN KEY([CuentaOrigenId])
REFERENCES [dbo].[Cuentas] ([Id])
GO
ALTER TABLE [dbo].[Transacciones] CHECK CONSTRAINT [FK_Transacciones_Cuentas_CuentaOrigenId]
GO
ALTER TABLE [dbo].[TransaccionesInterbancarias]  WITH CHECK ADD  CONSTRAINT [FK_TransaccionesInterbancarias_Cuentas_CuentaOrigenId] FOREIGN KEY([CuentaOrigenId])
REFERENCES [dbo].[Cuentas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransaccionesInterbancarias] CHECK CONSTRAINT [FK_TransaccionesInterbancarias_Cuentas_CuentaOrigenId]
GO
USE [master]
GO
ALTER DATABASE [BancoDB] SET  READ_WRITE 
GO
