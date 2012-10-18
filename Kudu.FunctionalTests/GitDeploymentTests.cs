﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Kudu.Core.Deployment;
using Kudu.FunctionalTests.Infrastructure;
using Kudu.TestHarness;
using Xunit;
using Xunit.Extensions;

namespace Kudu.FunctionalTests
{
    public class GitDeploymentTests
    {
        static GitTestConfig gitTestConfig = GetConfig();
    
        [Theory]
        [PropertyData("GetTestData")]
        public void PushAndDeployApps(string name, string repoName,
                                      string repoUrl, string repoCloneUrl,
                                      string defaultBranchName, string verificationText, 
                                      HttpStatusCode expectedResponseCode, bool skip, string verificationLogText = null)
        {
            if (!skip)
            {
                string randomTestName = KuduUtils.GetRandomWebsiteName(repoName);
                using (var repo = Git.Clone(randomTestName, repoCloneUrl))
                {
                    ApplicationManager.Run(randomTestName, appManager =>
                    {
                        // Act
                        appManager.GitDeploy(repo.PhysicalPath, defaultBranchName);
                        var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                        // Assert
                        Assert.Equal(1, results.Count);
                        Assert.Equal(DeployStatus.Success, results[0].Status);
                        KuduAssert.VerifyUrl(appManager.SiteUrl, verificationText, expectedResponseCode);
                        if (!String.IsNullOrEmpty(verificationLogText.Trim()))
                        {
                            KuduAssert.VerifyLogOutput(appManager, results[0].Id, verificationLogText.Trim());
                        }
                    });
                }
                Debug.Write(String.Format("Test completed: {0}\n", name));
            }
            else
            {
                Debug.Write(String.Format("Test skipped: {0}\n", name));
            }
        }

        public static IEnumerable<object[]> GetTestData
        {
            get
            {
                return gitTestConfig.GetTests();
            }
        }

        private static GitTestConfig GetConfig()
        {
            string path = PathHelper.GetPath(PathHelper.GitDeploymentTestsFile);
            return new GitTestConfig(path);
        }
    }
}
