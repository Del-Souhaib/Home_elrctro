using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Home_elrctro
{
    public partial class Form1 : Form
    {

        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = "server=localhost;uid=root;pwd=;database=c#_electro";
        MySqlCommand cmd;
        MySqlDataReader dr;
        private string wherei_m = "all";
        private string selected_zone = "all";
        public Form1()
        {
            InitializeComponent();
        }

        private void guna2Button1_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("1");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            displaydata();
        }
        private void displaydata()
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();

                conn.ConnectionString = myConnectionString;
                conn.Open();

                dataGridView1.DataSource = null;
                MySqlDataAdapter adapter1 = new MySqlDataAdapter("SELECT o.id,o.nom,z.nom,o.description" +
                    " ,o.etat,o.etat2 FROM `objet` o inner join `zone` z on z.id=o.zone_id"
                 , conn);
                DataTable dt3 = new DataTable();

                adapter1.Fill(dt3);
                dataGridView1.DataSource = dt3;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].HeaderText = "Nom";
                dataGridView1.Columns[2].HeaderText = "Zone nom";
                dataGridView1.Columns[3].HeaderText = "Description";
                dataGridView1.Columns[4].HeaderText = "Etat 1";
                dataGridView1.Columns[5].HeaderText = "Etat 2";

                /*   DataGridViewButtonColumn buttoncolumn = new DataGridViewButtonColumn();
                   {
                       buttoncolumn.Name = "button1";
                       buttoncolumn.HeaderText = "Etat 1";
                       buttoncolumn.Text = "Connecte";
                       buttoncolumn.UseColumnTextForButtonValue = true; //dont forget this line
                       this.dataGridView1.Columns.Add(buttoncolumn);
                   }
                   DataGridViewButtonColumn buttoncolumn2 = new DataGridViewButtonColumn();
                   {
                       buttoncolumn2.Name = "button2";
                       buttoncolumn2.HeaderText = "Etat 2";
                       buttoncolumn2.Text = "Demarrer";
                       buttoncolumn2.UseColumnTextForButtonValue = true; //dont forget this line
                       this.dataGridView1.Columns.Add(buttoncolumn2);

                   }*/


                /*  MySqlCommand cmd0 = new MySqlCommand("SELECT id,etat,etat2 FROM `objet` ", conn);
                  MySqlDataReader reader0 = cmd0.ExecuteReader();
                  int count0 = 0;
                  while (reader0.Read())
                  {
                      if (reader0.GetString("etat") == "connecte")
                      {
                          dataGridView1.Rows[count0].Cells[4].Value = "deconnecte";
                      }
                      else
                      {
                          dataGridView1.Rows[count0].Cells[4].Value = "connecte";
                      }
                      if (reader0.GetString("etat2") == "demarrer")
                      {
                          //  eteindre
                          dataGridView1.Rows[count0].Cells[5].Value = "eteindre";
                      }
                      else
                      {
                          dataGridView1.Rows[count0].Cells[5].Value = "demarrer";
                      }
                      this.dataGridView1.UpdateCellValue(4, count0);
                      this.dataGridView1.UpdateCellValue(5, count0);

                  }

                  reader0.Close();*/
                //   dataGridView1.Columns[5].HeaderText = "Action";

                MySqlCommand cmd = new MySqlCommand("SELECT * from zone", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, int> comboSource = new Dictionary<string, int>();
                while (reader.Read())
                {
                    int id = reader.GetInt16("id");
                    string name = reader.GetString("nom");

                    comboSource.Add(name, id);
                }
                guna2ComboBox1.DataSource = new BindingSource(comboSource, null);
                guna2ComboBox1.DisplayMember = "key";
                guna2ComboBox1.ValueMember = "Value";
                reader.Close();



                MySqlCommand cmd2 = new MySqlCommand("SELECT x,y,z.id as zone_id, o.id as machine_id,o.nom as name1,z.nom,o.description,o.etat" +
                    "  FROM `objet` o inner join `zone` z on z.id=o.zone_id", conn);
                MySqlDataReader reader2 = cmd2.ExecuteReader();


                area1.Controls.Clear();
                area2.Controls.Clear();
                area3.Controls.Clear();
                area4.Controls.Clear();
                area5.Controls.Clear();

                while (reader2.Read())
                {
                    Color color;
                    if (reader2.GetString("etat").Equals("connecte"))
                    {
                        color = Color.FromArgb(68, 177, 81);
                    }
                    else
                    {
                        color = Color.FromArgb(170, 0, 0);
                    }
                    Button button = new Button()
                    {
                        Text = reader2.GetString("name1"),
                        BackColor = color,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Width = 80,
                        Height = 30,
                        Name = "machine_" + reader2.GetInt32("machine_id")


                    };
                    button.LocationChanged += new EventHandler(NewButton_location_changed);
                    button.Click += Objet_Click;

                    button.Location = new Point(reader2.GetInt32("x"), reader2.GetInt32("y"));

                    if (button.Location.Y < 1)
                    {
                        button.Location = new Point(button.Location.X, 1);
                    }
                    if (button.Location.Y > (142 - 30))
                    {
                        button.Location = new Point(button.Location.X, 142 - 30);
                    }
                    if (button.Location.X < 1)
                    {
                        button.Location = new Point(1, button.Location.Y);
                    }
                    if (button.Location.X > (256 - 80))
                    {
                        button.Location = new Point(256 - 80, button.Location.Y);
                    }
                    if (int.Parse(reader2.GetString("zone_id")) == 1)
                    {
                        area1.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 2)
                    {
                        area2.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 3)
                    {
                        area3.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 4)
                    {
                        area4.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 5)
                    {
                        area5.Controls.Add(button);
                    }

                    ControlExtension.Draggable(button, true);

                }


                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void displaydatabyzone(int zoneid)
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();

                conn.ConnectionString = myConnectionString;
                conn.Open();

                dataGridView1.DataSource = null;
                MySqlDataAdapter adapter1 = new MySqlDataAdapter("SELECT o.id,o.nom,z.nom,o.description,o.etat" +
                    " ,o.etat2 FROM `objet` o inner join `zone` z on z.id=o.zone_id where o.zone_id=" + zoneid
                 , conn);

                DataTable dt3 = new DataTable();
                adapter1.Fill(dt3);
                dataGridView1.DataSource = dt3;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].HeaderText = "Nom";
                dataGridView1.Columns[2].HeaderText = "Zone nom";
                dataGridView1.Columns[3].HeaderText = "Description";
                dataGridView1.Columns[4].HeaderText = "Etat 1";
                dataGridView1.Columns[5].HeaderText = "Etat 2";

                //  dataGridView1.Columns[5].HeaderText = "Action";

                /* MySqlCommand cmd2 = new MySqlCommand("SELECT x,y,z.id as zone_id, o.id as machine_id,o.nom as name1,z.nom,o.description,o.etat" +
                     "  FROM `objet` o inner join `zone` z on z.id=o.zone_id", conn);
                 MySqlDataReader reader2 = cmd2.ExecuteReader();*/


                MySqlCommand cmd2 = new MySqlCommand("SELECT x,y,z.id as zone_id, o.id as machine_id,o.nom as name1,z.nom,o.description,o.etat" +
                 "  FROM `objet` o inner join `zone` z on z.id=o.zone_id", conn);
                MySqlDataReader reader2 = cmd2.ExecuteReader();

                /*
                                area1.Controls.Clear();
                                area2.Controls.Clear();
                                area3.Controls.Clear();
                                area4.Controls.Clear();
                                area5.Controls.Clear();

                                while (reader2.Read())
                                {
                                    Color color;
                                    if (reader2.GetString("etat").Equals("connecte"))
                                    {
                                        color = Color.FromArgb(68,177,81);
                                    }
                                    else
                                    {
                                        color = Color.FromArgb(170, 0, 0);
                                    }
                                    Button button = new Button()
                                    {
                                        Text = reader2.GetString("name1"),
                                        BackColor = color,
                                        ForeColor = Color.White,
                                        FlatStyle = FlatStyle.Flat,
                                        Width = 80,
                                        Height = 30,
                                        Name = "machine_" + reader2.GetInt32("machine_id")


                                    };
                                    button.LocationChanged += new EventHandler(NewButton_location_changed);
                                    button.Click += Objet_Click;

                                    button.Location = new Point(reader2.GetInt32("x"), reader2.GetInt32("y"));

                                    if (button.Location.Y < 1)
                                    {
                                        button.Location = new Point(button.Location.X, 1);
                                    }
                                    if (button.Location.Y > (142 - 30))
                                    {
                                        button.Location = new Point(button.Location.X, 142 - 30);
                                    }
                                    if (button.Location.X < 1)
                                    {
                                        button.Location = new Point(1, button.Location.Y);
                                    }
                                    if (button.Location.X > (256 - 80))
                                    {
                                        button.Location = new Point(256 - 80, button.Location.Y);
                                    }
                                    if (int.Parse(reader2.GetString("zone_id")) == 1)
                                    {
                                        area1.Controls.Add(button);
                                    }
                                    else if (int.Parse(reader2.GetString("zone_id")) == 2)
                                    {
                                        area2.Controls.Add(button);
                                    }
                                    else if (int.Parse(reader2.GetString("zone_id")) == 3)
                                    {
                                        area3.Controls.Add(button);
                                    }
                                    else if (int.Parse(reader2.GetString("zone_id")) == 4)
                                    {
                                        area4.Controls.Add(button);
                                    }
                                    else if (int.Parse(reader2.GetString("zone_id")) == 5)
                                    {
                                        area5.Controls.Add(button);
                                    }

                                    ControlExtension.Draggable(button, true);

                                }
                */
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Objet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            String id = btn.Name.Replace("machine_", "");
            selected_zone = id;

            get_object_data(id);
        }
        private void get_object_data(String id)
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();

                conn.ConnectionString = myConnectionString;
                conn.Open();

                dataGridView1.DataSource = null;
                MySqlDataAdapter adapter1 = new MySqlDataAdapter("SELECT o.id,o.nom,z.nom,o.description,o.etat" +
                    " ,o.etat2 FROM `objet` o inner join `zone` z on z.id=o.zone_id where o.id=" + id
                 , conn);

                DataTable dt3 = new DataTable();
                adapter1.Fill(dt3);
                dataGridView1.DataSource = dt3;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].HeaderText = "Nom";
                dataGridView1.Columns[2].HeaderText = "Zone nom";
                dataGridView1.Columns[3].HeaderText = "Description";
                dataGridView1.Columns[4].HeaderText = "Etat 1";
                dataGridView1.Columns[5].HeaderText = "Etat 2";


                /*
                MySqlCommand cmd2 = new MySqlCommand("SELECT x,y,z.id as zone_id, o.id as machine_id,o.nom as name1,z.nom,o.description,o.etat" +
                 "  FROM `objet` o inner join `zone` z on z.id=o.zone_id", conn);
                MySqlDataReader reader2 = cmd2.ExecuteReader();


                area1.Controls.Clear();
                area2.Controls.Clear();
                area3.Controls.Clear();
                area4.Controls.Clear();
                area5.Controls.Clear();

                while (reader2.Read())
                {
                    Color color;
                    if (reader2.GetString("etat").Equals("connecte"))
                    {
                        color = Color.FromArgb(68, 177, 81);
                    }
                    else
                    {
                        color = Color.FromArgb(170, 0, 0);
                    }
                    Button button = new Button()
                    {
                        Text = reader2.GetString("name1"),
                        BackColor = color,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Width = 80,
                        Height = 30,
                        Name = "machine_" + reader2.GetInt32("machine_id")


                    };
                    button.LocationChanged += new EventHandler(NewButton_location_changed);
                    button.Click += Objet_Click;

                    button.Location = new Point(reader2.GetInt32("x"), reader2.GetInt32("y"));

                    if (button.Location.Y < 1)
                    {
                        button.Location = new Point(button.Location.X, 1);
                    }
                    if (button.Location.Y > (142 - 30))
                    {
                        button.Location = new Point(button.Location.X, 142 - 30);
                    }
                    if (button.Location.X < 1)
                    {
                        button.Location = new Point(1, button.Location.Y);
                    }
                    if (button.Location.X > (256 - 80))
                    {
                        button.Location = new Point(256 - 80, button.Location.Y);
                    }
                    if (int.Parse(reader2.GetString("zone_id")) == 1)
                    {
                        area1.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 2)
                    {
                        area2.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 3)
                    {
                        area3.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 4)
                    {
                        area4.Controls.Add(button);
                    }
                    else if (int.Parse(reader2.GetString("zone_id")) == 5)
                    {
                        area5.Controls.Add(button);
                    }

                    ControlExtension.Draggable(button, true);

                }*/
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void guna2Button1_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("2");

        }

        private void guna2Button1_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("3");

        }

        private void NewButton_location_changed(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Location.Y < 1)
            {
                btn.Location = new Point(btn.Location.X, 1);
            }
            if (btn.Location.Y > (142 - 30))
            {
                btn.Location = new Point(btn.Location.X, 142 - 30);
            }
            if (btn.Location.X < 1)
            {
                btn.Location = new Point(1, btn.Location.Y);
            }
            if (btn.Location.X > (256 - 80))
            {
                btn.Location = new Point(256 - 80, btn.Location.Y);
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                MessageBox.Show("Remplir les informations");
            }
            else
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    String req = "INSERT INTO `objet`(`zone_id`, `nom`, `description`, `etat`) VALUES " +
                        " (@zone_id,@nom,@description,@etat)";
                    cmd = new MySqlCommand(req, conn);
                    cmd.Parameters.AddWithValue("@zone_id", ((KeyValuePair<string, int>)guna2ComboBox1.SelectedItem).Value);
                    cmd.Parameters.AddWithValue("@nom", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                    cmd.Parameters.AddWithValue("@etat", "off");
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    displaydata();
                    guna2TextBox2.Clear();
                    guna2TextBox3.Clear();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }



        private void guna2Button2_Click(object sender, EventArgs e)
        {




            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            conn.Open();

            MySqlCommand cmd2 = new MySqlCommand("SELECT id,zone_id from `objet`", conn);
            MySqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                Control machine;
                int X, Y;
                if (reader2.GetInt32("zone_id") == 1)
                {
                    machine = area1.Controls.Find("machine_" + reader2.GetInt32("id"), true).First();
                }
                else if (reader2.GetInt32("zone_id") == 2)
                {
                    machine = area2.Controls.Find("machine_" + reader2.GetInt32("id"), true).First();
                }
                else if (reader2.GetInt32("zone_id") == 3)
                {
                    machine = area3.Controls.Find("machine_" + reader2.GetInt32("id"), true).First();
                }
                else if (reader2.GetInt32("zone_id") == 4)
                {
                    machine = area4.Controls.Find("machine_" + reader2.GetInt32("id"), true).First();
                }
                else
                {
                    machine = area5.Controls.Find("machine_" + reader2.GetInt32("id"), true).First();
                }
                X = machine.Location.X;
                Y = machine.Location.Y;


                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    String req = "update `objet` set x=@x, y=@y where id=@id";
                    cmd = new MySqlCommand(req, conn);
                    cmd.Parameters.AddWithValue("@x", X);
                    cmd.Parameters.AddWithValue("@y", Y);
                    cmd.Parameters.AddWithValue("@id", reader2.GetInt32("id"));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (selected_zone.Equals("all"))
            {
                displaydata();
            }
            else
            {
                if (selected_zone.Contains("zone_"))
                {
                    displaydatabyzone(int.Parse(selected_zone.Replace("zone_", "")));
                }
                else
                {
                    get_object_data(dataGridView1.Rows[0].Cells[0].Value.ToString());
                }
            }

        }

        private void area1_Paint(object sender, PaintEventArgs e)
        {
            displaydatabyzone(1);
        }

        private void area2_Paint(object sender, PaintEventArgs e)
        {
            displaydatabyzone(2);

        }

        private void area1_Click(object sender, EventArgs e)
        {
            displaydatabyzone(1);
            selected_zone = "zone_1";

        }

        private void area2_Click(object sender, EventArgs e)
        {
            displaydatabyzone(2);
            selected_zone = "zone_2";

        }

        private void area3_Click(object sender, EventArgs e)
        {
            displaydatabyzone(3);
            selected_zone = "zone_3";


        }

        private void area4_Click(object sender, EventArgs e)
        {
            displaydatabyzone(4);
            selected_zone = "zone_4";

        }

        private void area5_Click(object sender, EventArgs e)
        {
            displaydatabyzone(5);
            selected_zone = "zone_5";
        }

        private void guna2Panel1_Click(object sender, EventArgs e)
        {
            selected_zone = "all";
            displaydata();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (dataGridView1.Columns[e.ColumnIndex].HeaderCell.Value == "Etat 1")
            {
                try
                {

                    String req = "update `objet` set `etat`=@etat where id=@id ";
                    cmd = new MySqlCommand(req, conn);
                    if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Equals("connecte"))
                    {
                        cmd.Parameters.AddWithValue("@etat", "deconnecte");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@etat", "connecte");
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else if (dataGridView1.Columns[e.ColumnIndex].HeaderCell.Value == "Etat 2")
            {
                try
                {

                    String req = "update `objet` set `etat2`=@etat where id=@id ";
                    cmd = new MySqlCommand(req, conn);
                    if (dataGridView1.SelectedRows[0].Cells[5].Value.ToString().Equals("demarrer"))
                    {
                        cmd.Parameters.AddWithValue("@etat", "eteindre");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@etat", "demarrer");
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            cmd.Parameters.AddWithValue("@id", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();

            if (selected_zone.Equals("all"))
            {
                displaydata();
            }
            else
            {
                if (selected_zone.Contains("zone_"))
                {
                    displaydatabyzone(int.Parse(selected_zone.Replace("zone_", "")));
                }
                else
                {
                    get_object_data(dataGridView1.Rows[0].Cells[0].Value.ToString());
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            String text = message_dialogue.Text = "vouler vous deconecter tous les appareils?";
            DialogResult result = message_dialogue.Show(text);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    String req = "";
                    if (selected_zone.Equals("all"))
                    {
                        req = "update `objet` set `etat`=@etat";
                        cmd = new MySqlCommand(req, conn);
                        cmd.Parameters.AddWithValue("@etat", "deconnecte");
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        displaydata();
                    }
                    else
                    {
                        if (selected_zone.Contains("zone_"))
                        {
                            req = "update `objet` set `etat`=@etat where zone_id=@zone_id  ";
                            cmd = new MySqlCommand(req, conn);
                            cmd.Parameters.AddWithValue("@zone_id", selected_zone.Replace("zone_", ""));
                            cmd.Parameters.AddWithValue("@etat", "deconnecte");
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            displaydatabyzone(int.Parse(selected_zone.Replace("zone_", "")));


                        }
                        else
                        {
                            req = "update `objet` set `etat`=@etat where id=@id  ";
                            cmd = new MySqlCommand(req, conn);
                            cmd.Parameters.AddWithValue("@id", selected_zone);
                            cmd.Parameters.AddWithValue("@etat", "deconnecte");
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            get_object_data(dataGridView1.Rows[0].Cells[0].Value.ToString());


                        }
                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Zones_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            String text = message_dialogue.Text = "vouler vous Eteindre tous les appareils?";
            DialogResult result = message_dialogue.Show(text);
            if (result == DialogResult.Yes)
            {

                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    String req = "";
                    if (selected_zone.Equals("all"))
                    {
                        req = "update `objet` set `etat2`=@etat";
                        cmd = new MySqlCommand(req, conn);
                        cmd.Parameters.AddWithValue("@etat", "eteindre");
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        displaydata();

                    }
                    else
                    {
                        if (selected_zone.Contains("zone_"))
                        {
                            req = "update `objet` set `etat2`=@etat where zone_id=@zone_id  ";
                            cmd = new MySqlCommand(req, conn);
                            cmd.Parameters.AddWithValue("@zone_id", selected_zone.Replace("zone_", ""));
                            cmd.Parameters.AddWithValue("@etat", "eteindre");
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            displaydatabyzone(int.Parse(selected_zone.Replace("zone_", "")));


                        }
                        else
                        {
                            req = "update `objet` set `etat2`=@etat where id=@id  ";
                            cmd = new MySqlCommand(req, conn);
                            cmd.Parameters.AddWithValue("@id", selected_zone);
                            cmd.Parameters.AddWithValue("@etat", "eteindre");
                            cmd.ExecuteNonQuery();
                            conn.Close();

                            get_object_data(dataGridView1.Rows[0].Cells[0].Value.ToString());

                        }
                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
