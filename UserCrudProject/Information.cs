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
    public partial class Information : Form
    {
        public static string proUpdate;
        public static string qtyUpdate;
        public static int rowNum;
        private static string prodSelect;
        private static string qty;

        public static DataGridView pGrid = new DataGridView();

        Crud crud = new Crud();
        public Information()
        {
            InitializeComponent();
            loadTable();
            loadChart();
            pGrid = dgvTablica;
            bUpdate.Enabled = false;
            bDelete.Enabled = false;
        }



        public void loadTable()
        {
            crud.Read(dgvTablica, LoginWin.us);
            dgvTablica.Columns[0].HeaderText = "User";
            dgvTablica.Columns[1].HeaderText = "Products";
            dgvTablica.Columns[2].HeaderText = "Qty.";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(numUpDown.Value != 0)
            {
                qty = numUpDown.Value.ToString();

                crud.Insert(dgvTablica, prodSelect, qty);
                loadTable();


                if (!(chart1.Series == null))
                {
                    chart1.Series["Products"].Points.AddXY(prodSelect, qty);
                }
            cbProducts.SelectedIndex = -1;
            numUpDown.Value = 0;
            }
        }

        private void Information_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dgvTablica_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvTablica = sender as DataGridView;
            DataGridViewRow row = dgvTablica.Rows[e.RowIndex];
            if (dgvTablica == null)
            { return; }
            else
            {
                proUpdate = row.Cells["items"].Value.ToString();
                qtyUpdate = row.Cells["number"].Value.ToString();

                rowNum = e.RowIndex;
                label4.Text = proUpdate;
                numUpDownUpdate.Value = Convert.ToInt32(qtyUpdate);

                if (label4.Text != null || numUpDownUpdate != null)
                {
                    bDelete.Enabled = true;
                    bUpdate.Enabled = true;
                }
            }
      
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            crud.Update(label4.Text, label4.Text, numUpDownUpdate.Value.ToString(), pGrid);
            loadTable();
            chart1.Series["Products"].Points.Clear();

            crud.getArray.Clear();
            crud.getArray1.Clear();
            loadChart();

            bUpdate.Enabled = false;
            bDelete.Enabled = false;
            label4.Text = "";
            numUpDownUpdate.Value = 0;
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            crud.Delete(dgvTablica, proUpdate, qtyUpdate);
            loadTable();
            //ključno
            if (!(chart1.Series == null))
            {
                chart1.Series["Products"].Points.RemoveAt(rowNum);
            }
            bUpdate.Enabled = false;
            bDelete.Enabled = false;
            
        }

        public void loadChart() 
        {
 
            int br = crud.chartRead();
            
            int count = 0;

            

            while(!(count == br))
            {
                this.chart1.Series["Products"].Points.AddXY(crud.getArray1[count], crud.getArray[count]);
                count++;
            }       
        }


        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProducts.SelectedItem != null)
            {
                prodSelect = cbProducts.SelectedItem.ToString();

                foreach (DataGridViewRow row in dgvTablica.Rows)
                {
                    foreach (DataGridViewCell cel in row.Cells)
                    {
                        if (!(cel.Value == null))
                        {
                            string valuee = cel.Value.ToString();
                            if (valuee == prodSelect)
                            {
                                cbProducts.SelectedIndex = -1;


                                MessageBox.Show("Products allrdy exist plz update or chose new product.");

                            }
                            //MessageBox.Show(valuee);
                        }

                    }
                }
            }
            //MessageBox.Show("završio");
            //MessageBox.Show("odabrali ste: " + prodSelect);
        }

    }
}
