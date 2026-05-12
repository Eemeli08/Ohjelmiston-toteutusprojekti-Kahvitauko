using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kahvitauko_ohjelma.View
{
    public partial class settingsform : Form
    {
        public settingsform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingsform_Load(object sender, EventArgs e)
        {

        }
    } 
}