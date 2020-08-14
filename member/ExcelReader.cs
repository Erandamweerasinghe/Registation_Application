using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace member
{
    public class ExcelReader
    {
        public ExcelReader(string aPath)
        {
            FileName = aPath;
                Application ExcelApp = new Application();
                WorkBook = (_Workbook)(ExcelApp.Workbooks.Open(aPath));
                WorkSheet = (_Worksheet)(WorkBook.ActiveSheet);
        }
        public string Read(int aRow,int aColumn)
        {
            object o = WorkSheet.Cells[aRow, aColumn].Value;
            return o == null ? "" : o.ToString();
        }
     
        public void Close()
        {
            WorkBook.Save();
            WorkBook.Close();
        }
        private string FileName = "";
        private _Workbook WorkBook;
        private _Worksheet WorkSheet;
        private int Row = START_ROW;
        private int Col = START_COL;
        /* Const */
        private const int START_ROW = 4;
        private const int START_COL = 1;

    }
}
