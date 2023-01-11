using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonBlazorLibrary.Helpers
{
    public class GitHelper
    {
        private static GitHelper _githelper = new GitHelper();

        public static GitHelper Instance { get { return _githelper; } }

        private string RepoPath = null;

        public void VerifyRepository(string sourceUrl, string repositoryFolder)
        {
            if (!Repository.IsValid(repositoryFolder))
            {
                //var cloneOpt = new CloneOptions
                //{
                //    CredentialsProvider = (ur, us, ce) => new UsernamePasswordCredentials
                //    {
                //        Username = "MyUser",
                //        Password = ("Some App password from bitbucket")
                //    },
                //    OnTransferProgress = onTransfer,
                //    CertificateCheck = onCertCheck,
                //    OnProgress = onProgress,
                //};

                try
                {
                    StaticObjects.Logger.Info($"Start cloning repository: {sourceUrl}");
                    StaticObjects.Logger.Info($" Destination folder: {repositoryFolder}");
                    RepoPath = Repository.Clone(sourceUrl, repositoryFolder);
                    StaticObjects.Logger.Info($" Cloned to: {RepoPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            else
            {
                RepoPath = Repository.Init(repositoryFolder);
            }
        }


    }
}
