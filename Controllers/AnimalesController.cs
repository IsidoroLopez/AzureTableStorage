using AppTableStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppTableStorage.Controllers
{
    public class AnimalesController : Controller
    {
        // GET: Animales
        ModeloAnimalesStorage modelo;

        public AnimalesController()
        {
            this.modelo = new ModeloAnimalesStorage();
        }

        // GET: animales
        public ActionResult Index()
        {
            return View();
        }

        // GET: Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(String idanimal, String nombre, int edad, String raza)
        {
            modelo.GuardarAnimalTabla(idanimal, nombre, edad, raza);
            ViewBag.Mensaje = "Animal registrado en tabla correctamente.";
            return View();
        }

        public ActionResult Buscaranimalesraza()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Buscaranimalesraza(string raza)
        {
            List<Animales> lista =this.modelo.GetAnimalesRaza(raza);
            if (lista == null)
            {
                ViewBag.Mensaje = "No se han encontrado animales";
                return View();
            }
            else
            {
                return View(lista);
            }
        }

        public ActionResult DetalleAnimal(String idanimal, String raza)
        {
            Animales cli = this.modelo.GetAnimalTabla(raza, idanimal);
            return View(cli);
        }

        public ActionResult ModificarAnimal(String idanimal, String raza)
        {
            Animales cli = this.modelo.GetAnimalTabla(raza, idanimal);
            return View(cli);
        }

        [HttpPost]
        public ActionResult ModificarAnimal(String idanimal, String raza
            , String nombre, int edad)
        {
            Animales cli = this.modelo.ModificarAnimal(raza, idanimal, nombre, edad);
            if (cli != null)
            {
                ViewBag.Mensaje = "Animal modificado correctamente";
            }
            else
            {
                ViewBag.Mensaje = "Error al modificar el Animal";
            }
            return View(cli);
        }


        public ActionResult EliminarAnimal(String idanimal, String raza)
        {
            this.modelo.EliminarAnimal(raza, idanimal);
            return View("Index");
        }
    }
}

