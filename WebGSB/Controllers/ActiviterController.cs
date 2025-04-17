using Microsoft.AspNetCore.Mvc;
using WebGSB.Models.Dao;
using WebGSB.Models.Metier;
using WebGSB.Models.MesExceptions;
using System;
using System.Diagnostics;
using WebGSB.Filters;

namespace WebGSB.Controllers
{
    public class ActiviterController : Controller
    {
        [AuthorizeAdmin]
        public IActionResult Add()
        {
            return View();
        }

        //Ne pas oublier d'afficher la vue sinon la page ne va jamais s'afficher (return la vue avec le nom de la page) 
        [AuthorizeAdmin]
        public IActionResult Modifier()
        {
            return View();
        }
        [AuthorizeAdmin]
        public IActionResult Ajouter()
        {
            return View();
        }

        [AuthorizeAdmin]
        public IActionResult IndexAc(int idPraticien)
        {
            var invitations = ServiceActiviter.GetInvitationsParPraticienId(idPraticien);
            if (invitations.Count == 0)
            {
                // Log for debugging or handle the case where no data is returned
                Console.WriteLine("No activities found for practitioner with ID: " + idPraticien);
            }
            return View(invitations);
        }





        [AuthorizeAdmin]
        [HttpGet]
        public IActionResult Add(int praticienId)
        {
            var model = new ActiviteCompl { PraticienId = praticienId };
            return View(model);
        }



        [AuthorizeAdmin]
        [HttpPost]
        public IActionResult Add(ActiviteCompl activite)
        {
            Debug.WriteLine($"Received: Date: {activite.DateActivite}, Lieu: {activite.LieuActivite}, Theme: {activite.ThemeActivite}, Motif: {activite.MotifActivite}, PraticienId: {activite.PraticienId}");

            if (ModelState.IsValid)
            {
                try
                {
                    ServiceActiviter.AjouterActivite(activite);
                    TempData["SuccessMessage"] = "Activité ajoutée avec succès.";
                    return RedirectToAction("Index", "Home"); // Assurez-vous que cette redirection est correcte.
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = $"Erreur lors de l'ajout de l'activité : {e.Message}";
                    return View(activite);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Des erreurs de validation sont présentes.";
                return View(activite);
            }
        }




        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult Modifier(int id)
        {
            var activite = ServiceActiviter.GetActiviteById(id);
            if (activite == null)
            {
                return NotFound();
            }
            return View(activite);
        }


        [AuthorizeAdmin]
        // POST: Update the activity in the database
        [HttpPost]
        [ValidateAntiForgeryToken] // Ensure this attribute is here to prevent CSRF attacks
        public ActionResult Modifier(ActiviteCompl activite)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ServiceActiviter.UpdateActivite(activite);
                    TempData["SuccessMessage"] = "Activité modifiée avec succès.";
                    return RedirectToAction("Index", "Praticien"); // Redirect to the index or a summary page
                }
                catch (MonException ex)
                {
                    ModelState.AddModelError("", "Erreur lors de la mise à jour : " + ex.Message);
                }
            }
            return View(activite);
        }




        [AuthorizeAdmin]

        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            try
            {
                ServiceActiviter.DeleteActivite(id);
                TempData["SuccessMessage"] = "Activité supprimée avec succès";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression de l'activité";
            } 
            return RedirectToAction("IndexAc", "Activiter");
        }

    }
}
