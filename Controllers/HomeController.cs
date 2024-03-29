﻿using DotNet5OctBatch_2021.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNet5OctBatch_2021.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly POSContext _dbcontext;
        public HomeController(ILogger<HomeController> logger, POSContext dbcontext, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            webHostEnvironment = hostEnvironment;
        }
        #region Manual Work
        public IActionResult Index()
        {
            try
            {
                Users oUser1 = new Users();
                oUser1.Id = 1;
                oUser1.Name = "ABC";
                oUser1.Password = "TEST";
                //  ViewBag.User1 = oUser1;

                Users oUser2 = new Users
                {
                    Id = 2,
                    Name = "XYZ",
                    Password = "123"
                };

                Users oUser3 = new Users
                {
                    Id = 3,
                    Name = "XYZ-ABC",
                    Password = "1234556"
                };
                // ViewBag.OUser2 = oUser2;
                IList<Users> oListUsers = new List<Users>();
                oListUsers.Add(oUser1);
                oListUsers.Add(oUser2);
                oListUsers.Add(oUser3);
                ViewBag.UsersList = oListUsers;
                try
                {

                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // log in error
                ViewBag.ErroMesasge = "Some Error Occured. Please Try Again!";
            }

            return View();
        }

        public IActionResult UsersList()
        {
            Users oUser1 = new Users();
            oUser1.Id = 1;
            oUser1.Name = "ABC";
            oUser1.Password = "TEST";
            Users oUser2 = new Users
            {
                Id = 2,
                Name = "XYZ",
                Password = "123"
            };
            Users oUser3 = new Users
            {
                Id = 3,
                Name = "XYZ-ABC",
                Password = "1234556"
            };
            IList<Users> oListUsers = new List<Users>();
            oListUsers.Add(oUser3);
            oListUsers.Add(oUser1);
            oListUsers.Add(oUser2);

            ViewBag.UsersList = oListUsers;
            return View();
        }
        public IActionResult MyContact()
        {
            ViewBag.Name = "ASP Dot Net";
            return View();
        }
        public IActionResult MyFirstPage()
        {
            return View();
        }
        public IActionResult AddUser()
        {
            Users ObjUser = new Users();
            try
            {

                ObjUser.Id = 1;
                ObjUser.Name = "ASP COre Class";
                ObjUser.UserName = "theta";
                ObjUser.Password = "123";

            }
            catch (Exception ex)
            {

            }
            return View(ObjUser);
        }
        #endregion

        #region Categories Management
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(ItemCategory itemcat)
        {
            try
            {
                _dbcontext.ItemCategories.Add(itemcat);
                _dbcontext.SaveChanges();
                ViewBag.SMessage = "Data Saved Successfully";
            }
            catch (Exception ex)
            {
                ViewBag.EMessage = "Some Error Occured. Please try again";
            }

            return View();
        }
        public IActionResult AllCategories()
        {
            // show level 1 and level 2 categories
          //  IList<ItemCategory> lCategories = _dbcontext.ItemCategories.Where(o=> o.CatLevel >=1 && o.CatLevel <=2).ToList();
            // show all categories except level 1
          //  lCategories = _dbcontext.ItemCategories.Where(o => o.CatLevel !=1).ToList();
            // string ---- show all categories start with alphabet A
           var lCategories = _dbcontext.ItemCategories.ToList();
            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            return View(lCategories);
        }
        [HttpGet]
        public IActionResult DetailCategory(int id)
        {
            try
            {
                ItemCategory ObjItemCategory = _dbcontext.ItemCategories.Find(id);
                return View(ObjItemCategory);
            }
            catch(Exception ex)
            {

            }
            return View();
        }
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            try
            {
                ViewBag.SMeesage = TempData["SMessage"];
                ViewBag.EMessage = TempData["EMessage"];
                ItemCategory ObjItemCategory = _dbcontext.ItemCategories.Find(id);
                return View(ObjItemCategory);
            }
            catch (Exception)
            {
                return View();
            }
          
        }
        [HttpPost]
        public IActionResult EditCategory(ItemCategory ObjCategory)
        {
            try
            {
                //_dbcontext.ItemCategories.Attach(ObjCategory);
                //var entry = _dbcontext.Entry(ObjCategory);
                //entry.State = EntityState.Modified;
                ObjCategory.ModifyBy = "Theta";
                ObjCategory.ModifyDate = DateTime.Now;
                _dbcontext.ItemCategories.Update(ObjCategory);
                _dbcontext.SaveChanges();
                TempData["SMessage"] = "Data Updated Successfully";
            }
            catch(Exception ex)
            {
                TempData["EMessage"] = "Some error occured. please try again";
            }
           
            return RedirectToAction(nameof(HomeController.EditCategory), new { ObjCategory.Id });
        }

        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var objCat = _dbcontext.ItemCategories.Find(id);
                if(objCat != null)
                {
                    _dbcontext.ItemCategories.Remove(objCat);
                    _dbcontext.SaveChanges();
                    TempData["SMessage"] = "Deleted Successfully";
                }
                else
                {
                    TempData["EMessage"] = "Category not found";
                }
            }
            catch(Exception ex)
            {
                TempData["EMessage"] = "Some error occured";
            }
            return RedirectToAction(nameof(HomeController.AllCategories));
        }

        
        #endregion

        #region item Management 
        [HttpGet]
        public IActionResult AddItem()
        {
            ViewBag.ListCategories = _dbcontext.ItemCategories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddItem(Item oItem)
        {
            try
            {
                if(oItem.ItemCategory == null)
                {
                    oItem.ItemCategory = 0;
                }
                oItem.Image = UploadedFile(oItem);
                oItem.CreatedBy = "Theta";
                oItem.CreatedDate = DateTime.Now;

                //_dbcontext.Items.Add(oItem);
                //_dbcontext.SaveChanges();
                ViewBag.SMessage = "Data saved successfully";
            }
            catch(Exception ex)
            {
                ViewBag.EMessage = "Some Error Occurred";
            }
            return View();
        }
        [HttpGet]
        public IActionResult AllItem()
        {
            IList<ViewItems> OListItemWithCategory = (from item in _dbcontext.Items
                                                     // join category in _dbcontext.ItemCategories on item.ItemCategory equals category.Id
                                                     from category in _dbcontext.ItemCategories.Where(c => c.Id == item.ItemCategory ).DefaultIfEmpty()
                                                      select new ViewItems
                                                      {
                                                          ItemCode = item.ItemCode,
                                                          ItemName = item.ItemName,
                                                          CatCode = category.CatCode,
                                                          CatName = category.CatName,
                                                          ItemQuantity = (double?) item.ItemQuantity ?? 0,
                                                      }).ToList();
            return View(OListItemWithCategory);
        }
        #endregion

        #region Sales Management
        [HttpGet]
        public ActionResult AddSale()
        {
            ViewBag.SMeesage = TempData["SMessage"];
            ViewBag.EMessage = TempData["EMessage"];
            ViewBag.ListItems = _dbcontext.Items.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddSale(string objData)
        {
            try
            {
                ViewSales objMain = JsonConvert.DeserializeObject<ViewSales>(objData, new IsoDateTimeConverter());
                var exSale = _dbcontext.Sale.OrderByDescending(m => m.Id).FirstOrDefault();
                var serial = 0;
                if (exSale != null)
                {
                    serial = exSale.Serial + 1;
                }
                objMain.objSale.Serial = serial;
                objMain.objSale.Code = "Sale/" + objMain.objSale.SaleDate.Month.ToString() + "/" + objMain.objSale.SaleDate.Year.ToString() + "/" + serial.ToString("000");

                objMain.objSale.CreatedBy = "System";
                objMain.objSale.CreatedDate = DateTime.Now;
                _dbcontext.Sale.Add(objMain.objSale);
                _dbcontext.SaveChanges();
                foreach (SaleLine item in objMain.ListSaleLine)
                {
                  
                    var saleid = objMain.objSale.Id;
                    item.SaleId = saleid;
                    item.CreatedBy = "System";
                    item.CreatedDate = DateTime.Now;
                    _dbcontext.SaleLine.Add(item);
                    _dbcontext.SaveChanges();
                    // Sale stock update (minus)
                    // Purchase stock update (plus)
                    Item objItem = _dbcontext.Items.Find(item.ItemId);
                    objItem.ItemQuantity = objItem.ItemQuantity - (double)item.Qty;
                    _dbcontext.Items.Update(objItem);
                    _dbcontext.SaveChanges();
                }
                TempData["SMessage"] = "Data Updated Successfully";
            }
            catch(Exception ex)
            {
                TempData["EMessage"] = "Some error occured. please try again";
            }

            return RedirectToAction(nameof(HomeController.AddSale));
        }

        [HttpPost]
        public JsonResult ReturnItemDetail(int ItemId)
        {
            Item objItem = _dbcontext.Items.Find(ItemId);
            //bool Result = false;
            //if(objItem.ItemQuantity > 0)
            //{
            //    Result = true;
            //}
            //var jsonObj = new
            //{
            //    Qty = objItem.ItemQuantity,
            //};
            return Json(objItem);
            //return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }
        #region all sales
        public ActionResult AllSales()
        {
            IList<ViewSales> ListSales = (from sale in _dbcontext.Sale
                                          from saleline in _dbcontext.SaleLine.Where(m => m.SaleId == sale.Id).DefaultIfEmpty()
                                          from item in _dbcontext.Items.Where(m=> m.Id == saleline.ItemId).DefaultIfEmpty()
                                          select new ViewSales
                                          {

                                              // properties
                                          }).ToList();
            return View();
        }
        #endregion
        #endregion
        private string UploadedFile(Item model)
        {
            string uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                     model.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
