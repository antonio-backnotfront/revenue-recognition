namespace RevenueRecognition.Models.Software;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Category")]
public class Category
{
    [Key] public int Id { get; set; }
    [MaxLength(255)] public string Name { get; set; }

    public ICollection<Software> Softwares { get; set; }
}