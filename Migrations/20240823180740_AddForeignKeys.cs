using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace autocom_netcore_rest_api.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosProducto",
                table: "PedidosProducto",
                columns: new[] { "pedido_id", "producto_id" });

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProducto_producto_id",
                table: "PedidosProducto",
                column: "producto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProducto_Pedidos_pedido_id",
                table: "PedidosProducto",
                column: "pedido_id",
                principalTable: "Pedidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProducto_Productos_producto_id",
                table: "PedidosProducto",
                column: "producto_id",
                principalTable: "Productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProducto_Pedidos_pedido_id",
                table: "PedidosProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProducto_Productos_producto_id",
                table: "PedidosProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosProducto",
                table: "PedidosProducto");

            migrationBuilder.DropIndex(
                name: "IX_PedidosProducto_producto_id",
                table: "PedidosProducto");
        }
    }
}
