﻿using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Diagnostics;
using UbStandardObjects;

namespace AmadonStandardLib.Classes
{
    internal class LogMAUI : Log
    {
        private bool _logIniciado = false;

        public string PathLog { get; set; }

        public readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Start(string pathLog, bool append = false, bool reStart = false)
        {
            if (!append)
            {
                if (File.Exists(pathLog))
                {
                    File.Delete(pathLog);
                }
            }
            PathLog = pathLog;
            SetupLof4Net(append, reStart);
            Enable();
        }

        private void SetupLof4Net(bool append = false, bool reStart = false)
        {
            if (_logIniciado)
                return;

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            //patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ConversionPattern = "%date [%thread] %-5level %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = append;
            roller.File = PathLog;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            //MemoryAppender memory = new MemoryAppender();
            //memory.ActivateOptions();
            //hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

#if DEBUG
            if (!_logIniciado)
            {
                LogManager.GetRepository().Threshold = Level.All;
                _logIniciado = true;
                if (reStart)
                {
                    Logger.Info("UbStudyHelp log re-started");
                }
                else
                {
                    Logger.Info("UbStudyHelp log Started");
                }
            }
#else
            LogManager.GetRepository().Threshold = Level.Off;
#endif

        }


        private void Start(string pathLog, bool append = false)
        {
            if (!append)
            {
                if (File.Exists(pathLog))
                {
                    try
                    {
                        File.Delete(pathLog);
                    }
                    catch // Error ignored
                    {
                    }
                }
            }
            PathLog = pathLog;
            SetupLof4Net();
            Enable();
        }

        public void Disable()
        {
            LogManager.GetRepository().Threshold = Level.Off;
            //((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Off;
            //((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
        }

        public void Enable()
        {
            //SetupLof4Net(true);
            LogManager.GetRepository().Threshold = Level.All;
            //((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.All;
            //((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
        }



        public override void NonFatalError(string message)
        {
            Logger.Error(message);
            EventsControl.FireError(message);
        }


        public override void Initialize(string path, bool append = false)
        {
            //Logger.Error(message);
            Start(path, append);
        }

        public override void Clear()
        {
        }

        public override void Warn(string message)
        {
            Logger.Warn(message);
        }

        public override void Info(string message)
        {
            Logger.Info(message);
        }

        public override void Error(string message)
        {
            Logger.Error(message);
        }

        public override void Error(string message, Exception ex)
        {
            Logger.Error(message, ex);
        }


        public override void FatalErrorAsync(string message)
        {
            // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
            Logger.Error(message);
            EventsControl.FireFatalError(message);
        }



        /// <summary>
        /// Shoe log file
        /// </summary>
        public override void ShowLog()
        {
            //Close();
            //Process.Start("notepad.exe", PathLog);

            //using (var fs = new FileStream(PathLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (var sr = new StreamReader(fs, Encoding.Default))
            //{
            //    string text = sr.ReadToEnd();
            //}

        }

        /// <summary>
        /// Enable the log again
        /// </summary>
        public void Show()
        {
            LogManager.GetRepository().Shutdown();
            _logIniciado = false;
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
