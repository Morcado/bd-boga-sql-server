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
            PK = false;
            List<string> columnas = new List<string> {
                "IdTrabajo",
                "IdMaterial",
                "Cantidad",
                "PrecioCliente"
            };

            InitializeQuerys( columnas );
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
        }
    }
}
