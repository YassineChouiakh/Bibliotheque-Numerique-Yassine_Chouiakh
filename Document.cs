public abstract class Item
{
    public Guid Code { get; set; }
    public string Nom { get; set; }
    public string Createur { get; set; }
    public int Date { get; set; }

    public Item(Guid code, string nom, string createur, int date)
    {
        Code = code;
        Nom = nom ?? throw new ArgumentException("Nom requis");
        Createur = createur ?? throw new ArgumentException("Créateur requis");
        Date = (date >= 1000 && date <= 9999) ? date : throw new ArgumentException("Date invalide");
    }

    public abstract void Afficher();
}