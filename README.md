# Microservicio .NET Core con MSSQL

Este proyecto es un microservicio desarrollado con .NET Core que se conecta a una base de datos MSSQL. Utiliza Entity Framework Core para manejar las operaciones CRUD sobre tres tablas principales: `Pedidos`, `Productos`, y `PedidosProducto`.

## Características

- **ASP.NET Core**: Plataforma de desarrollo rápida y moderna para construir servicios web.
- **Entity Framework Core**: ORM que facilita las interacciones con la base de datos.
- **MSSQL**: Base de datos relacional utilizada para almacenar los datos.
- **Inyección de Dependencias**: Implementación de servicios siguiendo el patrón de inyección de dependencias.

## Endpoints

### Pedidos

- `GET /pedidos/` - Listar todos los pedidos.
- `GET /pedidos/{id}/` - Obtener un pedido específico por ID.
- `POST /pedidos/` - Crear un nuevo pedido.
- `PUT /pedidos/{id}/` - Actualizar un pedido existente.
- `DELETE /pedidos/{id}/` - Eliminar un pedido.
- `GET /pedidos/{id}/productos/` - Obtener los productos de un pedido específico.
- `POST /pedidos/{id}/productos/` - Manipular los productos del pedido.

### Productos

- `GET /productos/` - Listar todos los producto.
- `GET /productos/{id}/` - Obtener un producto específico por ID.
- `POST /productos/` - Crear un nuevo producto.
- `PUT /productos/{id}/` - Actualizar un producto existente.
- `DELETE /productos/{id}/` - Eliminar un producto.

## Documentación de la API

Toda la documentación de la API, incluyendo ejemplos de solicitudes y respuestas para cada endpoint, está disponible en Postman. Puedes acceder a ella en el siguiente enlace:

[Documentación de la API en Postman](https://documenter.getpostman.com/view/10308727/2sAXjDdvAS#887f1b84-8abc-47af-89f1-8f5ba9e06d7d)

## Requisitos

- .NET Core SDK 8+
- MSSQL Server
- Entity Framework Core

## Instalación

1. Clona este repositorio:
    ```bash
    git clone https://github.com/JaimeHere/autocom_netcore_rest_api.git
    cd autocom_netcore_rest_api
    ```

2. Configura la base de datos en MSSQL Server:
    - Crea una base de datos nueva en tu servidor MSSQL.
    - Actualiza el archivo `appsettings.json` o utiliza variables de entorno para configurar la cadena de conexión.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=tu-servidor;Database=nombre-de-la-bd;User Id=tu-usuario;Password=tu-contraseña;"
      }
    }
    ```

3. Aplica las migraciones para crear las tablas en la base de datos:

    ```bash
    dotnet ef database update
    ```

4. Construye y ejecuta el proyecto:

    ```bash
    dotnet build
    dotnet run
    ```

   El microservicio estará corriendo en `http://localhost:5192`.

## Estructura del Proyecto

```bash
├── classes/
│   └── Pedido.class.cs
│   └── PedidoProducto.class.cs
│   └── Producto.class.cs
├── db_context/
│   └── DbConnection.cs
├── Migrations/
├── appsettings.json
├── autocom_netcore_rest_api.csproj
├── Program.cs
├── autocom_netcore_rest_api.generated.sln
└── README.md
