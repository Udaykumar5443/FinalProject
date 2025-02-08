using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        public void UpdateTemplate(DynamicTemplate template)
        {
            _templateRepository.UpdateTemplate(template);
        }

        public void SoftDeleteTemplate(int id)
        {
            _templateRepository.SoftDeleteTemplate(id);
        }
        public void CreateTable(string templateName, string[] headers)
        {
            _templateRepository.CreateTable(templateName, headers);
        }
    }
}
