namespace ConsoleApp;

public class ConsoleUi
{
    private List<Scientist> Authors { get; set; }
    private List<ScientificArticle> Articles { get; set; }
    private List<ScienceJournal> Journals { get; set; }

    private void Seed()
    {
        Authors = new List<Scientist>();
        Journals = new List<ScienceJournal>();
        Articles = new List<ScientificArticle>();

        // Create Authors
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Authors.Add(walterLewin);
        
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Authors.Add(janVanParadijs);
        
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Authors.Add(holgerPedersen);
        
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Authors.Add(paulJoss);

        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Authors.Add(charlesWarwick);

        Scientist anelaChoy = new Scientist("Anela Choy", "Ocenaography", "UC San Diego");
        Authors.Add(anelaChoy);
        
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));
        Authors.Add(robSherlock);


        // Create Journals
        ScienceJournal journalNature = new ScienceJournal("Nature", 3.87);
        Journals.Add(journalNature);

        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);
        Journals.Add(journalScAdvances);

        // Create Articles
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53",
                new List<Scientist>(){ walterLewin, janVanParadijs, holgerPedersen},
                new DateOnly(1981, 12, 31), 3,
                ArticleCategory.Astrophysics, journalNature);
        Articles.Add(articleOrbitalPeriodXRayBurster);

        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources", new List<Scientist>() { walterLewin, paulJoss },
                new DateOnly(1977, 11, 17), 6, ArticleCategory.Astrophysics, journalNature);
        Articles.Add(articleXRayBurstSources);
        
        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch", new List<Scientist>() { charlesWarwick },
                new DateOnly(2024, 09, 25), 18, ArticleCategory.Neuroscience, journalScAdvances);
        Articles.Add(articleKappa);

        ScientificArticle articleLarvaceans =
            new ScientificArticle(
                "From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea",
                new List<Scientist>() { robSherlock, anelaChoy }, new DateOnly(2017, 8, 16), 5,
                ArticleCategory.MarineEcology, journalScAdvances);
        Articles.Add(articleLarvaceans);
        
    }
    
    public void Run()
    {
        Seed();

        Console.WriteLine(String.Join("\n", Journals[0].Articles));
        
    }
}