using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using WebGSB.Models.MesExceptions;

namespace WebGSB.Models.Persistance
{
    public class Connexion
    {
        private static MySqlConnection connexion = null;
        private static Connexion instance = null;
        private static readonly object padlock = new object();

        private Connexion()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("bddCourante");
            connexion = new MySqlConnection(connectionString);
            connexion.Open();
        }

        public static Connexion getInstance()
        {
            lock (padlock)
            {
                if (instance == null || connexion.State != ConnectionState.Open)
                {
                    instance = new Connexion();
                }
                return instance;
            }
        }

        public MySqlConnection getConnexion()
        {
            return connexion;
        }

        public static void closeConnexion()
        {
            if (instance != null && connexion != null)
            {
                connexion.Close();
                connexion = null;  // Reset the connection
            }
        }

        public static DataTable Lecture(string requete, Serreurs er)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(requete, getInstance().getConnexion()))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), ex.Message);
            }
            catch (Exception e)
            {
                throw new MonException("Erreur lors de l'exécution de la requête.", "Erreur lors de l'accès aux données.", e.Message);
            }
            return dt;
        }
    }
}
