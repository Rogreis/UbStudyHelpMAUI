using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon.Models
{
    /// <summary>
    /// Treeview node data
    /// </summary>
    [Serializable]
    public class XamlItem
    {
        public string Key { get; set; }
        public int ItemId { get; set; }
    }
}
