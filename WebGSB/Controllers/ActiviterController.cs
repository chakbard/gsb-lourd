using Microsoft.AspNetCore.Mvc;
using WebGSB.Models.Dao;
using WebGSB.Models.Metier;
using WebGSB.Models.MesExceptions;
using System;
using System.Diagnostics;

namespace WebGSB.Controllers
{
    public class ActiviterController : Controller
    {

        public IActionResult Add()
        {
            return View();
        }

        //Ne pas oublier d'afficher la vue sinon la page ne va jamais s'afficher (return la vue avec le nom de la page) 

        public IActionResult Modifier()
        {
            return View();
        }

        public IActionResult Ajouter()
        {
            return View();
        }


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




        [HttpGet]
        public IActionResult Add(int praticienId)
        {
            var model = new ActiviteCompl { PraticienId = praticienId };
            return View(model);
        }

        

        [HttpPost]
        public IActionResult Add(ActiviteCompl activite)
        {
            // Log data to verify what is received (You can use Debug.WriteLine or any logging approach)
            Debug.WriteLine($"Received: Date: {activite.DateActivite}, Lieu: {activite.LieuActivite}, Theme: {activite.ThemeActivite}, Motif: {activite.MotifActivite}, PraticienId: {activite.PraticienId}");

            if (ModelState.IsValid)
            {
                try
                {
                    ServiceActiviter.AjouterActivite(activite);
                    TempData["SuccessMessage"] = "Activité ajoutée avec succès.";
                    return RedirectToAction("Index");
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
