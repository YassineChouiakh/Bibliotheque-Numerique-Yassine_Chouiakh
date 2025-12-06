public class Revue : Item
{
    public int Edition { get; set; }

    public Revue(Guid code, string nom, string createur, int date, int edition) : base(code, nom, createur, date)
    {
        Edition = edition > 0 ? edition : throw new ArgumentException("Edition invalide");
    }

    public override void Afficher()
    {
        Console.WriteLine($"Revue: {Nom} | {Createur} | {Date} | Ed.{Edition}");
    }
}