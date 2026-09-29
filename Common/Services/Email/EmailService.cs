using Common.Services.Database;
using Common.Services.Template;
using Microsoft.Extensions.Configuration;

namespace Common.Services.Email
{
    internal partial class EmailService : IEmailService
    {
        private readonly ITemplateService _templateService;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;

        public EmailService(ITemplateService templateService, IConfiguration configuration, IDatabaseService databaseService)
        {
            _templateService = templateService;
            _configuration = configuration;
            _databaseService = databaseService;
        }
    }
}