using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Data passing class for TOC (in/out)
    /// </summary>
    public class TOCdata : InterchangeDataBase
    {
        // Input data
        public short TranslationId1 { get; set; } =-1;

        public short TranslationId2 { get; set; } = -1;

        public short TranslationId3 { get; set; } = -1;

        // Output data
        public TOC_Table? Toc { get; set; } = null;
    }
}
