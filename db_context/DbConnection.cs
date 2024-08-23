using Microsoft.EntityFrameworkCore;
class DBConnection : DbContext
{

    public DBConnection(DbContextOptions<DBConnection> options) : base(options) { }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<PedidoProducto> PedidosProducto => Set<PedidoProducto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PedidoProducto>()
            .HasKey(pp => new { pp.pedido_id, pp.producto_id });

        modelBuilder.Entity<PedidoProducto>()
            .HasOne(pp => pp.pedido)
            .WithMany(p => p.PedidoProductos)
            .HasForeignKey(pp => pp.pedido_id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PedidoProducto>()
            .HasOne(pp => pp.producto)
            .WithMany(p => p.PedidoProductos)
            .HasForeignKey(pp => pp.producto_id)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

}