using FakeItEasy;
using Microsoft.Extensions.Logging;
using SmartWMS.Controllers;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class AlertControllerTest
{
    private readonly IAlertRepository _alertRepository;
    private readonly AlertController _alertController;
    private readonly ILogger<AlertController> _logger;

    public AlertControllerTest()
    {
        this._alertRepository = A.Fake<IAlertRepository>();
        this._logger = A.Fake<ILogger<AlertController>>();
        this._alertController = new AlertController(_alertRepository, _logger);
    }

    private static AlertDto CreateFakeAlert() => A.Fake<AlertDto>();

}