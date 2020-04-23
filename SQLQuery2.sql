CREATE DATABASE TallerBoga
USE TallerBoga

CREATE SCHEMA Taller
CREATE SCHEMA Trabajo

CREATE TABLE Taller.TipoTrabajo (
    IdTipoTrabajo BIGINT IDENTITY( 1, 1 ) NOT NULL,
    Costo FLOAT NOT NULL,
    NombreConfeccion VARCHAR( 100 ) NOT NULL,

    CONSTRAINT PK_TIPOTRABAJO PRIMARY KEY( IdTipoTrabajo )
)

CREATE TABLE Taller.Cliente (
    IdCliente BIGINT IDENTITY( 1, 1 ) NOT NULL,
    Nombre VARCHAR( 80 ) NOT NULL,
    ApellidoPaterno VARCHAR( 80 ) NOT NULL,
    ApellidoMaterno VARCHAR( 80 ) NOT NULL,
    Telefono VARCHAR( 10 ) NOT NULL,

    CONSTRAINT PK_CLIENTE PRIMARY KEY( IdCliente )
)

CREATE TABLE Taller.Empleado (
    IdEmpleado BIGINT IDENTITY( 1, 1 ) NOT NULL,
    Nombre VARCHAR( 80 ) NOT NULL,
    ApellidoMaterno VARCHAR( 80 ) NOT NULL,
    ApellidoPaterno VARCHAR( 80 ) NOT NULL,
    Telefono VARCHAR( 50 ) NOT NULL,
    Direccion VARCHAR( 200 ) NOT NULL,

    CONSTRAINT PK_EMPLEADO PRIMARY KEY( IdEmpleado )
)

CREATE TABLE Taller.Confeccion (
    IdConfeccion BIGINT IDENTITY( 1, 1 ) NOT NULL,
    CostoTotal FLOAT NULL,
    FechaPedido DATETIME NULL,
    FechaEntrega DATETIME NULL,
    Anticipo FLOAT NOT NULL,
    IdCliente BIGINT NOT NULL,

    CONSTRAINT PK_CONFECCION PRIMARY KEY( IdConfeccion ),
    CONSTRAINT FK_CONFECCION_CLIENTE FOREIGN KEY( IdCliente ) REFERENCES Taller.Cliente( IdCliente ),
)

ALTER TABLE Taller.Confeccion ADD CONSTRAINT DF_FECHAPEDIDO DEFAULT GETDATE() FOR FechaPedido


CREATE TABLE Taller.Prenda (
    IdPrenda BIGINT IDENTITY( 1, 1 ) NOT NULL,
    IdConfeccion BIGINT NOT NULL,
    CostoTrabajo FLOAT NULL,
    CostoMaterial FLOAT NULL,
    Finalizado BIGINT NOT NULL,

    CONSTRAINT PK_PRENDA PRIMARY KEY( IdPrenda ),
    CONSTRAINT FK_PRENDA_CONFECCION FOREIGN KEY( IdConfeccion ) REFERENCES Taller.Confeccion( IdConfeccion )
)

CREATE TABLE Taller.Trabajo (
    IdTrabajo BIGINT IDENTITY( 1, 1 ) NOT NULL,
    IdTipoTrabajo BIGINT NOT NULL,
    IdPrenda BIGINT NOT NULL,
    IdEmpleado BIGINT NOT NULL,

    CONSTRAINT PK_TRABAJO PRIMARY KEY( IdTrabajo ),
    CONSTRAINT FK_TRABAJO_TIPO_TRABAJO FOREIGN KEY( IdTipoTrabajo ) REFERENCES Taller.TipoTrabajo( IdTipoTrabajo ),
    CONSTRAINT FK_TRABAJO_PRENDA FOREIGN KEY( IdPrenda ) REFERENCES Taller.Prenda( IdPrenda ),
    CONSTRAINT FK_TRABAJO_EMPLEADO FOREIGN KEY( IdEmpleado ) REFERENCES Taller.Empleado( IdEmpleado )
)

CREATE TABLE Taller.Material (
    IdMaterial BIGINT IDENTITY( 1, 1 ) NOT NULL,
    Descripcion VARCHAR( 200 ) NOT NULL,

    CONSTRAINT PK_MATERIAL PRIMARY KEY( IdMaterial )
)

CREATE TABLE Taller.MaterialParaTrabajo	(
    IdTrabajo BIGINT NOT NULL,
    IdMaterial BIGINT NOT NULL,
    Cantidad BIGINT NOT NULL,
    PrecioCliente FLOAT NOT NULL,

    CONSTRAINT FK_MPT_TIPO_TRABAJO FOREIGN KEY( IdTrabajo ) REFERENCES Taller.Trabajo( IdTrabajo ),
    CONSTRAINT FK_MPT_MATERIAL FOREIGN KEY( IdMaterial ) REFERENCES Taller.Material( IdMaterial )
)

CREATE TABLE Taller.Proveedor (
    IdProveedor BIGINT IDENTITY( 1, 1 ) NOT NULL,
    Nombre VARCHAR( 80 ) NOT NULL,
    Telefono VARCHAR( 50 ) NOT NULL,
    Direccion VARCHAR( 200 ) NOT NULL,
    
    CONSTRAINT PK_PROVEEDOR PRIMARY KEY (IdProveedor)
)

CREATE TABLE Taller.Compra (
    IdCompra BIGINT IDENTITY( 1, 1 ) NOT NULL,
    IdProveedor BIGINT NOT NULL,
    FechaCompra DATETIME NULL,
    Total FLOAT NOT NULL,
    
    CONSTRAINT PK_COMPRA PRIMARY KEY (IdCompra),
    CONSTRAINT FK_PROVEEDOR FOREIGN KEY( IdProveedor ) REFERENCES Taller.Proveedor( IdProveedor )
)

ALTER TABLE Taller.Compra ADD CONSTRAINT DF_FECHACOMPRA DEFAULT GETDATE() FOR FechaCompra

CREATE TABLE Taller.DetalleCompra (
    IdDetalleCompra BIGINT IDENTITY( 1, 1 ) NOT NULL,
    IdCompra BIGINT NOT NULL,
    IdMaterial BIGINT NOT NULL,
    CostoUnitario FLOAT NOT NULL,
    Cantidad BIGINT NOT NULL,
    Subtotal FLOAT NOT NULL,

    CONSTRAINT FK_DT_COMPRA FOREIGN KEY( IdCompra ) REFERENCES Taller.Compra( IdCompra ),
    CONSTRAINT FK_DT_MATERIAL FOREIGN KEY( IdMaterial ) REFERENCES Taller.Material( IdMaterial )
)


// una regla numérica y una regla de cadena
//alta baja modificación y eliminación

