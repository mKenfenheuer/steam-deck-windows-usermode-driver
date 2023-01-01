using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWICD.Pages;
using SWICD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.ApiControllers
{
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Route("/config")]
        public async Task<IActionResult> GetConfig()
        {
            return Content(JsonConvert.SerializeObject(ControllerService.Instance.Configuration), "text/json");
        }

        [HttpGet]
        [Route("/driver/status")]
        public async Task<IActionResult> GetStatus()
        {
            return Content(JsonConvert.SerializeObject(new
            {
                ControllerService.Instance.EmulationEnabled,
                ControllerService.Instance.FailedInit,
                ControllerService.Instance.DecisionExecutable,
                ControllerService.Instance.LizardMouseEnabled,
                ControllerService.Instance.LizardButtonsEnabled,
                ControllerService.Instance.OnScreenKeyboardEnabled,
                ControllerService.Instance.Started
            }), "text/json");
        }

        [HttpGet]
        [Route("/driver/control/stop")]
        public async Task<IActionResult> StopDriver()
        {
            MainWindow.Instance.Dispatcher.Invoke(() => ControllerService.Instance.Stop());
            ContentResult result = Content(JsonConvert.SerializeObject(new
            {
                Started = ControllerService.Instance.Started,
            }), "text/json");

            if (!ControllerService.Instance.Started)
                result.StatusCode = 500;

            return result;
        }

        [HttpGet]
        [Route("/driver/control/start")]
        public async Task<IActionResult> StartDriver()
        {
            MainWindow.Instance.Dispatcher.Invoke(() => ControllerService.Instance.Start());
            ContentResult result = Content(JsonConvert.SerializeObject(new
            {
                Started = ControllerService.Instance.Started,
            }), "text/json");

            if (!ControllerService.Instance.Started)
                result.StatusCode = 500;

            return result;
        }

        [HttpGet]
        [Route("/gui/show")]
        public async Task<IActionResult> ShowGui()
        {
            MainWindow.Instance.Dispatcher.Invoke(() => {
                MainWindow.Instance.Show();
                MainWindow.Instance.Focus();
            });
            ContentResult result = Content(JsonConvert.SerializeObject(new
            {
                Visible = MainWindow.Instance.IsVisible,
            }), "text/json");

            return result;
        }

        [HttpGet]
        [Route("/gui/hide")]
        public async Task<IActionResult> HideGui()
        {
            MainWindow.Instance.Dispatcher.Invoke(() => MainWindow.Instance.Hide());
            ContentResult result = Content(JsonConvert.SerializeObject(new
            {
                Visible = MainWindow.Instance.IsVisible,
            }), "text/json");

            return result;
        }
    }
}
