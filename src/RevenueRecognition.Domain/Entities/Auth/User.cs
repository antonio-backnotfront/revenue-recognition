namespace RevenueRecognition.Domain.Entities.Auth;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}