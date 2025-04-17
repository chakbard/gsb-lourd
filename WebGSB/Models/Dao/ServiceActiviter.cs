using System.Data;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Persistance;
using System.Collections.Generic;
using WebGSB.Models.Metier;
using System;
using MySql.Data.MySqlClient;
using WebGSB.Models.Utilitaires;

namespace WebGSB.Models.Dao
{

    public static class ServiceActiviter
    {
        public static List<Invitation> GetInvitationsParPraticienId(int praticienId)
        {
            Serreurs er = new Serreurs("Erreur sur la récupération des invitations.", "ServiceInviter.GetInvitationsParPraticienId()");
            try
            {
                //string mysql = "SELECT ac.id_activite_compl, ac.date_activite, ac.lieu_activite, ac.theme_activite, ac.motif_activite\r\nFROM PRATICIEN p\r\nJOIN INVITER i ON p.id_praticien = i.id_praticien\r\nJOIN ACTIVITE_COMPL ac ON i.id_activite_compl = ac.id_activite_compl\r\nWHERE p.id_praticien = 1";
                string mysql = "SELECT ac.id_activite_compl, ac.date_activite, ac.lieu_activite, ac.theme_activite, ac.motif_activite \r\nFROM PRATICIEN p \r\nJOIN INVITER i ON p.id_praticien = i.id_praticien \r\nJOIN ACTIVITE_COMPL ac ON i.id_activite_compl = ac.id_activite_compl\r\nWHERE p.id_praticien = @idPraticien;\r\n";
                //string mysql = "SELECT ac.id_activite_compl, ac.date_activite, ac.lieu_activite, ac.theme_activite, ac.motif_activite FROM INVITER i JOIN ACTIVITE_COMPL ac ON i.id_activite_compl = ac.id_activite_compl WHERE i.id_praticien = @idPraticien";
                //string mysql = "SELECT p.id_praticien, ac.id_activite_compl, ac.date_activite, ac.lieu_activite, ac.theme_activite, ac.motif_activite \r\nFROM PRATICIEN p \r\nJOIN INVITER i ON p.id_praticien = i.id_praticien \r\nJOIN ACTIVITE_COMPL ac ON i.id_activite_compl = ac.id_activite_compl;\r\n";
                Dictionary<string, object> parametres = new Dictionary<string, object>();
                parametres.Add("@idPraticien", praticienId);
                DataTable dataTable = DBInterface.Lecture(mysql, parametres, er);

                List<Invitation> invitations = new List<Invitation>();



                foreach (DataRow row in dataTable.Rows)
                {
                    Invitation invitation = new Invitation
                    {
                        IdActiviteCompl = Convert.ToInt32(row["id_activite_compl"]),
                        DateActivite = Convert.ToDateTime(row["date_activite"]),
                        LieuActivite = Convert.ToString(row["lieu_activite"]),
                        ThemeActivite = Convert.ToString(row["theme_activite"]),
                        MotifActivite = Convert.ToString(row["motif_activite"])
                    };

                    invitations.Add(invitation);
                }

                return invitations;
            }
            catch (Exception e)
            {
                // Log the exception details (e.Message, e.StackTrace) for debugging
                // You can also consider custom exception handling based on the exception type

                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }








        public static void AjouterActivite(ActiviteCompl activite)
        {
            using (var conn = Connexion.getInstance().getConnexion())
            {
                // Assurez-vous que la connexion est ouverte.
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Créez une commande pour l'insertion de l'activité.
                        using (var cmd = new MySqlCommand("", conn, transaction))
                        {
                            cmd.CommandText = "INSERT INTO ACTIVITE_COMPL (date_activite, lieu_activite, theme_activite, motif_activite) VALUES (@dateActivite, @lieuActivite, @themeActivite, @motifActivite);";
                            cmd.Parameters.AddWithValue("@dateActivite", activite.DateActivite);
                            cmd.Parameters.AddWithValue("@lieuActivite", activite.LieuActivite);
                            cmd.Parameters.AddWithValue("@themeActivite", activite.ThemeActivite);
                            cmd.Parameters.AddWithValue("@motifActivite", activite.MotifActivite);
                            // Insérer l'activité dans la table ACTIVITE_COMPL
                            cmd.ExecuteNonQuery();

                            // Récupérer l'ID de l'activité insérée en exécutant une nouvelle requête
                            cmd.CommandText = "SELECT LAST_INSERT_ID();";
                            long activiteId = (long)cmd.ExecuteScalar();


                            // Vérifiez si l'ID de l'activité est valide.
                            if (activiteId > 0)
                            {
                                // Créez une nouvelle commande pour l'insertion dans la table "inviter".
                                using (var cmdInviter = new MySqlCommand("", conn, transaction))
                                {
                                    // Définissez la commande avec la nouvelle requête SQL.
                                    cmdInviter.CommandText = "INSERT INTO inviter (id_activite_compl, id_praticien) VALUES (@activiteId, @praticienId);";

                                    // Ajoutez les paramètres à la commande.
                                    cmdInviter.Parameters.AddWithValue("@activiteId", activiteId);
                                    cmdInviter.Parameters.AddWithValue("@praticienId", activite.PraticienId);

                                    // Exécutez la commande pour insérer dans la table "inviter".
                                    cmdInviter.ExecuteNonQuery();


                                }

                                // Confirmez la transaction si toutes les commandes sont exécutées avec succès.
                                transaction.Commit();
                            }
                            else
                            {
                                // Si l'ID de l'activité n'est pas valide, lancez une exception.
                                throw new Exception("Failed to insert activity: No ID returned");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // En cas d'erreur, annulez la transaction et lancez une exception avec un message approprié.
                        transaction.Rollback();
                        throw new Exception("Failed to insert activity: " + ex.Message, ex);
                    }
                    finally
                    {
                        // Assurez-vous que la connexion est correctement fermée.
                        conn.Close();
                    }
                }
            }
        }














        public static ActiviteCompl GetActiviteById(int id)
        {
            Serreurs er = new Serreurs("Erreur sur récupération de l'activité.", "ServiceActiviter.GetActiviteById()");
            string mysql = "SELECT id_activite_compl, date_activite, lieu_activite, theme_activite, motif_activite FROM activite_compl WHERE id_activite_compl = @id";
            DataTable dataTable;

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                dataTable = DBInterface.Lecture(mysql, param, er);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dr = dataTable.Rows[0];
                    return new ActiviteCompl
                    {
                        IdActiviteCompl = Convert.ToInt32(dr["id_activite_compl"]),
                        DateActivite = Convert.ToDateTime(dr["date_activite"]),
                        LieuActivite = dr["lieu_activite"].ToString(),
                        ThemeActivite = dr["theme_activite"].ToString(),
                        MotifActivite = dr["motif_activite"].ToString()
                    };
                }
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }

            return null;
        }



        public static void UpdateActivite(ActiviteCompl activite)
    {
        string query = "UPDATE activite_compl SET " +
                       "date_activite = @dateActivite, " +
                       "lieu_activite = @lieuActivite, " +
                       "theme_activite = @themeActivite, " +
                       "motif_activite = @motifActivite " +
                       "WHERE id_activite_compl = @idActiviteCompl";

        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"@dateActivite", activite.DateActivite},
            {"@lieuActivite", activite.LieuActivite},
            {"@themeActivite", activite.ThemeActivite},
            {"@motifActivite", activite.MotifActivite},
            {"@idActiviteCompl", activite.IdActiviteCompl}
        };

        try
        {
            DBInterface.Execute_Transaction(query, parameters); // Assuming this method can accept parameters
        }
        catch (MySqlException ex)
        {
            throw new MonException("Erreur lors de la mise à jour de l'activité.", "ServiceActiviter.UpdateActivite", ex.Message);
        }
    }

        





        public static void DeleteActivite(int id)
        {
            string requete = "DELETE FROM activite_compl WHERE id_activite_compl = @id";
            Dictionary<string, object> parametres = new Dictionary<string, object>();
            parametres.Add("@id", id);

            try
            {
                DBInterface.Insert(requete, parametres); // Utilisez ici la méthode appropriée pour exécuter une commande de suppression.
            }
            catch (MySqlException ex)
            {
                throw new MonException("Erreur lors de la suppression de l'activité", "DeleteActivite", ex.Message);
            }
        }




        // ... autres méthodes pour ajouter, modifier et supprimer des invitations ...
    }
}
