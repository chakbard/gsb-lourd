namespace WebGSB.Models.Metier
{
    public class Utilisateur
    {
        // Private fields
        private int idVisiteur;
        private string nomVisiteur;
        private string prenomVisiteur;
        private string pwdVisiteur;
        private string typeVisiteur;

        // Public properties
        public int IdVisiteur
        {
            get => idVisiteur;
            set => idVisiteur = value;
        }
        public string NomVisiteur
        {
            get => nomVisiteur;
            set => nomVisiteur = value;
        }
        public string PrenomVisiteur
        {
            get => prenomVisiteur;
            set => prenomVisiteur = value;
        }
        public string PwdVisiteur
        {
            get => pwdVisiteur;
            set => pwdVisiteur = value;
        }
        public string TypeVisiteur
        {
            get => typeVisiteur;
            set => typeVisiteur = value;
        }

        // Default constructor
        public Utilisateur()
        { }

        // Constructor with parameters
        public Utilisateur(int id, string nom, string prenom, string pwd, string type)
        {
            idVisiteur = id;
            nomVisiteur = nom;
            prenomVisiteur = prenom;
            pwdVisiteur = pwd;
            typeVisiteur = type;
        }
    }
    

}