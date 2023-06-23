using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Used to persist data from several app locations
    /// </summary>
    [Serializable]
    public class GenericData
    {
        public string HighlightedText { get; set; } = "";
    }
}
