using Microsoft.AspNetCore.Mvc;
using WebGSB.Models.Dao;
using WebGSB.Models.Metier;
using WebGSB.Models.MesExceptions;
using System;
using System.Collections.Generic;

namespace WebGSB.Controllers
{
    public class PraticienController : Controller
    {
        public IActionResult Index(string nomRecherche = "") // Ajout du paramètre nomRecherche
        {
            List<Praticien> mesPraticiens = null;

            try
            {
                if (!string.IsNullOrEmpty(nomRecherche))
                {
                    mesPraticiens = ServicePraticien.GetPraticiensParNom(nomRecherche);
                }
                else
                {
                    mesPraticiens = ServicePraticien.GetTousLesPraticiens();
                }
            }
            catch (MonException e)
            {
                ModelState.AddModelError("Erreur", "Erreur lors de la recherche des praticiens: " + e.Message);
            }

            return View(mesPraticiens);
        }
    }
}