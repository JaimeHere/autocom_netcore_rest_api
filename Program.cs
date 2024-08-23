using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBConnection>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow} Started]");
//     await next(context);
//     Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow} Finished]");
// });

app.MapGet("/pedidos", async (DBConnection db) => await db.Pedidos.ToListAsync());
app.MapPost("/pedidos", async (Pedido pedido, DBConnection db) =>
{
    pedido.fecha = DateTime.Now;
    db.Pedidos.Add(pedido);
    await db.SaveChangesAsync();
    return TypedResults.Created("/pedidos/{id}", pedido);
})
.AddEndpointFilter(async (context, next) =>
{
    var pedidoArgument = context.GetArgument<Pedido>(0);
    var errors = new Dictionary<string, string[]>();
    if (pedidoArgument.cliente_id == null)
    {
        errors.Add(nameof(Pedido.cliente_id), ["Es un campo obligatorio"]);
    }
    if (pedidoArgument.cliente_id <= 0)
    {
        errors.Add(nameof(Pedido.cliente_id), ["Debe ser mayor a 0"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});

app.MapGet("/pedidos/{id}", Results<Ok<Pedido>, NotFound> (int id, DBConnection db) =>
{
    var targetPedido = db.Pedidos.Find(id);
    return targetPedido is null
    ? TypedResults.NotFound()
    : TypedResults.Ok(targetPedido);
});

app.MapGet("/pedidos/{id}/productos", (int id, DBConnection db) =>
{
    var targetPedido = db.Pedidos.Find(id);
    if (targetPedido is null) return Results.NotFound();

    var productos = db.PedidosProducto.Where(pp => pp.pedido_id == id)
        .Select(pp => pp.producto)
        .ToList();
    return Results.Ok(productos);
});

app.MapPost("/pedidos/{id}/productos", async (int id, [FromBody]PedidoProducto payload, DBConnection db) =>
{
    var targetPedido = db.Pedidos.Find(id);
    if (targetPedido is null) return Results.NotFound();

    var productToAssign = db.Productos.Find(payload.producto_id);
    if (productToAssign is null) return Results.NotFound();

    var checkProductInOrder = db.PedidosProducto.Where(pp => pp.pedido_id == id && pp.producto_id == payload.producto_id)
    .ToList();
    if (checkProductInOrder.Count() > 0)
    {
        // Means it exist.
        var pedido_producto = checkProductInOrder.FirstOrDefault();
        if (payload.cantidad < 0)
        {
            // reduce product
            pedido_producto.cantidad += payload.cantidad;
            if (pedido_producto.cantidad < 0)
            {
                return Results.BadRequest("No hay suficientes productos en el pedido.");
            }
            else if (pedido_producto.cantidad == 0)
            {
                db.PedidosProducto.Remove(pedido_producto);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
        }
        else
        {
            // add product
            pedido_producto.cantidad += payload.cantidad;
        }
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    else
    {
        if (payload.cantidad < 0)
        {
            return Results.BadRequest("No se puede agregar una cantidad negativa a un producto nuevo.");
        }
        // payload.pedido_id = id;
        db.PedidosProducto.Add(payload);
        await db.SaveChangesAsync();
        return Results.Ok(payload);
    }
})
.AddEndpointFilter(async (context, next) =>
{
    Console.WriteLine("Before Get Argument");
    Console.WriteLine(context.Arguments.ToString());
    var pedidoProductoArgument = context.GetArgument<PedidoProducto>(1);
    var errors = new Dictionary<string, string[]>();
    if (pedidoProductoArgument.producto_id == null)
    {
        errors.Add(nameof(PedidoProducto.producto_id), ["Es un campo obligatorio"]);
    }
    if (pedidoProductoArgument.producto_id <= 0)
    {
        errors.Add(nameof(PedidoProducto.producto_id), ["Debe ser mayor a 0"]);
    }
    if (pedidoProductoArgument.cantidad == 0)
    {
        errors.Add(nameof(PedidoProducto.cantidad), ["Debe ser diferente a 0 (use negativos para reducir)"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});

app.MapPut("/pedidos/{id}", async (int id, Pedido pedido_payload, DBConnection db) =>
{
    var pedido = await db.Pedidos.FindAsync(id);

    if (pedido is null) return Results.NotFound();

    pedido.cliente_id = pedido_payload.cliente_id;
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.AddEndpointFilter(async (context, next) =>
{
    var pedidoArgument = context.GetArgument<Pedido>(1);
    var errors = new Dictionary<string, string[]>();
    if (pedidoArgument.cliente_id == null)
    {
        errors.Add(nameof(Pedido.cliente_id), ["Es un campo obligatorio"]);
    }
    if (pedidoArgument.cliente_id <= 0)
    {
        errors.Add(nameof(Pedido.cliente_id), ["Debe ser mayor a 0"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});
app.MapDelete("/pedidos/{id}", async (int id, DBConnection db) =>
{
    bool hasRelatedProductos = await db.PedidosProducto
                                                 .AnyAsync(pp => pp.pedido_id == id);
    if (hasRelatedProductos)
    {
        var errors = new Dictionary<string, string[]>();
        errors.Add("detail", ["No se puede eliminar un pedido que tiene productos relacionados"]);
        return Results.ValidationProblem(errors);
    }
    var pedido = await db.Pedidos.FindAsync(id);
    if (pedido is null) return Results.NotFound();

    db.Pedidos.Remove(pedido);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
});


app.MapGet("/productos", async (DBConnection db) => await db.Productos.ToListAsync());
app.MapPost("/productos", async (Producto producto, DBConnection db) =>
{
    db.Productos.Add(producto);
    await db.SaveChangesAsync();
    return TypedResults.Created("/productos/{id}", producto);
})
.AddEndpointFilter(async (context, next) =>
{
    var productoArgument = context.GetArgument<Producto>(0);
    var errors = new Dictionary<string, string[]>();
    if (productoArgument.nombre == null)
    {
        errors.Add(nameof(Producto.nombre), ["Es un campo obligatorio"]);
    }
    else if (productoArgument.nombre.Length > 100)
    {
        errors.Add(nameof(Producto.nombre), ["No debe exceder los 100 caracteres"]);
    }
    if (productoArgument.precio == null)
    {
        errors.Add(nameof(Producto.precio), ["Es un campo obligatorio"]);
    }
    if (productoArgument.precio < 0)
    {
        errors.Add(nameof(Producto.precio), ["Debe ser mayor o igual 0 (producto gratis)"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});

app.MapGet("/productos/{id}", Results<Ok<Producto>, NotFound> (int id, DBConnection db) =>
{
    var targetProducto = db.Productos.Find(id);
    return targetProducto is null
    ? TypedResults.NotFound()
    : TypedResults.Ok(targetProducto);
});

app.MapPut("/productos/{id}", async (int id, Producto producto_payload, DBConnection db) =>
{
    var pedido = await db.Productos.FindAsync(id);

    if (pedido is null) return Results.NotFound();

    pedido.nombre = producto_payload.nombre;
    pedido.precio = producto_payload.precio;
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.AddEndpointFilter(async (context, next) =>
{
    var productoArgument = context.GetArgument<Producto>(1);
    var errors = new Dictionary<string, string[]>();
    if (productoArgument.nombre == null)
    {
        errors.Add(nameof(Producto.nombre), ["Es un campo obligatorio"]);
    }
    else if (productoArgument.nombre.Length > 100)
    {
        errors.Add(nameof(Producto.nombre), ["No debe exceder los 100 caracteres"]);
    }
    if (productoArgument.precio == null)
    {
        errors.Add(nameof(Producto.precio), ["Es un campo obligatorio"]);
    }
    if (productoArgument.precio < 0)
    {
        errors.Add(nameof(Producto.precio), ["Debe ser mayor o igual 0 (producto gratis)"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});
app.MapDelete("/productos/{id}", async (int id, DBConnection db) =>
{
    var producto = await db.Productos.FindAsync(id);
    if (producto is null) return Results.NotFound();

    db.Productos.Remove(producto);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
});

app.Run();
