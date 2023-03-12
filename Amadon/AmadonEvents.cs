using Amadon.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon
{
    internal delegate void LeftColumnControl(string controlName);

    internal static class AmadonEvents
    {
        public static event LeftColumnControl OnLeftColumnControl;

        public static void LeftColumnControl(string controlName)
        {
            OnLeftColumnControl?.Invoke(controlName);
        }
    }
}
