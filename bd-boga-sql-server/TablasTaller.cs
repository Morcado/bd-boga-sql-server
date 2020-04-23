using System.Data;

namespace bd_boga_sql_server
{
    public class TipoTrabajo : Tabla
    {
        public TipoTrabajo() : base( "TipoTrabajo" )
        {
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
            string[] columnas = new string[] {
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
