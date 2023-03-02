using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    public class TOCdata : InterchangeDataBase
    {
        public short TranslationId { get; set; } =-1;

        // Output data
        public TOC_Table? Toc { get; set; } = null;
    }
}
