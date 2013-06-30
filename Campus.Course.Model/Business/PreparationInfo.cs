using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    public class PreparationInfo
    {
        public int PrepId { get; set; }

        public string PrepName { get; set; }

        public string PrepContent { get; set; }

        public string TeachNo { get; set; }

        public int MeteiralId { get; set; } // >0有效

        public string MeteiralName { get; set; }

        public string MeteiralType { get; set; }

        public string MeteiralPath { get; set; }
    }
}
