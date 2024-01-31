﻿using BudgetTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class CategoriesController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public CategoriesController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }

        // GET: CategoriesController
        public ActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            ViewBag.Action = "create";
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
