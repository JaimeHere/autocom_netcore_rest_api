using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PedidoProducto
{
    [Required]
    [ForeignKey(nameof(Pedido))]
    public required int pedido_id { get; set; }
    public Pedido pedido { get; set; }
   
    [Required]
    [ForeignKey(nameof(Producto))]
    public required int producto_id { get; set; }
    public Producto producto {get; set;}
    public required int cantidad { get; set; }
}