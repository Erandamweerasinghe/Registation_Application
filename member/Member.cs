using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace member
{
    public class Member
    {
        public Member()
        {
            Reg_Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public string GetInsertText()
        {
            string cmd = string.Format("INSERT INTO `io_2019`.`member` " +
                "(`Index_Number`, `Name`, `Level`, `Degree_Program`, `Food_References`, `Email`,`Attendens`," +
                "`Gender`,`Reg_Date`,`oldIndex`) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}', '{7}','{8}','{9}');"
                , Index_Number,Name,Level,Degree_Program,Food_References,Email,Attendence,
                Gender,Reg_Date,oldIndex);

            return cmd;
        }

        public string GetUpdateText()
        {
            string cmd = string.Format("UPDATE `io_2019`.`member` SET `Name` = '{0}', `Level`='{1}',`Degree_Program`='{2}'," +
                                     "`Food_References`='{3}',`Email`='{4}'," +
                                     "`Attendens`='{5}',`Gender`='{6}',`Reg_Date`='{7}' WHERE `Index_Number`='{8}';",
                                     Name,Level, Degree_Program, 
                                    Food_References, Email, Attendence,Gender,Reg_Date,Index_Number);
            return cmd;
                                     
        }
        public string GetUpdateAttendance()
        {
            string cmd = string.Format("UPDATE `io_2019`.`member` SET `Attendens`='{0}',`Gender`='{1}',`Wtm`='{2}',`Lab`='{3}' " +
                "WHERE `Index_Number`='{4}';", Attendence, Gender, Wtm, Lab, Index_Number);
            return cmd;

        }
        public override string ToString()  //ToString method 1 overid kraa
        {
            return Index_Number + "  |  " + Name;
        }


        public static bool LoadFromDataBase()
        {
            try
            {
                DataTable dataTable = Connection.GetDataTable("SELECT * FROM member Order by `Reg_Date`");
                Data.members = new List<Member>(dataTable.Rows.Count);  //database 1 n load krala  RAM 1 save kra  gannawa

                foreach (DataRow r in dataTable.Rows)
                {
                    Data.members.Add(parse(r));
                }
                return true;
            }
            catch(Exception ex)
            {
               MessageBox.Show("Unble to update Database");
                
            }
            return false;
        }
        public static Member GetMember(string indexNumber)
        {
            try
            {
                DataTable dataTable = Connection.GetDataTable("SELECT * FROM `member` WHERE `Index_Number` = '" 
                    + indexNumber + "'");
                return parse(dataTable.Rows[0]);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Member Not Found");

            }
            return new Member();
        }
        private static Member parse(DataRow r) //dataRow 1 k member kenek widiyata convert kranawa
        {
            Member m = new Member();
            m.Index_Number = r["Index_Number"].ToString();
            m.Name = r["Name"].ToString();
            m.Level = r["Level"].ToString();
            m.Degree_Program = r["Degree_Program"].ToString();
            m.Food_References = r["Food_References"].ToString();
            m.Email = r["Email"].ToString();
            m.Attendence = int.Parse(r["Attendens"].ToString());
            m.Gender =
                (Member.GenderType)Enum.Parse(typeof(Member.GenderType), r["Gender"].ToString());
            m.Reg_Date = r["Reg_Date"].ToString();
            m.Lab = int.Parse(r["Lab"].ToString());
            m.Wtm = int.Parse(r["Wtm"].ToString());
            return m;
        }
        public enum GenderType { Male ,Female };
        public string Index_Number = "";
        public string Name;
        public string Level;
        public string Degree_Program;
        public string Food_References;
        public string Email;
        public int Attendence;
        public string Reg_Date;
        public int Wtm;
        public int Lab;
        public string oldIndex = "";
        public GenderType Gender = GenderType.Male; 

    }
}
