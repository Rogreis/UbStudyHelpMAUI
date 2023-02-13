using AmadonBlazorLibrary.Classes;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                EventsControl.FireFatalError($"Fetch Error, repository= {repo.Head.FriendlyName}: ", ex);
                return false;
            }
        }


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
                EventsControl.FireFatalError($"Pull Error, repository= {repositoryPath}: ", ex);
                StaticObjects.Logger.Error($"Pull Error, repository= {repositoryPath}: ", ex);
                return false;
            }
        }

        private static void CheckoutProgress(string path, int completedSteps, int totalSteps)
        {
            EventsControl.FireSendMessage($"Checkout progress: {completedSteps} of {totalSteps}");
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
                using Repository localRepo = new Repository(repositoryPath);

                Branch branch = localRepo.Branches.ToList().Find(b => b.CanonicalName == branchName);
                if (branch == null)
                {
                    // Let's get a reference on the remote tracking branch...
                    string trackedBranchName = $"origin/{branchName}";
                    Branch trackedBranch = localRepo.Branches[trackedBranchName];

                    // ...and create a local branch pointing at the same Commit
                    branch = localRepo.CreateBranch(branchName, trackedBranch.Tip);

                    // The local branch is not configured to track anything
                    if (!branch.IsTracking)
                    {
                        // So, let's configure the local branch to track the remote one.
                        Branch updatedBranch = localRepo.Branches.Update(branch, b => b.TrackedBranch = trackedBranch.CanonicalName);
                    }

                }
                CheckoutOptions options = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force, OnCheckoutProgress = CheckoutProgress };
                Branch currentBranch = Commands.Checkout(localRepo, branchName, options);
                return true;
            }
            catch (Exception ex)
            {
                EventsControl.FireFatalError($"Checkout Error, repository= {repositoryPath}, branch= {branchName}: ", ex);
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
                EventsControl.FireFatalError($"Clone Error, repository= {repositoryPath}, sourceUrl= {sourceUrl}: ", ex);
                StaticObjects.Logger.Error($"Clone Error, repository= {repositoryPath}, sourceUrl= {sourceUrl}: ", ex);
                return false;
            }
        }

        public void Test(string repositoryPath, string branchName, string url)
        {
            Directory.CreateDirectory(repositoryPath);
            if (!Repository.IsValid(repositoryPath))
            {
                Clone(url, repositoryPath);
            }

            using Repository localRepo = new Repository(repositoryPath);

            Branch branch= localRepo.Branches.ToList().Find(b => b.CanonicalName == branchName);

            if (branch == null)
            {
                // Let's get a reference on the remote tracking branch...
                string trackedBranchName = $"origin/{branchName}";
                Branch trackedBranch = localRepo.Branches[trackedBranchName];

                // ...and create a local branch pointing at the same Commit
                branch = localRepo.CreateBranch(branchName, trackedBranch.Tip);

                // The local branch is not configured to track anything
                if (branch.IsTracking)
                {
                    return;
                }

                // So, let's configure the local branch to track the remote one.
                Branch updatedBranch = localRepo.Branches.Update(branch, b => b.TrackedBranch = trackedBranch.CanonicalName);
            }

            CheckoutOptions options = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force, OnCheckoutProgress = CheckoutProgress };
            Commands.Checkout(localRepo, branchName, options);


            //Process.Start


            //var remoteBranches = localRepo.Network.Remotes
            //                    .SelectMany(r => localRepo.Branches.Where(b =>
            //                        b.IsRemote &&
            //                        b.CanonicalName == $"refs/remotes/{r.Name}/{branchName}"))
            //                    .ToList();
            //Commands.Checkout(localRepo, remoteBranches[0].Tip.Tree, options, remoteBranches[0].Tip.Tree.Sha);
        }

    }

}
