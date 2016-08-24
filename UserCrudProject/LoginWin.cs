using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserCrudProject
{
    

    public partial class LoginWin : Form
    {
        Crud crud = new Crud();
        public static string us;
        
        public LoginWin()
        {
            InitializeComponent();
            if (cbShowHide.Checked)
                tbPass.UseSystemPasswordChar = false;
            Crud.init();
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            string user = tbUser.Text;
            us = user;
            string pass = tbPass.Text;
            Boolean check = crud.Login(user, pass);
            if (check) 
            {
                this.Hide();
                Information info = new Information();
                info.ShowDialog();
            }
                
        }

        private void cbShowHide_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowHide.Checked)
            {
                tbPass.UseSystemPasswordChar = false;
            }
            else 
            {
                tbPass.UseSystemPasswordChar = true;
            }
        }

    }
}
