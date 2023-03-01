﻿using Lucene.Net.Store;
using System;

namespace AmadonStandardLib.Helpers
{
    public delegate void SendMessageDelegate(string message);
    public delegate void SendLogMessageDelegate(string message);
    public delegate void SendLogErrorMessageDelegate(string message, Exception ex);


    public class LuceneBase
    {
        public event SendMessageDelegate? SendMessage = null;
        public event SendLogMessageDelegate? SendLogMessage = null;
        public event SendLogErrorMessageDelegate? SendLogErrorMessage = null;

        /// <summary>
        /// Set and used inside search class only
        /// </summary>
        protected string IndexPath { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        protected FSDirectory? luceneIndexDirectory;


        protected void FireSendMessage(string message)
        {
            SendMessage?.Invoke(message);
        }
        protected void FireSendLogMessage(string message)
        {
            SendLogMessage?.Invoke(message);
        }
        protected void FireSendLogErrorMessage(string message, Exception? ex = null)
        {
            SendLogErrorMessage?.Invoke(message, ex);
        }
    }
}
