using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RevenueRecognition.Application.Enums;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateClientRequest
{
    [Required]
    public string Type { get; set; }
    
}