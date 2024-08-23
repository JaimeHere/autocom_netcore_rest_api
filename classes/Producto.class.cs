using System.ComponentModel.DataAnnotations;

public class Producto
{
    [Key]
    public int id { get; set; }
    [Required]
    public required string nombre { get; set; }
    [Required]
    public float? precio { get; set; }

    public ICollection<PedidoProducto> PedidoProductos { get; set; }
}