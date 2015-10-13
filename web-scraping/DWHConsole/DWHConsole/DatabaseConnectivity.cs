using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DWHConsole
{
    public class DatabaseConnectivity
    {
        public void Insert(StudentsResult sr)
        {
            string connectionString = @"DATA SOURCE=localhost\SQLEXPRESS;INITIAL CATALOG=federalboard; INTEGRATED SECURITY=true";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = string.Format("INSERT INTO Results VALUES('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},'{9}',{10},'{11}',{12},'{13}',{14},'{15}',{16},'{17}',{18},{19},'{20}',{21},{22},'{23}',{24},{25},'{26}',{27},{28},'{29}',{30},{31},'{32}',{33},{34})", sr.RollNoValue, sr.RegistrationNoValue, sr.CandidateNameValue, sr.FatherNameValue, sr.ResultValue, sr.Subject1, int.Parse(sr.Subject1Value), sr.Subject2, int.Parse(sr.Subject2Value), sr.Subject3, int.Parse(sr.Subject3Value), sr.Subject4, int.Parse(sr.Subject4Value), sr.Subject5, int.Parse(sr.Subject5Value), sr.Subject6, int.Parse(sr.Subject6Value), sr.Subject7, int.Parse(sr.Subject7Value), sr.Subject7Practical, sr.Subject8, int.Parse(sr.Subject8Value), sr.Subject8Practical, sr.Subject9, int.Parse(sr.Subject9Value), sr.Subject9Practical, sr.Subject10, int.Parse(sr.Subject10Value),sr.Subject10Practical, sr.Subject11, int.Parse(sr.Subject11Value),sr.Subject11Practical, sr.Subject12, int.Parse(sr.Subject12Value),sr.Subject12Practical);
                SqlCommand cmd = new SqlCommand(sql, conn);
               cmd.ExecuteNonQuery();
            }
            // 0 to 4 basic inputs
            //subjects starts from 5th placeholder, subject name and then following placeholder
            //for its marks
            //subject starts from 17th placeholder have marx and practicals consectively
        }
        public void PrintToScreen(StudentsResult sr)
        {
            
            string sql = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34}", sr.RollNoValue, sr.RegistrationNoValue, sr.CandidateNameValue, sr.FatherNameValue, sr.ResultValue, sr.Subject1, int.Parse(sr.Subject1Value), sr.Subject2, int.Parse(sr.Subject2Value), sr.Subject3, int.Parse(sr.Subject3Value), sr.Subject4, int.Parse(sr.Subject4Value), sr.Subject5, int.Parse(sr.Subject5Value), sr.Subject6, int.Parse(sr.Subject6Value), sr.Subject7, int.Parse(sr.Subject7Value), sr.Subject7Practical, sr.Subject8, int.Parse(sr.Subject8Value), sr.Subject8Practical, sr.Subject9, int.Parse(sr.Subject9Value), sr.Subject9Practical, sr.Subject10, int.Parse(sr.Subject10Value), sr.Subject10Practical, sr.Subject11, int.Parse(sr.Subject11Value), sr.Subject11Practical, sr.Subject12, int.Parse(sr.Subject12Value), sr.Subject12Practical);
            Console.WriteLine(sql);
        }
    }
}
