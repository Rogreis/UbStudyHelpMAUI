using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.Classes
{
    public class PaperTextFormatted
    {
        public TOC_Entry? Entry { get; set; } = null;

        public List<Paragraph>? leftParagraphs { get; set; } = null;

        public List<Paragraph>? rightParagraphs { get; set; } = null;

        public List<Paragraph>? middleParagraphs { get; set; } = null;

        public List<Paragraph>? compareParagraphs { get; set; } = null;
    }
}
