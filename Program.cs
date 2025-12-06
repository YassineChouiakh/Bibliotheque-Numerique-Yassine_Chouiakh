using System;

class Application
{
    static void Main()
    {
        Collection collection = new Collection();
        bool actif = true;

        while (actif)
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1-Ajouter 2-Lister 3-Chercher 4-Supprimer 5-Sauver 6-Charger 7-Quitter");
            Console.Write("> ");

            try
            {
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": Ajouter(collection); break;
                    case "2": collection.Lister(); break;
                    case "3": Chercher(collection); break;
                    case "4": Supprimer(collection); break;
                    case "5": Sauver(collection); break;
                    case "6": Charger(collection); break;
                    case "7": actif = false; Console.WriteLine("Bye!"); break;
                    default: Console.WriteLine("Invalide!"); break;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Erreur: {ex.Message}"); }
        }
    }

    static void Ajouter(Collection col)
    {
        Console.Write("Type (1=Ouvrage, 2=Revue, 3=PDF): ");
        if (!int.TryParse(Console.ReadLine(), out int type)) return;

        Console.Write("Nom: ");
        string nom = Console.ReadLine() ?? "";
        Console.Write("Createur: ");
        string createur = Console.ReadLine() ?? "";
        Console.Write("Date: ");
        if (!int.TryParse(Console.ReadLine(), out int date)) return;

        Guid code = Guid.NewGuid();

        try
        {
            if (type == 1)
            {
                Console.Write("Pages: ");
                if (int.TryParse(Console.ReadLine(), out int pages))
                    col.Ajouter(new Ouvrage(code, nom, createur, date, pages));
            }
            else if (type == 2)
            {
                Console.Write("Edition: ");
                if (int.TryParse(Console.ReadLine(), out int edition))
                    col.Ajouter(new Revue(code, nom, createur, date, edition));
            }
            else if (type == 3)
            {
                Console.Write("Taille (Mo): ");
                if (double.TryParse(Console.ReadLine(), out double taille))
                    col.Ajouter(new FichierPDF(code, nom, createur, date, taille));
            }
        }
        catch (Exception ex) { Console.WriteLine($"Erreur: {ex.Message}"); }
    }

    static void Chercher(Collection col)
    {
        Console.Write("Mot: ");
        string mot = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(mot)) return;

        try
        {
            var res = col.Chercher(mot);
            Console.WriteLine($"\n{res.Count} resultat(s):");
            for (int i = 0; i < res.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                res[i].Afficher();
            }
        }
        catch (ItemIntrouvableException ex) { Console.WriteLine(ex.Message); }
    }

    static void Supprimer(Collection col)
    {
        if (col.Items.Count == 0) { Console.WriteLine("Vide!"); return; }

        Console.WriteLine("Items:");
        for (int i = 0; i < col.Items.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            col.Items[i].Afficher();
            Console.WriteLine($"   Code: {col.Items[i].Code}");
        }

        Console.Write("\nCode a supprimer: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid code)) { Console.WriteLine("Code invalide!"); return; }

        try { col.Supprimer(code); }
        catch (ItemIntrouvableException ex) { Console.WriteLine(ex.Message); }
    }

    static void Sauver(Collection col)
    {
        Console.Write("Fichier: ");
        string fichier = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(fichier)) return;
        try { col.Sauvegarder(fichier); }
        catch (Exception ex) { Console.WriteLine($"Erreur: {ex.Message}"); }
    }

    static void Charger(Collection col)
    {
        Console.Write("Fichier: ");
        string fichier = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(fichier)) return;
        try { col.Charger(fichier); }
        catch (Exception ex) { Console.WriteLine($"Erreur: {ex.Message}"); }
    }
}
