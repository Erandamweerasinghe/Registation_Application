using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace member
{
    class Data
    {
        public static List<Member> members = new List<Member>();

        public static char ToChar(int KeyValue)
        {
            if (IsDigit(KeyValue))
                return (char)(KeyValue - Limits.KEY_0 + (int)'0');
            else if (IsNumDigit(KeyValue))
                return (char)(KeyValue - Limits.KEY_NUM_0 + (int)'0');
            else if (IsLetter(KeyValue))
                return (char)(KeyValue - Limits.KEY_A + 65);
            return 'A';
        }
        public static bool IsLetter(int KeyValue)
        {
            return IsBetween(Limits.KEY_A, KeyValue, Limits.KEY_Z);
        }
        public static bool IsDigit(int KeyValue)
        {
            return IsBetween(Limits.KEY_0, KeyValue, Limits.KEY_9);
        }
        public static bool IsNumDigit(int KeyValue)
        {
            return IsBetween(Limits.KEY_NUM_0, KeyValue, Limits.KEY_NUM_9);
        }
        public static bool IsBetween(int low, int val, int high)
        {
            return low <= val && val <= high;
        }
        public static DataGridViewCellStyle CS_Emptystorage
        {
            get
            {
                DataGridViewCellStyle cs = null;
                if (cs == null)
                {
                    cs = new DataGridViewCellStyle();   
                    cs.BackColor = Color.Yellow;
                    cs.SelectionForeColor = Color.Black;
                }
                return cs;
            }
        }
    }
}
