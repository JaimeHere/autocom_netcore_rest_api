using System.ComponentModel.DataAnnotations;

public class Pedido
{
    [Key]
    public int id { get; set; }
    public DateTime fecha { get; set; }
    public int? cliente_id { get; set; }

    public ICollection<PedidoProducto> PedidoProductos { get; set; }
}