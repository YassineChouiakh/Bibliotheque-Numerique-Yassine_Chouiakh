public class FichierPDF : Item
{
    public double Taille { get; set; }

    public FichierPDF(Guid code, string nom, string createur, int date, double taille) : base(code, nom, createur, date)
    {
        Taille = taille > 0 ? taille : throw new ArgumentException("Taille invalide");
    }

    public override void Afficher()
    {
        Console.WriteLine($"PDF: {Nom} | {Createur} | {Date} | {Taille}Mo");
    }
}