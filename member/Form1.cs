using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace member
{
    public partial class Form1 : Form
    {
        private string SelectedID = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddMember add = new AddMember();
            add.ShowDialog();
            Member.LoadFromDataBase();
            SelectedID = add.txtAddNum.Text;
            UpdateList(null,null);
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            string cmd = "UPDATE `member` SET Attendence=1 where Index_Number='"+txtIndex.Text+"'";
            Member m = Member.GetMember(txtIndex.Text);
            MySqlCommand command = new MySqlCommand(cmd, Connection.GetDBConnection());
            if (m.Index_Number.Length == 0)
            {
                m.Index_Number = txtIndex.Text;
                if (new AddMember(txtIndex.Text).ShowDialog() == DialogResult.OK)
                    new Popup_Menu(m).ShowDialog();
            }
            else
            {
                new Popup_Menu(m).ShowDialog();
            }
            Member.LoadFromDataBase();
            SelectedID = txtIndex.Text;
            UpdateList(null,null);
            txtIndex.Clear();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Member.LoadFromDataBase();
            UpdateList(null,null);
        }

        public void UpdateList(object sender, EventArgs e)
        {
            //if (sender != null)
            //    Text = sender.ToString();
            int x = 1;
            int total_Present = 0;
            int absent_Cont = 0;
            int BCS_Level1 = 0;
            int BCS_Level2 = 0;
            int BCS_Level3 = 0;
            int BSc_Level2 = 0;
            int BSc_Level3 = 0;
            dgv_member.Rows.Clear();

            foreach (Member m in Data.members)
            {
                if (IsMatched(m, txt_Serach.Text.ToLower()))
                {
                    dgv_member.Rows.Add(x, m.Index_Number, m.Name, m.Level,
                               m.Degree_Program, m.Food_References, m.Email, 
                               m.Attendence,m.Reg_Date);
                    if (m.Attendence == 0)
                        dgv_member.Rows[x - 1].DefaultCellStyle = Data.CS_Emptystorage;
                    if (m.Index_Number.ToLower() == SelectedID.ToLower())
                        dgv_member.Rows[x - 1].Selected = true;
                    if (m.Attendence == 1)
                        total_Present++;

                    if (m.Attendence == 0)
                        absent_Cont++;

                    if (m.Level == "Level 1" && m.Degree_Program == "BCS")
                        BCS_Level1++;

                    if (m.Level == "Level 2" && m.Degree_Program == "BCS")
                        BCS_Level2++;

                    if (m.Level == "Level 2" && m.Degree_Program == "BSc")
                        BSc_Level2++;

                    if (m.Level == "Level 3" && m.Degree_Program == "BSc")
                        BSc_Level3++;

                    if (m.Level == "Level 3" && m.Degree_Program == "BCS")
                        BCS_Level3++;
                    x++;
                }
            }
            txtTotal.Text = total_Present.ToString();
            txtAbsernt.Text = absent_Cont.ToString();
            txtRegCont.Text = (total_Present + absent_Cont).ToString();

            txt_Level1_BCS.Text = BCS_Level1.ToString();
            txt_Level2_BCS.Text = BCS_Level2.ToString();
            txt_Level3_BCS.Text = BCS_Level3.ToString();
            txt_Level2_CS.Text = BSc_Level2.ToString();
            txt_Level3_CS.Text = BSc_Level3.ToString();
           
            
        }

        private bool IsMatched(Member m,string key)
        {
            if (!chkPresent.Checked && m.Attendence == 1)
                return false;
            if (!chkAbsent.Checked && m.Attendence == 0)
                return false;
            if (txt_Serach.Text.Trim().Length == 0) //trim()-->>mokak hari txt 1 ka mulai agai space  remove krnawa
                return true;
            if (chkID.Checked && m.Index_Number.ToLower().Contains(key))
                return true;
            if (chk_Name.Checked && m.Name.ToLower().Contains(key))
                return true;
            return false;
        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelReader rdr = new ExcelReader(@"F:\Billing_System\Mark_Attendence\Event Registration (Responses) (1).xlsx");
            for (int i = 2; i <= 189; i++)
            {
                Member m = new Member();  //create member object
                m.Reg_Date =
                    DateTime.Parse(rdr.Read(i, 1)).ToString("yyyy/MM/dd HH:mm:ss");
                m.Index_Number = rdr.Read(i, 2);
                m.Name = rdr.Read(i, 3);
                m.Level = rdr.Read(i, 4);
                m.Degree_Program = rdr.Read(i, 5);
                m.Food_References = rdr.Read(i, 6);
                m.Email = rdr.Read(i, 7);
                m.oldIndex = rdr.Read(i,8);

                //get member ditails in excelsheet and fill  MySql database
                MySqlCommand command = new MySqlCommand(m.GetInsertText(), Connection.GetDBConnection());
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Text = i.ToString();
                Update();
            }
            MessageBox.Show("Successfull insert....");
            rdr.Close();  //close ExcelReader
        }

        private void dgv_member_SelectionChanged(object sender, EventArgs e)
        { 
            if (dgv_member.SelectedRows.Count > 0)
            {

                txt_Index.Text = dgv_member.SelectedRows[0].Cells[1].Value.ToString();
                txt_Name.Text = dgv_member.SelectedRows[0].Cells[2].Value.ToString();
                txt_Level.Text = dgv_member.SelectedRows[0].Cells[3].Value.ToString();
                txt_Degree.Text = dgv_member.SelectedRows[0].Cells[4].Value.ToString();
                txt_Food.Text = dgv_member.SelectedRows[0].Cells[5].Value.ToString();
                txt_email.Text = dgv_member.SelectedRows[0].Cells[6].Value.ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddMember add = new AddMember(dgv_member.SelectedRows[0]);
            if (add.ShowDialog() == DialogResult.OK) //submit kranakota dialog result 1 ok nm
            {
                Member.LoadFromDataBase();
                UpdateList(null, null);
            }

           //.Text=dgv_member.SelectedRows.Cells[]
        }


      
        private void button2_Click(object sender, EventArgs e)
        {
            //UpdateList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            DialogResult dialog = MessageBox.Show("Remove Member", "Are you sure", MessageBoxButtons.YesNo);


            if (dialog == DialogResult.Yes)
            {
                string selectIndex = dgv_member.SelectedRows[0].Cells[1].Value.ToString();
                string cmd = "DELETE FROM `member` " +
                    "WHERE Index_Number='"+selectIndex+"';";
                MySqlCommand command = new MySqlCommand(cmd, Connection.GetDBConnection());
                command.ExecuteNonQuery();
                MessageBox.Show("Sucesfull  Removed...");

                Member.LoadFromDataBase();
                UpdateList(null, null);
            }
        }

        private void txtIndex_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData != Keys.Enter)
            {
                if (!txtIndex.Focused && !txt_Serach.Focused)
                {
                    txtIndex.Focus();
                }
            }
            else if (txtIndex.Focused)
                btn_Enter_Click(null, null);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txtIndex_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
