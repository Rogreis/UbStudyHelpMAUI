using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon.Models
{
    /// <summary>
    /// Treeview node collection data
    /// </summary>
    [Serializable]
    public class XamlItemGroup
    {
        public List<XamlItemGroup> Children { get; } = new();
        public List<XamlItem> XamlItems { get; } = new();

        public string Name { get; set; }
        public int GroupId { get; set; }
    }
}
