using AmadonStandardLib.Classes;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.IO.Enumeration;

namespace AmadonStandardLib.Helpers
{
    public class Logger
    {
        private bool _logIniciado = false;

        public const string FileName= "Amadon.log";

        public static string? PathLog { get; set; }

        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            //AmadonLogAppender amadonLogAppender= new AmadonLogAppender();
            //amadonLogAppender.Layout = patternLayout;
            //hierarchy.Root.AddAppender(amadonLogAppender);

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
                    _logger.Info("UbStudyHelp log re-started");
                }
                else
                {
                    _logger.Info("UbStudyHelp log Started");
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

        private void FatalError(string message)
        {
            // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
            _logger.Error(message);
            LibraryEventsControl.FireFatalError(message);
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



        public void NonFatalError(string message)
        {
            _logger.Error(message);
            LibraryEventsControl.FireError(message);
        }


        public void Initialize(string path, bool append = false)
        {
            //_logger.Error(message);
            Start(path, append);
        }

        public void Clear()
        {
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void IsNull(object obj, string message)
        {
            if (obj == null)
                FatalError(message);
        }

        public void InInterval(short value, short minValue, short maxValue, string message)
        {
            if (value < minValue || value > maxValue)
                FatalError(message);
        }


        /// <summary>
        /// Returns all current lines on log
        /// </summary>
        public string GetLog()
        {
            Close();
            return File.ReadAllText(PathLog);
        }


        public void Close()
        {
            LogManager.GetRepository().Shutdown();
            _logIniciado = false;
        }
    }
}
