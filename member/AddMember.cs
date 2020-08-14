using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace member
{
    public partial class AddMember : Form
    {
        bool Isnew = false;
        public AddMember() //Add new member
        {
            InitializeComponent();
            Isnew = true;
        }
        public AddMember(string indexNumber) : this() //Add new member
        {
            txtAddNum.Text = indexNumber;
            txtAddNum.ReadOnly = true;
        }
        public AddMember(DataGridViewRow row)  //Edit member
        {
            InitializeComponent();
            txtAddName.Text = row.Cells[2].Value.ToString();
            txtAddNum.Text = row.Cells[1].Value.ToString();
            cmbLevel.Text = row.Cells[3].Value.ToString();
            cmbDP.Text = row.Cells[4].Value.ToString();
            cmbFood.Text = row.Cells[5].Value.ToString();
            txtAddEmail.Text = row.Cells[6].Value.ToString();
        }
        private void btnAddExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddSubmit_Click(object sender, EventArgs e)
        {
            Member m = null;
            string cmd = "";
           
                m = new Member()  //m.<????> wenuwta
                {
                    Attendence = 0,
                    Degree_Program = cmbDP.Text, 
                    Email = txtAddEmail.Text,
                    Food_References = cmbFood.Text,
                    Gender = Member.GenderType.Male,
                    Index_Number = txtAddNum.Text,
                    Level = cmbLevel.Text,
                    Name = txtAddName.Text,
                    Reg_Date = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                };
                
           if(Isnew)
                cmd = m.GetInsertText();
           else
                cmd = m.GetUpdateText();

            

            MySqlCommand command = new MySqlCommand(cmd, Connection.GetDBConnection());
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Sucsesfull job...");
                DialogResult = DialogResult.OK;   
                 
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to Update data..!!");
            }

        }

        private void AddMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                btnAddSubmit_Click(null, null);
        }
    }
}
