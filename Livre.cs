public class Ouvrage : Item
{
    public int Pages { get; set; }

    public Ouvrage(Guid code, string nom, string createur, int date, int pages) : base(code, nom, createur, date)
    {
        Pages = pages > 0 ? pages : throw new ArgumentException("Pages invalide");
    }

    public override void Afficher()
    {
        Console.WriteLine($"Ouvrage: {Nom} | {Createur} | {Date} | {Pages}p");
    }
}