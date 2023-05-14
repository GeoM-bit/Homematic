namespace HomematicApp.Domain.Abstractions
{
    public interface ITemplateFillerService
    {
        Task<string> FillTemplate(string path, object model);
    }
}
