using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.tb
{
    public class Folder
    {
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public int? ParentFolderID { get; set; }
    }
}
