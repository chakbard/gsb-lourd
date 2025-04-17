using Microsoft.AspNetCore.Mvc;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Utilitaires;
using WebGSB.Models.Metier;
using WebGSB.Models.Dao;
using WebGSB.Models.MesExceptions;
using WebGSB.Models.Utilitaires;
using WebGSB.Filters;

namespace WebGSB.Controllers
{
    [AuthorizeAdmin] // Appliquez le filtre au niveau du contrôleur ou de l'action spécifique
    public class PraticienController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ajout()
        {
            return View();
        }
    }

    public class ConnexionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string login, string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                ModelState.AddModelError("pwd", "The password field is required.");
                return View(); // Restez sur la page de connexion
            }

            try
            {
                ServiceUtilisateur serviceUtilisateur = new ServiceUtilisateur();
                Utilisateur utilisateur = serviceUtilisateur.GetUtilisateur(login);

                if (utilisateur == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(); // Restez sur la page de connexion avec l'erreur
                }

                if (pwd != utilisateur.PwdVisiteur)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(); // Restez sur la page de connexion avec l'erreur
                }

                // Set session with user's name and type
                HttpContext.Session.SetString("UserLogin", utilisateur.NomVisiteur);
                HttpContext.Session.SetString("typeVisiteur", utilisateur.TypeVisiteur);

                return RedirectToAction("Index", "Praticien"); // Redirection en cas de connexion réussie
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "An error occurred while processing your request: " + e.Message);
                return View(); // Restez sur la page de connexion en cas d'erreur
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Connexion");
        }
    }
}

