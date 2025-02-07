using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.DAL;
using FinalProject.Models;

namespace FinalProject.BLL
{
    public class DynamicTemplateService
    {
        private readonly DynamicTemplateRepository _templateRepository;

        public DynamicTemplateService()
        {
            _templateRepository = new DynamicTemplateRepository();
        }

        // Get all templates
        public List<DynamicTemplate> GetTemplates()
        {
            return _templateRepository.GetAllTemplates();
        }

        public DynamicTemplate GetTemplateById(int id)
        {
            return _templateRepository.GetTemplateById(id);
        }

        // Add new template
        public void AddTemplate(DynamicTemplate template)
        {
            // Business logic (if required) before adding template
            _templateRepository.AddTemplate(template);
        }

        public void ProcessCsvFile(string filePath)
        {
            List<DynamicTemplate> templates = new List<DynamicTemplate>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines.Skip(1)) // Skip header row
            {
                string[] values = line.Split(',');

                DynamicTemplate template = new DynamicTemplate
                {
                    FileTemplateName = values[0],
                    Domain = values[1],
                    Category = values[2],
                    SchoolYear = Convert.ToInt32(values[3]),
                    Roles = values[4],
                    StatusId = int.Parse(values[5])
                };

                templates.Add(template);
            }

            _templateRepository.SaveFileData(templates);
        }

        public List<DynamicTemplate> GetFileData(int fileId)
        {
            return _templateRepository.GetFileData(fileId);
        }

        public void UpdateTemplate(DynamicTemplate template)
        {
            _templateRepository.UpdateTemplate(template);
        }

        public void SoftDeleteTemplate(int id)
        {
            _templateRepository.SoftDeleteTemplate(id);
        }
    }
}
