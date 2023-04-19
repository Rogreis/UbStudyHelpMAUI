﻿using AmadonStandardLib.Classes;
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

        // Output data
        public List<ItemForToc>? TocId1 { get; set; } = null;

        public List<ItemForToc>? TocId2 { get; set; } = null;

        public string TitleTranslation1 { get; set; } = "Primary";
        public string TitleTranslation2 { get; set; } = "Secondary";


    }
}
