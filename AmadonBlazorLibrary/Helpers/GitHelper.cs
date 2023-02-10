using LibGit2Sharp;
using LibGit2Sharp.Handlers;
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

        /// <summary>
        /// Check is a path is a valid repository
        /// </summary>
        /// <param name="respositoryPath"></param>
        /// <returns></returns>
        public bool IsValid(string respositoryPath)
        {
            return Repository.IsValid(respositoryPath);
        }



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

        private CredentialsHandler Credentials(string username, string password)
        {
            return new CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = "username",
                            Password = "password"
                        });
        }


        private bool Fetch(Repository repo)
        {
            try
            {
                var options = new FetchOptions();
                options.Prune = true;
                options.TagFetchMode = TagFetchMode.Auto;
                //options.CredentialsProvider = Credentials(string username, string password)
                Remote remote = repo.Network.Remotes["origin"];
                string msg = "Fetching remote";
                IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                Commands.Fetch(repo, remote.Name, refSpecs, options, msg);
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.FireShowExceptionMessage($"Fetch Error, repository= {repo.Head.FriendlyName}: ", ex);
                return false;
            }
        }


        public bool Pull(string repositoryPath)
        {
            try
            {
                using Repository localRepo = new Repository(repositoryPath);
                PullOptions pullOptions = new PullOptions();
                pullOptions.FetchOptions = new FetchOptions();
                //options.CredentialsProvider = Credentials(string username, string password)
                Commands.Pull(localRepo, new Signature("username", "rogreis@gmail.com", new DateTimeOffset(DateTime.Now)), pullOptions);
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.FireShowExceptionMessage($"Pull Error, repository= {repositoryPath}: ", ex);
                StaticObjects.Logger.Error($"Pull Error, repository= {repositoryPath}: ", ex);
                return false;
            }
        }


        /// <summary>
        /// Checkout a branch overwriting local changes
        /// </summary>
        /// <param name="repositoryPath"></param>
        /// <param name="branchName"></param>
        /// <returns></returns>
        public bool Checkout(string repositoryPath, string branchName)
        {
            try
            {
                /*
                    git fetch --all
                    git reset --hard origin/abranch
                    git checkout abranch 
                */
                CheckoutOptions options = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force };
                using Repository localRepo = new Repository(repositoryPath);
                Branch branch = localRepo.Branches[branchName];
                Fetch(localRepo);
                Commands.Checkout(localRepo, branch, options);
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.FireShowExceptionMessage($"Checkout Error, repository= {repositoryPath}, branch= {branchName}: ", ex);
                StaticObjects.Logger.Error($"Checkout Error, repository= {repositoryPath}, branch= {branchName}: ", ex);
                return false;
            }
        }

        public bool Clone(string sourceUrl, string repositoryPath)
        {
            try
            {
                var cloneOptions = new CloneOptions { BranchName = "master", Checkout = true };
                var cloneResult = Repository.Clone(sourceUrl, repositoryPath);
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.FireShowExceptionMessage($"Clone Error, repository= {repositoryPath}, sourceUrl= {sourceUrl}: ", ex);
                StaticObjects.Logger.Error($"Clone Error, repository= {repositoryPath}, sourceUrl= {sourceUrl}: ", ex);
                return false;
            }
        }




    }
}
