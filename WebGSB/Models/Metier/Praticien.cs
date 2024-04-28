namespace WebGSB.Models.Metier
{
    public class Praticien
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public double CoefficientPrescription { get; set; }
        public string Diplome { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Adresse { get; set; }

        private int id_praticien;
        // Constructeur sans paramètre
        public Praticien() { }

        // Constructeur avec paramètres pour initialiser l'objet
        


        // Vous pouvez ajouter d'autres méthodes et logiques métier liées à un praticien ici
    }
}
