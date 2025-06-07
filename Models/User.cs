namespace EvaluadorInteligente.Models;

public class User
{
    public string Id   { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
}
