using HomematicApp.Domain.Abstractions;
using RazorLight;
using System.Reflection;

namespace HomematicApp.Service.Services
{
    public class TemplateFillerService : ITemplateFillerService
    {
        private IRazorLightEngine _razorEngine;
        public TemplateFillerService()
        {
            _razorEngine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
            .Build();
        }

        public async Task<string> FillTemplate(string path, object model)
        {
            string template = File.ReadAllText(path);
            string compiledTemplate = await _razorEngine.CompileRenderStringAsync("reset", template, model);

            return compiledTemplate;
        }
    }
}
