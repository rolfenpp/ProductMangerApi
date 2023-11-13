using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECSProductWebAPI.Data.Entites;

[Index(nameof(Sku), IsUnique = true)]
public class Product 
{
    // [Required] kan användas
    public int Id {get; set;}

    [MaxLength(50)]
    public string Namn { get; set;}

    [MaxLength(50)]
    public string Beskrivning {get; set;}

    [Column(TypeName = "nchar(6)")]
    public string Sku {get; set;}

    [MaxLength(50)]
    public string ImgUrl {get; set;}

    [MaxLength(50)]
    public int Pris {get; set;}
}