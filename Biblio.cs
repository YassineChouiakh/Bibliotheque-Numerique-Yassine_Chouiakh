using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class Collection
{
    private List<Item> items = new List<Item>();

    public void Ajouter(Item item)
    {
        if (item == null) throw new ArgumentNullException("Item null");
        foreach (Item i in items)
            if (i.Code == item.Code) { Console.WriteLine("Item existe d\u00e9j\u00e0"); return; }
        items.Add(item);
        Console.WriteLine($"Ajout\u00e9: {item.Nom}");
    }

    public void Supprimer(Guid code)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Code == code)
            {
                Console.WriteLine($"Supprim\u00e9: {items[i].Nom}");
                items.RemoveAt(i);
                return;
            }
        }
        throw new ItemIntrouvableException($"Item {code} introuvable");
    }

    public List<Item> Chercher(string mot)
    {
        if (string.IsNullOrWhiteSpace(mot)) throw new ArgumentException("Mot vide");
        List<Item> res = new List<Item>();
        string m = mot.ToLower();
        foreach (Item i in items)
            if (i.Nom.ToLower().Contains(m) || i.Createur.ToLower().Contains(m)) res.Add(i);
        if (res.Count == 0) throw new ItemIntrouvableException($"'{mot}' introuvable");
        return res;
    }

    public void Lister()
    {
        if (items.Count == 0) { Console.WriteLine("Collection vide"); return; }
        Console.WriteLine($"\n--- {items.Count} item(s) ---");
        for (int i = 0; i < items.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            items[i].Afficher();
        }
    }

    public void Sauvegarder(string fichier)
    {
        if (string.IsNullOrWhiteSpace(fichier)) throw new ArgumentException("Fichier vide");
        try
        {
            string dir = Path.GetDirectoryName(fichier);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (FileStream fs = new FileStream(fichier, FileMode.Create, FileAccess.Write))
            using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
            {
                foreach (Item item in items)
                {
                    if (item is Ouvrage o)
                        w.WriteLine($"Ouvrage;{item.Code};{item.Nom};{item.Createur};{item.Date};{o.Pages}");
                    else if (item is Revue r)
                        w.WriteLine($"Revue;{item.Code};{item.Nom};{item.Createur};{item.Date};{r.Edition}");
                    else if (item is FichierPDF p)
                        w.WriteLine($"FichierPDF;{item.Code};{item.Nom};{item.Createur};{item.Date};{p.Taille.ToString(CultureInfo.InvariantCulture)}");
                }
            }
            Console.WriteLine($"Sauvegarde: {items.Count} items dans '{fichier}'");
        }
        catch (Exception ex) { Console.WriteLine($"Erreur sauvegarde: {ex.Message}"); throw; }
    }

    public void Charger(string fichier)
    {
        if (string.IsNullOrWhiteSpace(fichier)) throw new ArgumentException("Fichier vide");
        if (!File.Exists(fichier)) throw new FileNotFoundException($"'{fichier}' introuvable");

        try
        {
            items.Clear();
            int ligne = 0;

            using (FileStream fs = new FileStream(fichier, FileMode.Open, FileAccess.Read))
            using (StreamReader r = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                string l;
                while ((l = r.ReadLine()) != null)
                {
                    ligne++;
                    if (string.IsNullOrWhiteSpace(l)) continue;

                    try
                    {
                        string[] p = l.Split(';');
                        if (p.Length < 6) continue;

                        string type = p[0].Trim();
                        Guid code = Guid.Parse(p[1].Trim());
                        string nom = p[2].Trim();
                        string createur = p[3].Trim();
                        int date = int.Parse(p[4].Trim());

                        switch (type)
                        {
                            case "Ouvrage":
                                items.Add(new Ouvrage(code, nom, createur, date, int.Parse(p[5].Trim())));
                                break;
                            case "Revue":
                                items.Add(new Revue(code, nom, createur, date, int.Parse(p[5].Trim())));
                                break;
                            case "FichierPDF":
                                items.Add(new FichierPDF(code, nom, createur, date, double.Parse(p[5].Trim(), CultureInfo.InvariantCulture)));
                                break;
                            default:
                                Console.WriteLine($"Ligne {ligne}: Type '{type}' inconnu");
                                break;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine($"Ligne {ligne}: {ex.Message}"); }
                }
            }
            Console.WriteLine($"Chargement: {items.Count} items depuis '{fichier}'");
        }
        catch (Exception ex) { Console.WriteLine($"Erreur chargement: {ex.Message}"); throw; }
    }

    public List<Item> Items => items;
}
