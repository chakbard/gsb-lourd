using WebGSB.Models.Persistance;
using System.Collections.Generic;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Metier;
using System;
using System.Data;

namespace WebGSB.Models.Dao
{
    public static class ServicePraticien
    {
        public static List<Praticien> GetTousLesPraticiens()
        {
            Serreurs er = new Serreurs("Erreur sur lecture des Praticien.", "Praticien.GetPraticiens()");
            try
            {
                string mysql = "SELECT id_praticien AS Id, nom_praticien AS Nom, prenom_praticien AS Prenom, cp_praticien AS CodePostal, ville_praticien AS Ville, adresse_praticien AS Adresse FROM Praticien";


                DataTable dataTable = DBInterface.Lecture(mysql, new Dictionary<string, object>(), er);

                List<Praticien> mesPraticiens = new List<Praticien>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Praticien praticien = new Praticien
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nom = Convert.ToString(row["Nom"]),
                        Prenom = Convert.ToString(row["Prenom"]),
                        CodePostal = Convert.ToString(row["CodePostal"]),
                        Ville = Convert.ToString(row["Ville"]),
                        Adresse = Convert.ToString(row["Adresse"])
                    };

                    mesPraticiens.Add(praticien);
                }

                return mesPraticiens;
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }


        public static List<Praticien> GetPraticiensParNom(string nom)
        {
            Serreurs er = new Serreurs("Erreur sur la recherche des praticiens par nom.", "ServicePraticien.GetPraticiensParNom()");
            try
            {
                string mysql = "SELECT id_praticien AS Id, nom_praticien AS Nom, prenom_praticien AS Prenom, cp_praticien AS CodePostal, ville_praticien AS Ville, adresse_praticien AS Adresse FROM Praticien WHERE nom_praticien LIKE @nom";
                Dictionary<string, object> parametres = new Dictionary<string, object>();
                parametres.Add("@nom", "%" + nom + "%");
                DataTable dataTable = DBInterface.Lecture(mysql, parametres, er);

                List<Praticien> mesPraticiens = new List<Praticien>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Praticien praticien = new Praticien
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nom = Convert.ToString(row["Nom"]),
                        Prenom = Convert.ToString(row["Prenom"]),
                        CodePostal = Convert.ToString(row["CodePostal"]),
                        Ville = Convert.ToString(row["Ville"]),
                        Adresse = Convert.ToString(row["Adresse"])
                    };

                    mesPraticiens.Add(praticien);
                }

                return mesPraticiens;
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static Praticien GetPraticienById(int praticienId)
        {
            Serreurs er = new Serreurs("Erreur sur récupération du praticien.", "ServicePraticien.GetPraticienById()");
            try
            {
                string mysql = "SELECT nom_praticien AS Nom, prenom_praticien AS Prenom, cp_praticien AS CodePostal, ville_praticien AS Ville, adresse_praticien AS Adresse FROM Praticien WHERE id_praticien = @praticienId";
                DataTable dataTable = DBInterface.Lecture(mysql, new Dictionary<string, object> { { "@praticienId", praticienId } }, er);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    Praticien praticien = new Praticien
                    {
                        Nom = Convert.ToString(row["Nom"]),
                        Prenom = Convert.ToString(row["Prenom"]),
                        CodePostal = Convert.ToString(row["CodePostal"]),
                        Ville = Convert.ToString(row["Ville"]),
                        Adresse = Convert.ToString(row["Adresse"])
                    };

                    return praticien;
                }
                else
                {
                    // Praticien non trouvé, retourner null ou gérer selon votre logique
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }
    }
}
