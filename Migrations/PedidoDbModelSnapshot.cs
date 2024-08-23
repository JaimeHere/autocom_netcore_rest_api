﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace autocom_netcore_rest_api.Migrations
{
    [DbContext(typeof(DBConnection))]
    partial class PedidoDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pedido", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("cliente_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("PedidoProducto", b =>
                {
                    b.Property<int>("pedido_id")
                        .HasColumnType("int");

                    b.Property<int>("producto_id")
                        .HasColumnType("int");

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.HasKey("pedido_id", "producto_id");

                    b.HasIndex("producto_id");

                    b.ToTable("PedidosProducto");
                });

            modelBuilder.Entity("Producto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("precio")
                        .HasColumnType("real");

                    b.HasKey("id");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("PedidoProducto", b =>
                {
                    b.HasOne("Pedido", "pedido")
                        .WithMany("PedidoProductos")
                        .HasForeignKey("pedido_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Producto", "producto")
                        .WithMany("PedidoProductos")
                        .HasForeignKey("producto_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("pedido");

                    b.Navigation("producto");
                });

            modelBuilder.Entity("Pedido", b =>
                {
                    b.Navigation("PedidoProductos");
                });

            modelBuilder.Entity("Producto", b =>
                {
                    b.Navigation("PedidoProductos");
                });
#pragma warning restore 612, 618
        }
    }
}
