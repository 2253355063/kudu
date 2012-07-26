﻿using System.Net;
using Kudu.Client.Deployment;
using Kudu.Client.Infrastructure;
using Kudu.Web.Infrastructure;

namespace Kudu.Web.Models
{
    public class SettingsService : ISettingsService
    {
        private readonly IApplicationService _applicationService;
        private readonly ICredentialProvider _credentialProvider;

        public SettingsService(IApplicationService applicationService, ICredentialProvider credentialProvider)
        {
            _applicationService = applicationService;
            _credentialProvider = credentialProvider;
        }

        public ISettings GetSettings(string siteName)
        {
            IApplication application = _applicationService.GetApplication(siteName);
            ICredentials credentials = _credentialProvider.GetCredentials();
            RemoteDeploymentSettingsManager settingsManager = application.GetSettingsManager(credentials);

            return new Settings
            {
                AppSettings = settingsManager.GetValues().Result
            };
        }

        public void SetConnectionString(string siteName, string name, string connectionString)
        {
            IApplication application = _applicationService.GetApplication(siteName);
            ICredentials credentials = _credentialProvider.GetCredentials();
            RemoteDeploymentSettingsManager settingsManager = application.GetSettingsManager(credentials);

            // settingsManager.SetConnectionString(name, connectionString);
        }

        public void RemoveConnectionString(string siteName, string name)
        {
            IApplication application = _applicationService.GetApplication(siteName);
            ICredentials credentials = _credentialProvider.GetCredentials();
            RemoteDeploymentSettingsManager settingsManager = application.GetSettingsManager(credentials);

            // settingsManager.RemoveConnectionString(name);
        }

        public void RemoveAppSetting(string siteName, string key)
        {
            IApplication application = _applicationService.GetApplication(siteName);
            ICredentials credentials = _credentialProvider.GetCredentials();
            RemoteDeploymentSettingsManager settingsManager = application.GetSettingsManager(credentials);

            settingsManager.Delete(key);
        }

        public void SetAppSetting(string siteName, string key, string value)
        {
            IApplication application = _applicationService.GetApplication(siteName);
            ICredentials credentials = _credentialProvider.GetCredentials();
            RemoteDeploymentSettingsManager settingsManager = application.GetSettingsManager(credentials);

            settingsManager.SetValue(key, value);
        }

    }
}