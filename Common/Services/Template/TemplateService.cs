using Common.Models.Template;
using Common.Repositories.Template;
using Common.Services.Database;

namespace Common.Services.Template
{
    internal class TemplateService : ITemplateService
    {
        private readonly IDatabaseService _databaseService;

        public TemplateService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<TemplateModel> Get(string strongName)
        {
            return await _databaseService.Get<TemplateModel>(Sql.Email_Template_Get, parameters: new { StrongName = strongName })
                ?? throw new Exception("Nie znaleziono szablonu");
        }
    }
}
