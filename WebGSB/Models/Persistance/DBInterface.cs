using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using WebGSB.Models.MesExceptions;
namespace WebGSB.Models.Persistance
{

    public class DBInterface
    {
        public static DataTable Lecture(string req, Dictionary<string, object> parameters, Serreurs er)
        {
            MySqlConnection cnx = null;
            try
            {
                cnx = Connexion.getInstance().getConnexion();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = req;

                // Ajoutez les paramètres à la commande
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "resultat");
                cnx.Close();
                return ds.Tables["resultat"];
            }
            catch (MonException me)
            {
                throw me;
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
            finally
            {
                if (cnx != null)
                    cnx.Close();
            }
        }






        public static DataTable Lecture(String req, Serreurs er)
        {
            MySqlConnection cnx = null;
            try
            {
                cnx = Connexion.getInstance().getConnexion();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = req;
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                // Construire le DataSet
                DataSet ds = new DataSet();
                da.Fill(ds, "resultat");
                cnx.Close();
                // Retourner la table
                return (ds.Tables["resultat"]);
            }
            catch (MonException me)
            {
                throw (me);
            }
            catch (Exception e)
            {
                throw new
               MonException(er.MessageUtilisateur(), er.MessageApplication(),
               e.Message);
            }
            finally
            {
                // S'il y a eu un problème, la connexion
                // peut être encore ouverte, dans ce cas
                // il faut la fermer.
                if (cnx != null)
                    cnx.Close();
            }
        }

        public static void Execute_Transaction(string query, Dictionary<string, object> parameters)
        {
            using (var conn = Connexion.getInstance().getConnexion())
            {
                // Check if the connection is already open
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var transaction = conn.BeginTransaction())
                {
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Transaction = transaction;

                    // Adding parameters to the command
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }

                // Ideally, the connection should be closed here, but the 'using' statement handles it.
            }
        }


        public static void Execute_Transaction(String requete)
        {
            MySqlConnection cnx = null;
            try
            {
                // On ouvre une transaction
                cnx = Connexion.getInstance().getConnexion();
                MySqlTransaction OleTrans =
                cnx.BeginTransaction();
                MySqlCommand OleCmd = new MySqlCommand();
                OleCmd = cnx.CreateCommand();
                OleCmd.Transaction = OleTrans;
                OleCmd.CommandText = requete;
                OleCmd.ExecuteNonQuery();
                OleTrans.Commit();
            }
            catch (MySqlException uneException)
            {
                throw new MonException(uneException.Message,
               "Insertion", "SQL");
            }
        }

        


        public static DataTable Lecture(MySqlCommand cmd, Serreurs er)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = Connexion.getInstance().getConnexion())
                {
                    cmd.Connection = conn;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), ex.Message);
            }
            return dt;
        }

        public static void Insert(string requete, Dictionary<string, object> parametres)
        {
            MySqlConnection cnx = null;
            try
            {
                cnx = Connexion.getInstance().getConnexion();
                MySqlCommand cmd = new MySqlCommand(requete, cnx);

                foreach (var parameter in parametres)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Improved error handling: Log this exception or include more details
                throw new MonException("Error during database insertion.", "Insertion", "SQL", ex);
            }
            finally
            {
                cnx?.Close();
            }
        }


        
    }
}
