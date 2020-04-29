using System.Collections.Generic;
using System.Data;

namespace bd_boga_sql_server
{
    public class TipoTrabajo : Tabla
    {
        public TipoTrabajo() : base( "TipoTrabajo" )
        {
            List<string> columnas = new List<string> {
                "IdTipoTrabajo",
                "Costo",
                "NombreConfeccion"
            };

            InitializeQuerys( columnas );
        }
    }

    public class Cliente : Tabla
    {
        public Cliente() : base( "Cliente" )
        {
            List<string> columnas = new List<string> {
                "IdCliente",
                "Nombre",
                "ApellidoPaterno",
                "ApellidoMaterno",
                "Telefono"
            };

            InitializeQuerys( columnas );
        }
    }

    public class Empleado : Tabla
    {
        public Empleado() : base( "Empleado" )
        {
            List<string> columnas = new List<string> {
                "IdEmpleado",
                "Nombre",
                "ApellidoPaterno",
                "ApellidoMaterno",
                "Telefono",
                "Direccion"
            };

            InitializeQuerys( columnas );
        }
    }

    public class Confeccion : Tabla
    {
        public Confeccion() : base( "Confeccion" )
        {
            List<string> columnas = new List<string> {
                "IdConfeccion",
                "CostoTotal",
                "FechaPedido",
                "FechaEntrega",
                "Anticipo",
                "IdCliente"
            };

            InitializeQuerys( columnas );
        }
    }

    public class Prenda : Tabla
    {
        public Prenda() : base( "Prenda" )
        {
            List<string> columnas = new List<string> {
                "IdPrenda",
                "IdConfeccion",
                "CostoTrabajo",
                "CostoMaterial",
                "Finalizado"
            };

            InitializeQuerys( columnas );

            SelectQuery = @"SELECT p.IdPrenda, p.IdConfeccion, ( cl.Nombre + ' ' + cl.ApellidoPaterno + ' ' + cl.ApellidoMaterno + ' ' + CONVERT( varchar, c.FechaPedido, 1 ) ) AS InfoConfeccion,
                            CostoTrabajo, CostoMaterial, Finalizado
                            FROM Taller.Prenda AS p 
                            INNER JOIN Taller.Confeccion AS c ON p.IdConfeccion = c.IdConfeccion
                            INNER JOIN Taller.Cliente AS cl ON c.IdCliente = cl.IdCliente" ;

            AdditionalInfoQuery = @"SELECT c.IdConfeccion, ( cl.Nombre + ' ' + cl.ApellidoPaterno + ' ' + cl.ApellidoMaterno + ' ' + CONVERT( varchar, c.FechaPedido, 1 ) ) AS InfoConfeccion
                FROM Taller.Confeccion AS c INNER JOIN Taller.Cliente AS cl ON c.IdCliente = cl.IdCliente" ;

            AdditionalInfoCols = new List<int>();
            AdditionalInfoCols.Add( 0 );
        }
    }

    public class Trabajo : Tabla
    {
        public Trabajo() : base( "Trabajo" )
        {
            List<string> columnas = new List<string> {
                "IdTrabajo",
                "IdTipoTrabajo",
                "IdPrenda",
                "IdEmpleado"
            };

            InitializeQuerys( columnas );

            SelectQuery = @"SELECT IdTrabajo, t.IdTipoTrabajo, NombreConfeccion AS DescTrabajo, IdPrenda, IdEmpleado FROM Taller.Trabajo AS t 
INNER JOIN Taller.TipoTrabajo as tp ON t.IdTipoTrabajo = tp.IdTipoTrabajo";

            AdditionalInfoQuery = @"SELECT IdTipoTrabajo, NombreConfeccion FROM Taller.TipoTrabajo" ;
            AdditionalInfoCols = new List<int>();
            AdditionalInfoCols.Add( 0 );
        }
    }

    public class Material : Tabla
    {
        public Material() : base( "Material" )
        {
            List<string> columnas = new List<string> {
                "IdMaterial",
                "Descripcion"
            };

            InitializeQuerys( columnas );
        }
    }

    public class MaterialParaTrabajo : Tabla
    {
        public MaterialParaTrabajo() : base( "MaterialParaTrabajo" )
        {
            PK = true ;
            List<string> columnas = new List<string> {
                "IdMaterialParaTrabajo",
                "IdTrabajo",
                "IdMaterial",
                "Cantidad",
                "PrecioCliente"
            };

            InitializeQuerys( columnas );

            SelectQuery = @"SELECT IdMaterialParaTrabajo, IdTrabajo, mt.IdMaterial, m.Descripcion, Cantidad, PrecioCliente FROM Taller.MaterialParaTrabajo AS mt
INNER JOIN Taller.Material AS m on mt.IdMaterial = m.IdMaterial";

            AdditionalInfoQuery = @"SELECT IdMaterial, Descripcion FROM Taller.Material";
            AdditionalInfoCols = new List<int>();
            AdditionalInfoCols.Add( 1 );
        }
    }

    public class Proveedor : Tabla
    {
        public Proveedor() : base( "Proveedor" )
        {
            List<string> columnas = new List<string> {
                "IdProveedor",
                "Nombre",
                "Telefono",
                "Direccion"
            };

            InitializeQuerys( columnas );
        }
    }

    public class Compra : Tabla
    {
        public Compra() : base( "Compra" )
        {
            List<string> columnas = new List<string> {
                "IdCompra",
                "IdProveedor",
                "FechaCompra",
                "Total"
            };

            InitializeQuerys( columnas );

            SelectQuery = @"SELECT IdCompra, c.IdProveedor, p.Nombre, FechaCompra, 
            Total FROM Taller.Compra AS c INNER JOIN Taller.Proveedor AS p ON c.IdProveedor = p.IdProveedor" ;

            AdditionalInfoQuery = @"SELECT IdProveedor, Nombre FROM Taller.Proveedor";
            AdditionalInfoCols = new List<int>();
            AdditionalInfoCols.Add( 0 );
        }
    }

    public class DetalleCompra : Tabla
    {
        public DetalleCompra() : base( "DetalleCompra" )
        {
            List<string> columnas = new List<string> {
                "IdDetalleCompra",
                "IdCompra",
                "IdMaterial",
                "CostoUnitario",
                "Cantidad",
                "Subtotal"
            };

            InitializeQuerys( columnas );

            SelectQuery = @"SELECT IdDetalleCompra, IdCompra, d.IdMaterial, m.Descripcion, CostoUnitario, Cantidad, Subtotal FROM Taller.DetalleCompra AS d 
INNER JOIN Taller.Material AS m ON d.IdMaterial = m.IdMaterial" ;

            AdditionalInfoQuery = @"SELECT IdMaterial, Descripcion FROM Taller.Material";
            AdditionalInfoCols = new List<int>();
            AdditionalInfoCols.Add( 1 );
        }
    }
}
