CREATE DATABASE BancoDB;
GO

USE BancoDB;
GO

-- Tabla Usuario
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Identificacion NVARCHAR(20) NOT NULL UNIQUE,
    TipoUsuario NVARCHAR(50) NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(10) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    ContrasenaHash NVARCHAR(MAX) NOT NULL
);
GO

-- Tabla Cuenta
CREATE TABLE Cuenta (
    NumeroCuenta NVARCHAR(20) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Saldo DECIMAL(18,2) NOT NULL DEFAULT 0,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    Confirmada BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Cuenta_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);
GO

-- Tabla Transaccion
CREATE TABLE Transaccion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero UNIQUEIDENTIFIER NOT NULL UNIQUE DEFAULT NEWID(),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    CuentaOrigen NVARCHAR(20) NOT NULL,
    CuentaDestino NVARCHAR(20) NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    Tipo NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_Transaccion_CuentaOrigen FOREIGN KEY (CuentaOrigen) REFERENCES Cuenta(NumeroCuenta),
    CONSTRAINT FK_Transaccion_CuentaDestino FOREIGN KEY (CuentaDestino) REFERENCES Cuenta(NumeroCuenta)
);
GO
