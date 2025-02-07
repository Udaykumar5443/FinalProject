using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.BLL;
using FinalProject.Models;



namespace FinalProject.web.Controllers
{
    public class DynamicTemplateController : Controller
    {
        private readonly DynamicTemplateService _templateService;

        public DynamicTemplateController()
        {
            _templateService = new DynamicTemplateService();
        }

        // Display all templates

        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<DynamicTemplate> templates = _templateService.GetTemplates()
                .Where(t => t.StatusId == 1) // Filter only active records
                .ToList();

            return View(templates);
        }



        // Show the Add Template form
        public ActionResult AddTemplate()
        {
            return View();
        }

        // Add new template (POST)
        [HttpPost]
        public ActionResult AddTemplate(DynamicTemplate template)
        {
            if (ModelState.IsValid)
            {
                _templateService.AddTemplate(template);
                return RedirectToAction("Index");
            }
            return View(template);
        }

        // Add New Template - Manual Form
        public ActionResult AddManual()
        {
            return View();
        }

        // Handle Manual Template Submission
        [HttpPost]
        public ActionResult AddManual(DynamicTemplate template)
        {
            if (ModelState.IsValid)
            {
                _templateService.AddTemplate(template);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), file.FileName);
                file.SaveAs(filePath);

                // Process CSV and save data
                _templateService.ProcessCsvFile(filePath);
            }
            return RedirectToAction("Index");
        }

        // Add New Template - CSV Upload Form
        public ActionResult AddCsv()
        {
            return View();
        }

        // Handle CSV Upload
        [HttpPost]
        public ActionResult AddCsv(HttpPostedFileBase file, DynamicTemplate template)
        {
            if (file != null && file.ContentLength > 0)
            {
                var filePath = Server.MapPath("~/Uploads/" + file.FileName);
                file.SaveAs(filePath);

                _templateService.ProcessCsvFile(filePath);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult ViewFileData(int id)
        {
            List<DynamicTemplate> fileData = _templateService.GetFileData(id);
            return View(fileData);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var template = _templateService.GetTemplateById(id);
            return View(template);
        }

        [HttpPost]
        public ActionResult Edit(DynamicTemplate template)
        {
            _templateService.UpdateTemplate(template);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _templateService.SoftDeleteTemplate(id);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var template = _templateService.GetTemplateById(id);
            return View(template);
        }

        [HttpPost]
        //public ActionResult UploadCsv(HttpPostedFileBase file, string templateName)
        //  {
        //      if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(templateName))
        //      {
        //          ViewBag.Message = "Invalid file or template name.";
        //          return View();
        //      }

        //      try
        //      {
        //          // Ensure the Uploads folder exists
        //          string folderPath = Server.MapPath("~/Uploads");
        //          if (!Directory.Exists(folderPath))
        //          {
        //              Directory.CreateDirectory(folderPath);
        //          }

        //          // Save the CSV file
        //          string filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));
        //          file.SaveAs(filePath);

        //          // Read CSV data
        //          string[] csvLines = System.IO.File.ReadAllLines(filePath);
        //          if (csvLines.Length == 0)
        //          {
        //              ViewBag.Message = "Empty CSV file.";
        //              return View();
        //          }

        //          // Extract headers (column names)
        //          string[] headers = csvLines[0].Split(',');

        //          //Call DAL to create table
        //            _templateService.CreateTable(templateName, headers);

        //          // Call DAL to insert data
        //          _templateService.InsertData(templateName, headers, csvLines);


        //          ViewBag.Message = "CSV Uploaded & Table Created Successfully!";
        //      }
        //      catch (Exception ex)
        //      {
        //          ViewBag.Message = "Error: " + ex.Message;
        //      }

        //      return View("Index");
        //  }

        public ActionResult UploadCsv(HttpPostedFileBase file, string templateName)
        {
            if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(templateName))
            {
                ViewBag.Message = "Invalid file or template name.";
                return View();
            }

            try
            {
                // Ensure the Uploads folder exists
                string folderPath = Server.MapPath("~/Uploads");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Save the CSV file
                string filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));
                file.SaveAs(filePath);

                // Read CSV file
                string[] csvLines = System.IO.File.ReadAllLines(filePath);
                if (csvLines.Length == 0)
                {
                    ViewBag.Message = "Empty CSV file.";
                    return View();
                }

                // Extract headers (column names)
                string[] headers = csvLines[0].Split(',');

                // Create the table with extracted headers
               // _templateService.CreateTable(templateName, headers);

                // Store the manual user inputs in the database

                ViewBag.Message = "Table Created Successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}