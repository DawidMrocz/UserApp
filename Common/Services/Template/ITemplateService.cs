using Common.Models.Template;

namespace Common.Services.Template
{
    internal interface ITemplateService
    {
        Task<TemplateModel> Get(string strongName);
    }
}