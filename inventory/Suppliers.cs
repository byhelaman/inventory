using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inventory
{
    public partial class Suppliers : Form
    {
        public Suppliers()
        {
            InitializeComponent();

            Connect.CheckConnection();

            DataTable dt = Connect.ReadRecords("suppliers");
            DgvSuppliers.DataSource = dt;

            DgvSuppliers.Columns["supplier_id"].Visible = false;
        }
    }
}
