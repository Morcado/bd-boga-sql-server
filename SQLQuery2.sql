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
    Anticipo FLOAT NULL,
    IdCliente BIGINT NOT NULL,

    CONSTRAINT PK_CONFECCION PRIMARY KEY( IdConfeccion ),
    CONSTRAINT FK_CONFECCION_CLIENTE FOREIGN KEY( IdCliente ) REFERENCES Taller.Cliente( IdCliente ),
)

ALTER TABLE Taller.Confeccion ADD CONSTRAINT DF_FECHAPEDIDO DEFAULT GETDATE() FOR FechaPedido
ALTER TABLE Taller.Confeccion ALTER COLUMN Anticipo FLOAT NULL ;

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

ALTER TABLE Taller.DetalleCompra (
    IdDetalleCompra BIGINT IDENTITY( 1, 1 ) NOT NULL,
    IdCompra BIGINT NOT NULL,
    IdMaterial BIGINT NOT NULL,
    CostoUnitario FLOAT NULL,
    Cantidad BIGINT NOT NULL,
    Subtotal FLOAT NULL,

    CONSTRAINT FK_DT_COMPRA FOREIGN KEY( IdCompra ) REFERENCES Taller.Compra( IdCompra ),
    CONSTRAINT FK_DT_MATERIAL FOREIGN KEY( IdMaterial ) REFERENCES Taller.Material( IdMaterial )
)

ALTER TABLE Taller.DetalleCompra ALTER COLUMN CostoUnitario FLOAT NULL ;
ALTER TABLE Taller.DetalleCompra ALTER COLUMN Subtotal FLOAT NULL ;
ALTER TABLE Taller.DetalleCompra ADD CHECK ( ( CostoUnitario IS NOT NULL ) OR ( Subtotal IS NOT NULL ) )

--Triggers--------------------

--1er Trigger
--Cada vez que se inserte/elimine un registro de "Trabajo", se obtiene la prenda a la que corresponde el trabajo,
--y se obtiene el tipo de trabajo del trabajo. Luego, se suman los precios de todos los trabajos
--que se van a hacer sobre la prenda. Esto actualiza solo la prenda afectada y recalcula el "Costo" de "Prenda".
CREATE TRIGGER Taller.COSTO_PRENDA_TR ON Taller.Trabajo 
AFTER INSERT, DELETE
AS
BEGIN
	DECLARE @IdPrenda BIGINT ;

	IF EXISTS ( SELECT * FROM inserted )
		SET @IdPrenda = ( SELECT TOP 1 IdPrenda FROM inserted );
	ELSE
		SET @IdPrenda = ( SELECT TOP 1 IdPrenda FROM deleted );

	UPDATE Taller.Prenda SET Taller.Prenda.CostoTrabajo = un.Costo FROM ( 
		SELECT SUM( tp.Costo ) AS Costo FROM Taller.Prenda AS pr INNER JOIN Taller.Trabajo AS t ON pr.IdPrenda = t.IdPrenda  
		INNER JOIN Taller.TipoTrabajo AS tp ON t.IdTipoTrabajo = tp.IdTipoTrabajo 
		WHERE pr.IdPrenda = @IdPrenda ) AS un
	 WHERE Taller.Prenda.IdPrenda = @IdPrenda ;
END

--2do Trigger
--Cada vez que se inserta/elimina un registro en "MaterialParaTrabajo", se busca el trabajo 
--correspondiente, la prenda correspondiente al trabajo, y se recalcula el "CostoMaterial" de la prenda.
CREATE TRIGGER Taller.COSTO_EXTRA_PRENDA_TR ON Taller.MaterialParaTrabajo
AFTER INSERT, DELETE
AS
BEGIN
	DECLARE @IdPrenda BIGINT ;

	IF EXISTS ( SELECT * FROM inserted )
		SET @IdPrenda = ( SELECT TOP 1 Taller.Trabajo.IdPrenda FROM inserted INNER JOIN Taller.Trabajo ON inserted.IdTrabajo = Taller.Trabajo.IdTrabajo );
	ELSE
		SET @IdPrenda = ( SELECT TOP 1 Taller.Trabajo.IdPrenda FROM deleted INNER JOIN Taller.Trabajo ON deleted.IdTrabajo = Taller.Trabajo.IdTrabajo );

	UPDATE Taller.Prenda SET Taller.Prenda.CostoMaterial = un.Costo FROM ( 
		SELECT SUM( mt.PrecioCliente ) AS Costo FROM Taller.Prenda AS pr INNER JOIN Taller.Trabajo AS t ON pr.IdPrenda = t.IdPrenda  
		INNER JOIN Taller.MaterialParaTrabajo AS mt ON t.IdTrabajo = mt.IdTrabajo
		WHERE pr.IdPrenda = @IdPrenda ) AS un
	WHERE Taller.Prenda.IdPrenda = @IdPrenda ;
END

--3er Trigger
--Al modificar algun registro de "Prenda" se actualiza la "Confección" correspondiente
--y se recalcula el "CostoTotal"
CREATE TRIGGER Taller.COSTO_TOTAL_CONFECCION_TR ON Taller.Prenda
FOR UPDATE
AS
BEGIN
	DECLARE @IdConfeccion BIGINT ;

	--Se obtiene el Id de la tabla modificada.
	SET @IdConfeccion = ( SELECT TOP 1 IdConfeccion FROM inserted );

	UPDATE Taller.Confeccion SET Taller.Confeccion.CostoTotal = un.Costo FROM ( 
		SELECT SUM( p.CostoTrabajo + p.CostoMaterial ) AS Costo FROM Taller.Prenda AS p INNER JOIN Taller.Confeccion AS c ON p.IdConfeccion = c.IdConfeccion  
		WHERE c.IdConfeccion = @IdConfeccion ) AS un
	WHERE Taller.Confeccion.IdConfeccion = @IdConfeccion ;
END

--4to Trigger
--Cuando se inserte/elimine un registro en "Trabajo" se ajusta la "FechaEntrega" de
--"Confección". Por cada trabajo asociado a la confección se agrega un dia a la "FechaPedido"
--y el resultado de esta operación se convierte en "FechaEntrega"
CREATE TRIGGER Taller.FECHA_ENTREGA_CONFECCION_TR ON Taller.Trabajo
AFTER INSERT, DELETE
AS
BEGIN
	DECLARE @IdConfeccion BIGINT ;
	DECLARE @FechaPedido DATETIME ;

	IF EXISTS ( SELECT * FROM inserted )
	BEGIN
		SET @IdConfeccion = ( SELECT TOP 1 Taller.Prenda.IdConfeccion FROM inserted INNER JOIN Taller.Prenda ON inserted.IdPrenda = Taller.Prenda.IdPrenda );	
	END
	ELSE
	BEGIN
		SET @IdConfeccion = ( SELECT TOP 1 Taller.Prenda.IdConfeccion FROM deleted INNER JOIN Taller.Prenda ON deleted.IdPrenda = Taller.Prenda.IdPrenda );
	END
	
	SET @FechaPedido = ( SELECT TOP 1 FechaPedido FROM Taller.Confeccion WHERE Taller.Confeccion.IdConfeccion = @IdConfeccion );

	UPDATE Taller.Confeccion SET Taller.Confeccion.FechaEntrega = DATEADD( DAY, un.DiasAgregar, @FechaPedido )  FROM (
		SELECT COUNT( * ) AS DiasAgregar FROM Taller.Prenda AS p INNER JOIN Taller.Confeccion AS c ON p.IdConfeccion = c.IdConfeccion
		INNER JOIN Taller.Trabajo AS t ON p.IdPrenda = t.IdPrenda
		WHERE c.IdConfeccion = @IdConfeccion ) AS un
	WHERE Taller.Confeccion.IdConfeccion = @IdConfeccion ;
END

--5to Trigger
--El "Anticipo" de "Confección" es el 50% del "CostoTotal" de la misma tabla.
--Cada vez que se actualize esta tabla, es decir, cada vez que se actualize el
--"CostoTotal", se recalcula el "Anticipo".
CREATE TRIGGER Taller.ANTICIPO_CONFECCION_TR ON Taller.Confeccion
FOR UPDATE
AS
BEGIN
	DECLARE @IdConfeccion BIGINT ;

	--Se obtiene el Id de la tabla modificada.
	SET @IdConfeccion = ( SELECT TOP 1 IdConfeccion FROM inserted );

	UPDATE Taller.Confeccion SET Taller.Confeccion.Anticipo = un.Anticipo FROM ( 
		SELECT  ( c.CostoTotal / 2 ) AS Anticipo FROM Taller.Confeccion AS c 
		WHERE c.IdConfeccion = @IdConfeccion ) AS un
	WHERE Taller.Confeccion.IdConfeccion = @IdConfeccion ;
END

--6to Trigger
--El valor de "Total" de "Compra" se actualiza cada vez que se 
--inserta/elimina un registro de "DetalleCompra". Este "Total" es la suma
--de todos los "Subtotal" de "DetalleCompra".
CREATE TRIGGER Taller.TOTAL_COMPRA_TR ON Taller.DetalleCompra
AFTER INSERT, DELETE
AS
BEGIN
	DECLARE @IdCompra BIGINT ;

	IF EXISTS ( SELECT * FROM inserted )
	BEGIN
		SET @IdCompra = ( SELECT TOP 1 IdCompra FROM inserted );	
	END
	ELSE
	BEGIN
		SET @IdCompra = ( SELECT TOP 1 IdCompra FROM deleted );
	END

	UPDATE Taller.Compra SET Taller.Compra.Total = un.Total  FROM (
		SELECT SUM( d.Subtotal ) AS Total FROM Taller.DetalleCompra AS d INNER JOIN Taller.Compra AS c ON d.IdCompra = c.IdCompra
		WHERE c.IdCompra = @IdCompra ) AS un
	WHERE Taller.Compra.IdCompra = @IdCompra ;
END

--7mo Trigger (NO TERMINADO)
--Cada vez que se inserte un registro en detalle compra, si NO existe el subtotal,
--este se calcula multiplicando el precio unitario por la cantidad. Si no existe el
--precio unitario, este se calcula dividiendo el subtotal por la cantidad. Si no hay
--precio unitario ni cantidad, se marca error (Esto se maneja en la declaración de la tabla
--por medio de un CHECK ).
ALTER TRIGGER Taller.TOTAL_COMPRA_TR ON Taller.DetalleCompra
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @Subtotal FLOAT, @CostoUnitario FLOAT ;
	SELECT @Subtotal = Subtotal, @CostoUnitario =  FROM inserted
	
END

