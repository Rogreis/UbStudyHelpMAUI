using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using System.Collections.Generic;

namespace AmadonStandardLib.InterchangeData
{


    /// <summary>
    /// Data passing class for TOC (in/out)
    /// </summary>
    public class TOCdata : InterchangeDataBase
    {
        // Input data
        public short TranslationId1 { get; set; } = -1;

        public short TranslationId2 { get; set; } = -1;

        public short TranslationId3 { get; set; } = -1;

        // Output data
        public List<ItemForToc>? Toc { get; set; } = null;

 

     }
}
