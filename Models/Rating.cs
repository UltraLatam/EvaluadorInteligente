namespace EvaluadorInteligente.Models;

public class Rating
{
    public int     Id        { get; set; }
    public string  UserId    { get; set; } = string.Empty;
    public int     ProductId { get; set; }
    public float   Label     { get; set; }     // de 1 a 5

    /* navegación opcional */
    public User?    User    { get; set; }
    public Product? Product { get; set; }
}
