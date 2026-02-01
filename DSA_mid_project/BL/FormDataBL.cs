using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project
{
    public class FormDataBL
    {
        public string FormType { get; set; }
        public string FormTitle { get; set; }
        public DateTime Timestamp { get; set; }
        public Form FormInstance { get; set; } // Storing actual form

        public FormDataBL(Form form)
        {
            FormType = form.GetType().Name;
            FormTitle = form.Text;
            Timestamp = DateTime.Now;
            FormInstance = form; // Storing instance
        }
    }
}
