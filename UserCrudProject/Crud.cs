using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace UserCrudProject
{
    public class Crud
    {

        private const String UID = "skrpan";
        private const String SERVER = "db4free.net";
        //private const int PORT = 3306;
        private const String DATABASE = "dbskrpan";
        private const String PASSWORD = "konjina01";
        private static MySqlConnection con;
        private static int parentId;

        private ArrayList array = new ArrayList();
        private ArrayList array1 = new ArrayList();





        public static void init()
        {
            //kreiranje konekcijskog stringa
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.UserID = UID;
            builder.Server = SERVER;
            builder.Database = DATABASE;
            builder.Password = PASSWORD;


            String conString = builder.ToString();
            builder = null;
            con = new MySqlConnection(conString);

        }

        public Boolean Login(string user, string pass)
        {

            MySqlCommand com = new MySqlCommand("SELECT * FROM dbskrpan.korisnici WHERE BINARY user = '"
                + user + "' AND pass = '"
                + pass + "';", con);
            try
            {
                con.Open();
                MySqlDataReader reader = com.ExecuteReader();

                int count = 0;
                while (reader.Read())
                    count += 1;

                if (count == 1)
                {
                    parentId = reader.GetInt32("id");

                    MessageBox.Show("Uspješno povezani! Na id: " + parentId);
                    reader.Close();
                    con.Close();
                    return true;
                }
                else if (user == "" || pass == "")
                {
                    MessageBox.Show("User name ili pass su vam nepopunjeni!");
                    reader.Close();
                    con.Close();
                    return false;
                }
                else
                {
                    MessageBox.Show("Krivi username ili pass!");
                    reader.Close();
                    con.Close();
                    return false;
                }



            }
            catch (Exception) { MessageBox.Show("Korisnik ne postoji!"); return false; }
        }
        public void Read(DataGridView view, string user)
        {
            MySqlCommand com = new MySqlCommand("SELECT korisnici.user, namirnice.items, namirnice.number "
            + "FROM korisnici INNER JOIN namirnice on korisnici.id = namirnice.parent_id and korisnici.user = '"
            + user + "';", con);

            /*MySqlCommand com = new MySqlCommand("SELECT * FROM dbskrpan.namirnice", con);
            con.Open();*/

            try
            {
                con.Open();
                MySqlDataAdapter myAdapter = new MySqlDataAdapter();
                myAdapter.SelectCommand = com;

                DataTable dsTable = new DataTable();
                myAdapter.Fill(dsTable);


                BindingSource bSource = new BindingSource();
                bSource.DataSource = dsTable;

                view.DataSource = bSource;


                myAdapter.Update(dsTable);

                con.Close();
            }
            catch (Exception) { MessageBox.Show("Ne može dohvatit bazu i ispisat u tablicu"); }
        }

        public void Insert(DataGridView view, string product, string qty)
        {
            MySqlCommand com = new MySqlCommand("INSERT INTO dbskrpan.namirnice (`parent_id`, `items`, `number`) VALUES ("
                + parentId.ToString() + ", '" + product + "', " + qty + ");", con);

            try
            {
                con.Open();
                MySqlDataAdapter myAdapter = new MySqlDataAdapter();
                myAdapter.SelectCommand = com;

                DataTable dsTable = new DataTable();
                myAdapter.Fill(dsTable);


                BindingSource bSource = new BindingSource();
                bSource.DataSource = dsTable;
                view.DataSource = bSource;
                myAdapter.Update(dsTable);

                con.Close();
            }
            catch (Exception e) { MessageBox.Show("Error:" + e); }
        }
        public void Update(string preItem, string item, string number, DataGridView view)
        {
            MySqlCommand com = new MySqlCommand("UPDATE dbskrpan.namirnice SET items = '"
                + item + "', number = " + number + " WHERE items = '" + preItem + "';", con);

            try
            {
                con.Open();

                MySqlDataAdapter myAdapter = new MySqlDataAdapter();
                myAdapter.SelectCommand = com;

                DataTable dsTable = new DataTable();
                myAdapter.Fill(dsTable);

                BindingSource bSource = new BindingSource();
                bSource.DataSource = dsTable;
                view.DataSource = bSource;
                myAdapter.Update(dsTable);

                con.Close();
            }
            catch (Exception) { MessageBox.Show("Ne može UPDATE!!!"); }
        }
        public void Delete(DataGridView view, string item, string number)
        {
            MySqlCommand com = new MySqlCommand("DELETE FROM dbskrpan.namirnice WHERE items = '" + item + "' AND number = '"
                + number + "';", con);

            try
            {
                con.Open();
                MySqlDataAdapter myAdapter = new MySqlDataAdapter();
                myAdapter.SelectCommand = com;

                DataTable dsTable = new DataTable();
                myAdapter.Fill(dsTable);

                BindingSource bSource = new BindingSource();
                bSource.DataSource = dsTable;
                view.DataSource = bSource;
                myAdapter.Update(dsTable);

                con.Close();
            }
            catch (Exception) { MessageBox.Show("Ne može DELETE!"); }
        }

        //public int QTY_NUM { get; set; }

        //public string ITEM { get; set; }



        public int chartRead()
        {
            MySqlCommand com = new MySqlCommand("SELECT * FROM dbskrpan.namirnice WHERE parent_id = " + parentId + ";", con);

            try
            {
                con.Open();

                MySqlDataReader myReader = com.ExecuteReader();

                int count = 0;
                while (myReader.Read())
                {
                    array1.Add(myReader.GetString("items"));
                    array.Add(myReader.GetInt32("number"));

                    count++;
                }

                con.Close();
                return count;
            }
            catch (Exception) { MessageBox.Show("Ne mogu isčitati iz baze u CHART!!!"); return 1; }
        }

        public ArrayList getArray
        {
            get { return array; }
        }
        public ArrayList getArray1
        {
            get { return array1; }
        }

    }

}
