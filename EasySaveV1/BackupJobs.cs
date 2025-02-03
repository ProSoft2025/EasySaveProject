namespace EasySave
{
    public class BackupJobs
    {
        // Déclaration des variables (Attributs)
        public string Directory_src { get; set; }
        public string Directory_dst { get; set; }
        public string name { get; set; }
        public enum BackupType
        {
            Complete,
            Differentielle
        }
        public BackupType Type { get; set; }

        // Déclration du constructeur et affectation des variables
        public BackupJobs(string name, string Directory_src, string Directory_dst, BackupType Type)
        {
            this.Directory_src = Directory_src;
            this.Directory_dst = Directory_dst;
            this.name = name;
            this.Type = Type;
        }

        public void AfficherAttributs()
        {   
            Console.WriteLine("Nom du BackupJob :" + " " + this.name);
            Console.WriteLine("Répertoire source :" + " " + this.Directory_src);
            Console.WriteLine("Répertoire distant :" + " " + this.Directory_dst);
            Console.WriteLine("Type de sauvegarde :" + " " + this.Type);
        }

        public void execute()
        {
            // Utiliser Copy file manager
        }
    }
}
