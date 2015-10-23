using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using HtmlAgilityPack;

namespace DWHConsole
{
    class Program
    {
        static void Main(string[] args)
        {
                HtmlWeb htmlWeb = new HtmlWeb();
                int count = 0;
                for (int i = 524001; i <= 525000; i++)
                {
                    string dataFromFirstTable = ""; // to store data from first table of the webpage
                    string dataFromSecondTable = ""; // to store data from second table of the webpage
                    string[] fullData = { };
                    HtmlDocument htmlDocument = htmlWeb.Load("https://fbise.edu.pk/linkrollno-hssc-11?roll_no=" + i);
                    IEnumerable<HtmlNode> tableTags = htmlDocument.DocumentNode.Descendants("table").Where(table => table.Attributes.Contains("align"));
                    foreach (var tableTag in tableTags)
                    {
                        count++; // using this count variable to concatenate the data of two tables
                        IEnumerable<HtmlNode> trTags = tableTag.Descendants("tr");
                        foreach (var tr in trTags)
                        {
                            //Inserting this slash on purpose, so that I can use something to split
                            //there are no newline characters in the data that is extracted
                            //from the web page
                            tr.PrependChild(HtmlNode.CreateNode("<td>/</td>"));
                        }
                        if (count == 1)
                        {
                            dataFromFirstTable = tableTag.InnerText;
                        }
                        else if (count == 2)
                        {
                            dataFromSecondTable = tableTag.InnerText;
                            //merging the data from tabels
                            dataFromFirstTable += dataFromSecondTable;
                            count = 0;
                        }
                    }
                    fullData = dataFromFirstTable.Split('/');
                    List<string> tempList = new List<string>(fullData);
                    //Doing some cleaning by hard coding removing unwanted columns
                    if (tempList.Count > 16)
                    {
                        tempList.RemoveAt(0);
                        tempList.RemoveAt(4);
                        tempList.RemoveAt(16);
                        tempList.RemoveAt(16);
                        string[] finalArray = tempList.ToArray();



                        /////////////////////////////////////////////////
                        //getting data into our class
                        StudentsResult sr = new StudentsResult();
                        sr.RollNo = finalArray[0].Substring(0, 8).Trim();
                        sr.RollNoValue = finalArray[0].Substring(8, 7).Trim();
                        sr.RegistrationNo = finalArray[0].Substring(15, 14);
                        sr.RegistrationNoValue = finalArray[0].Substring(29, 10).Trim();
                        sr.CandidateName = finalArray[1].Substring(0, 14);
                        sr.CandidateNameValue = finalArray[1].Substring(15).Trim();
                        sr.FatherName = finalArray[2].Substring(0, 11);
                        sr.FatherNameValue = finalArray[2].Substring(12).Trim();
                        sr.Result = finalArray[3].Substring(0, 6);
                        if (finalArray[3].Contains("COMP."))
                        {
                            sr.ResultValue = "COMP.".Trim();
                        }
                        else if (finalArray[3].Contains("FAIL"))
                        {
                            sr.ResultValue = "FAIL".Trim();
                        }
                        else if (finalArray[3].Contains("Pass") || finalArray[3].Contains("PASS"))
                        {
                            finalArray[3] = finalArray[3].Remove(0, 29);
                            int tempIndex = finalArray[3].IndexOf('&');
                            finalArray[3] = finalArray[3].Remove(tempIndex);
                            finalArray[3] = finalArray[3].Trim();
                            sr.ResultValue = finalArray[3];
                        }

                        //////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroforFirstSubject = finalArray[4].IndexOf('0');
                        if (indexOfZeroforFirstSubject == -1)
                        {
                            finalArray[4] = finalArray[4].Replace("ABS", "0BS");
                            indexOfZeroforFirstSubject = finalArray[4].IndexOf('0');
                        }
                        sr.Subject1 = finalArray[4].Substring(1, indexOfZeroforFirstSubject - 1);
                        sr.Subject1Value = finalArray[4].Substring(indexOfZeroforFirstSubject);
                        if (sr.Subject1Value.Contains("ABS") || sr.Subject1Value.Contains("0BS"))
                        {
                            sr.Subject1Value = "0";
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////

                        int indexOfZeroForSecondSubject = finalArray[5].IndexOf('0');
                        if (indexOfZeroForSecondSubject == -1)
                        {
                            finalArray[5] = finalArray[5].Replace("ABS", "0BS");
                            indexOfZeroForSecondSubject = finalArray[5].IndexOf('0');
                            if (!finalArray[5].Contains('0'))
                            {
                                finalArray[5] = finalArray[5].Insert(finalArray[5].Length - 2,"0");
                                indexOfZeroForSecondSubject = finalArray[5].IndexOf('0');
                            }
                        }
                        sr.Subject2 = finalArray[5].Substring(1, indexOfZeroForSecondSubject - 1);
                        sr.Subject2Value = finalArray[5].Substring(indexOfZeroForSecondSubject);
                        if (sr.Subject2Value.Contains("ABS") || sr.Subject2Value.Contains("0BS"))
                        {
                            sr.Subject2Value = "0";
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForThirdSubject = finalArray[6].IndexOf('0');
                        if (indexOfZeroForThirdSubject == -1)
                        {
                            finalArray[6] = finalArray[6].Replace("ABS", "0BS");
                            indexOfZeroForThirdSubject = finalArray[6].IndexOf('0');
                        }
                        sr.Subject3 = finalArray[6].Substring(1, indexOfZeroForThirdSubject - 1);
                        sr.Subject3Value = finalArray[6].Substring(indexOfZeroForThirdSubject);
                        if (sr.Subject3Value.Contains("ABS") || sr.Subject3Value.Contains("0BS"))
                        {
                            sr.Subject3Value = "0";
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForFourthSubject = finalArray[7].IndexOf('0');
                        if (indexOfZeroForFourthSubject == -1)
                        {
                            finalArray[7] = finalArray[7].Replace("ABS", "0BS");
                            indexOfZeroForFourthSubject = finalArray[7].IndexOf('0');
                        }
                        sr.Subject4 = finalArray[7].Substring(1, indexOfZeroForFourthSubject - 1);
                        sr.Subject4Value = finalArray[7].Substring(indexOfZeroForFourthSubject);
                        if (sr.Subject4Value.Contains("ABS") || sr.Subject4Value.Contains("0BS"))
                        {
                            sr.Subject4Value = "0";
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForFifthSubject = finalArray[8].IndexOf('0');
                        if (indexOfZeroForFifthSubject == -1)
                        {
                            finalArray[8] = finalArray[8].Replace("ABS", "0BS");
                            indexOfZeroForFifthSubject = finalArray[8].IndexOf('0');
                        }
                        sr.Subject5 = finalArray[8].Substring(1, indexOfZeroForFifthSubject - 1);
                        sr.Subject5Value = finalArray[8].Substring(indexOfZeroForFifthSubject);
                        if (sr.Subject5Value.Contains("ABS") || sr.Subject5Value.Contains("0BS"))
                        {
                            sr.Subject5Value = "0";
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForSixthSubject = finalArray[9].IndexOf('0');
                        if (indexOfZeroForSixthSubject == -1)
                        {
                            finalArray[9] = finalArray[9].Replace("ABS", "0BS");
                            indexOfZeroForSixthSubject = finalArray[9].IndexOf('0');
                        }
                        sr.Subject6 = finalArray[9].Substring(1, indexOfZeroForSixthSubject - 1);
                        sr.Subject6Value = finalArray[9].Substring(indexOfZeroForSixthSubject);
                        if (sr.Subject6Value.Contains("ABS") || sr.Subject6Value.Contains("0BS"))
                        {
                            sr.Subject6Value = "0";
                        }
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        int indexOfZeroForSeventhSubject = finalArray[10].IndexOf('0');
                        if (indexOfZeroForSeventhSubject == -1)
                        {
                            finalArray[10] = finalArray[10].Replace("ABS", "0BS");
                            indexOfZeroForSeventhSubject = finalArray[10].IndexOf('0');
                        }
                        sr.Subject7 = finalArray[10].Substring(1, indexOfZeroForSeventhSubject - 1);
                        string tempHoldData7 = finalArray[10].Substring(indexOfZeroForSeventhSubject);
                        if (tempHoldData7.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData7.Contains("ABS") || tempHoldData7.Contains("0BS"))
                            {
                                sr.Subject7Value = "0";
                                sr.Subject7Practical = "0";
                            }
                            else
                            {
                                sr.Subject7Value = tempHoldData7;
                                sr.Subject7Practical = "0";
                                //assing the value of marx
                                //because of Value cannot be null exception
                                //Subject7Practical is string property, if any record is not fetched 
                                //into it, then null will be assigned. in db its column is "int"
                                //thats why query gives exception
                            }
                        }
                        else if (tempHoldData7.Length == 6)
                        {
                            sr.Subject7Value = tempHoldData7.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject7Practical = tempHoldData7.Substring(3);
                            if (sr.Subject7Value.Contains("ABS") || sr.Subject7Value.Contains("0BS"))
                            {
                                sr.Subject7Value = "0";
                            }
                            if (sr.Subject7Practical.Contains("ABS") || sr.Subject7Practical.Contains("0BS"))
                            {
                                sr.Subject7Practical = "0";
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForEightSubject = finalArray[11].IndexOf('0');
                        if (indexOfZeroForEightSubject == -1) //it contains ABS in theory subject
                        {
                            if (finalArray[11].Contains("ABS"))
                            {
                                finalArray[11] = finalArray[11].Replace("ABS", "0BS");
                                indexOfZeroForEightSubject = finalArray[11].IndexOf('0');
                            }
                        }
                        sr.Subject8 = finalArray[11].Substring(1, indexOfZeroForEightSubject - 1);
                        string tempHoldData8 = finalArray[11].Substring(indexOfZeroForEightSubject);
                        if (tempHoldData8.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData8.Contains("ABS") || tempHoldData8.Contains("0BS"))
                            {
                                sr.Subject8Value = "0";
                                sr.Subject8Practical = "0";
                            }
                            else
                            {
                                sr.Subject8Value = tempHoldData8;//assing the value of marx
                                sr.Subject8Practical = "0";
                            }
                        }
                        else if (tempHoldData8.Length == 6)
                        {
                            sr.Subject8Value = tempHoldData8.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject8Practical = tempHoldData8.Substring(3);//assigning last three digits to practical
                            if (sr.Subject8Value.Contains("ABS") || sr.Subject8Value.Contains("0BS"))
                            {
                                sr.Subject8Value = "0";
                            }
                            if (sr.Subject8Practical.Contains("ABS") || sr.Subject8Practical.Contains("0BS"))
                            {
                                sr.Subject8Practical = "0";
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForNineSubject = finalArray[12].IndexOf('0');
                        if (indexOfZeroForNineSubject == -1)
                        {
                            finalArray[12] = finalArray[12].Replace("ABS", "0BS");
                            indexOfZeroForNineSubject = finalArray[12].IndexOf('0');
                        }
                        sr.Subject9 = finalArray[12].Substring(1, indexOfZeroForNineSubject - 1);
                        string tempHoldData9 = finalArray[12].Substring(indexOfZeroForNineSubject);
                        if (tempHoldData9.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData9.Contains("ABS") || tempHoldData9.Contains("0BS"))
                            {
                                sr.Subject9Value = "0";
                                sr.Subject9Practical = "0";
                            }
                            else
                            {
                                sr.Subject9Value = tempHoldData9;//assing the value of marx
                                sr.Subject9Practical = "0";
                            }
                        }
                        else if (tempHoldData9.Length == 6)
                        {
                            sr.Subject9Value = tempHoldData9.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject9Practical = tempHoldData9.Substring(3);//assigning last three digits to practical
                            if (sr.Subject9Value.Contains("ABS") || sr.Subject9Value.Contains("0BS"))
                            {
                                sr.Subject9Value = "0";
                            }
                            if (sr.Subject9Practical.Contains("ABS") || sr.Subject9Practical.Contains("0BS"))
                            {
                                sr.Subject9Practical = "0";
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////
                        finalArray[13] = finalArray[13].Remove(0, 2);
                        int indexOfZeroForTenSubject = finalArray[13].IndexOf('0');
                        if (indexOfZeroForTenSubject == -1)
                        {
                            finalArray[13] = finalArray[13].Replace("ABS", "0BS");
                            indexOfZeroForTenSubject = finalArray[13].IndexOf('0');
                        }
                        sr.Subject10 = finalArray[13].Substring(0, indexOfZeroForTenSubject);
                        string tempHoldData10 = finalArray[13].Substring(indexOfZeroForTenSubject);
                        if (tempHoldData10.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData10.Contains("ABS") || tempHoldData10.Contains("0BS"))
                            {
                                sr.Subject10Value = "0";
                                sr.Subject10Practical = "0";
                            }
                            else
                            {
                                sr.Subject10Value = tempHoldData10;//assing the value of marx
                                sr.Subject10Practical = "0";
                            }
                        }
                        else if (tempHoldData10.Length == 6)
                        {
                            sr.Subject10Value = tempHoldData10.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject10Practical = tempHoldData10.Substring(3);//assigning last three digits to practical
                            if (sr.Subject10Value.Contains("ABS") || sr.Subject10Value.Contains("0BS"))
                            {
                                sr.Subject10Value = "0";
                                sr.Subject10Practical = "0";
                            }
                            if (sr.Subject10Practical.Contains("ABS") || sr.Subject10Practical.Contains("0BS"))
                            {
                                sr.Subject10Practical = "0";
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForElevenSubject = finalArray[14].IndexOf('0');
                        if (indexOfZeroForElevenSubject == -1)
                        {
                            finalArray[14] = finalArray[14].Replace("ABS", "0BS");
                            indexOfZeroForElevenSubject = finalArray[14].IndexOf('0');
                        }
                        sr.Subject11 = finalArray[14].Substring(2, indexOfZeroForElevenSubject - 2);
                        string tempHoldData11 = finalArray[14].Substring(indexOfZeroForElevenSubject);
                        if (tempHoldData11.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData11.Contains("ABS") || tempHoldData11.Contains("0BS"))
                            {
                                sr.Subject11Value = "0";
                                sr.Subject11Practical = "0";
                            }
                            else
                            {
                                sr.Subject11Value = tempHoldData11;//assing the value of marx
                                sr.Subject11Practical = "0";
                            }
                        }
                        else if (tempHoldData11.Length == 6)
                        {
                            sr.Subject11Value = tempHoldData11.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject11Practical = tempHoldData11.Substring(3);//assigning last three digits to practical
                            if (sr.Subject11Value.Contains("ABS") || sr.Subject11Value.Contains("0BS"))
                            {
                                sr.Subject11Value = "0";
                                sr.Subject11Practical = "0";
                            }
                            if (sr.Subject11Practical.Contains("ABS") || sr.Subject11Practical.Contains("0BS"))
                            {
                                sr.Subject11Practical = "0";
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////
                        int indexOfZeroForTwelveSubject = finalArray[15].IndexOf('0');
                        if (indexOfZeroForTwelveSubject == -1)
                        {
                            finalArray[15] = finalArray[15].Replace("ABS", "0BS");
                            indexOfZeroForTwelveSubject = finalArray[15].IndexOf('0');
                        }
                        sr.Subject12 = finalArray[15].Substring(2, indexOfZeroForTwelveSubject - 2);
                        string tempHoldData12 = finalArray[15].Substring(indexOfZeroForTwelveSubject);
                        if (tempHoldData12.Length == 3) //subject without practical will have 3 digits
                        {
                            if (tempHoldData12.Contains("ABS") || tempHoldData12.Contains("0BS"))
                            {
                                sr.Subject12Value = "0";
                                sr.Subject12Practical = "0";
                            }
                            else
                            {
                                sr.Subject12Value = tempHoldData12;//assing the value of marx
                                sr.Subject12Practical = "0";
                            }
                        }
                        else if (tempHoldData12.Length == 6)
                        {
                            sr.Subject12Value = tempHoldData12.Substring(0, 3);//assigning first three digit to marx
                            sr.Subject12Practical = tempHoldData12.Substring(3);//assigning last three digits to practical
                            if (sr.Subject12Value.Contains("ABS") || sr.Subject12Value.Contains("0BS"))
                            {
                                sr.Subject12Value = "0";
                                sr.Subject12Practical = "0";
                            }
                            if (sr.Subject12Practical.Contains("ABS") || sr.Subject12Practical.Contains("0BS"))
                            {
                                sr.Subject12Practical = "0";
                            }
                        }
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ///why did you not loop through this data??
                        ///because, subjects are 1 to 12,
                        ///in the substring function we have to specify index from where to start
                        ///1 to 9 , substring(1,indexofzero) but when it changes to 10th subject function will be substring(2,index of zero)
                        DatabaseConnectivity dc = new DatabaseConnectivity();
                        dc.Insert(sr);
                        //dc.PrintToScreen(sr);
                    }
                }
        }
    }
}
