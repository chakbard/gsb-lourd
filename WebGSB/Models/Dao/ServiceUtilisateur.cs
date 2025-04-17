
using System.Data;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Persistance;
using WebGSB.Models.Metier;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Metier;
using WebGSB.Models.Persistance;

public class ServiceUtilisateur
{
    public Utilisateur GetUtilisateur(string login)
    {
        DataTable dt;
        string mysql = "SELECT id_visiteur, nom_visiteur, prenom_visiteur, pwd_visiteur, type_visiteur FROM visiteur WHERE login_visiteur = @login";
        Serreurs er = new Serreurs("Erreur sur recherche d'un utilisateur.", "ServiceUtilisateur.GetUtilisateur");
        try
        {
            var parms = new Dictionary<string, object> { { "@login", login } };
            dt = DBInterface.Lecture(mysql, parms, er);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                return new Utilisateur
                {
                    IdVisiteur = Convert.ToInt32(dataRow["id_visiteur"]),
                    NomVisiteur = dataRow["nom_visiteur"].ToString(),
                    PrenomVisiteur = dataRow["prenom_visiteur"].ToString(),
                    PwdVisiteur = dataRow["pwd_visiteur"].ToString(), // Récupérer le mot de passe haché depuis la base de données
                    TypeVisiteur = dataRow["type_visiteur"].ToString()
                };
            }
            return null;
        }
        catch (Exception exc)
        {
            throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), exc.Message, exc);
        }
    }
}