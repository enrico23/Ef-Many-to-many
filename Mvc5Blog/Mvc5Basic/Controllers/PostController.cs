using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5Basic.Models;
using Mvc5Basic.DAL;
using Mvc5Basic.ViewModels;
using System.Data.Entity.Infrastructure;

namespace Mvc5Basic.Controllers
{
    public class PostController : Controller
    {
        private BlogDbContext db = new BlogDbContext();


        private void PopulateCategories(Post post)
        {
            
                var allCategories = db.Categories;
                HashSet<int> postCategories = new HashSet<int>();
                var viewModel = new List<AssignedCategoryViewModel>();

                if (post != null)
                {
                    postCategories = new HashSet<int>(post.categories.Select(c => c.CategoryId));
                    foreach (var item in allCategories)
                    {
                        viewModel.Add(new AssignedCategoryViewModel
                        {
                            CategoryId = item.CategoryId,
                            Name = item.Name,
                            Assigned = postCategories.Contains(item.CategoryId)
                        });
                    }
                }
                else {
                    postCategories = new HashSet<int>(allCategories.Select(c => c.CategoryId));
                    foreach (var item in allCategories)
                    {
                        viewModel.Add(new AssignedCategoryViewModel
                        {
                            CategoryId = item.CategoryId,
                            Name = item.Name,
                            Assigned = false
                        });
                    }
                }
                
               
                ViewBag.Categories = viewModel;
            
            
               
        }


        // GET: /Post/
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: /Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(c => c.categories).Where(i => i.PostId == id).SingleOrDefault();
            PopulateCategories(post);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: /Post/Create
        public ActionResult Create()
        {
            PopulateCategories(null);
            return View();
        }

        // POST: /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Name")] Post post, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                UpdatePostCategories(selectedCategories, post);

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: /Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(c => c.categories).Where(i => i.PostId == id).SingleOrDefault();
            PopulateCategories(post);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: /Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string[] selectedCategories, int? PostId)
        {
            
            Post newPost = db.Posts.Include(c => c.categories).Where(i => i.PostId == PostId).SingleOrDefault();
            if (TryUpdateModel(newPost, "", new string[] { "Name" }))
            {
                try
                {

                    UpdatePostCategories(selectedCategories, newPost);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateCategories(newPost);
            return View(newPost);
        }


        private void UpdatePostCategories(string[] selectedCategories, Post postToUpdate)
        {
               if (selectedCategories == null)
               {
                  postToUpdate.categories = new List<Category>();
                  return;
               }
 
               var selectedCategoriesHS = new HashSet<string>(selectedCategories);
               var postCategories = new HashSet<int>(postToUpdate.categories.Select(c => c.CategoryId));

               foreach (var item in db.Categories)
               {
                  if (selectedCategoriesHS.Contains(item.CategoryId.ToString()))
                  {
                     if (!postCategories.Contains(item.CategoryId))
                     {
                        postToUpdate.categories.Add(item);
                     }
                  }
                  else
                  {
                     if (postCategories.Contains(item.CategoryId))
                     {
                        postToUpdate.categories.Remove(item);
                     }
                  }
               }

        }




        // GET: /Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: /Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
