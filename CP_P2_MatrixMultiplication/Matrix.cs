using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CP_P2_MatrixMultiplication
{
    class Matrix
    {

        public static void createMatrixFile(string path, long rows, long columns, char separator)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    Random rnd = new Random();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            int number = rnd.Next(1001);
                            sb.Append(number);
                            sb.Append(separator);
                        }
                        if (sb.Length > 0)
                            sb.Length -= 1;
                        sw.WriteLine(sb);
                        sb.Clear();
                    }
                }
            }
        }


        public static void multiplicationSequential(string path1, string path2, long rows_m1, long columns_m1, char separator)
        {
            string m1_path = path1;
            string m2_path = transpose(path2, columns_m1, rows_m1, separator);

            //using (StreamReader sr1 = File.OpenText(path1), sr2 = File.OpenText(path2))
            //{

            //}
        }

        /*private static string transpose (string path_source, long rows, long columns, char separator)
        {
            string path_temp1 = @"C:\Users\Saul Chavez\Documents\Matrix_temp1.txt";
            string path_temp2 = @"C:\Users\Saul Chavez\Documents\Matrix_temp2.txt";

            int strBufferSize = 500;
            string[] strBuffer = new string[strBufferSize];

            bool temp2_empty = true;
            if (!File.Exists(path_temp1) && !File.Exists(path_temp2))
            {
                using (StreamReader sr = File.OpenText(path_source))
                {
                    using (StreamWriter sw1 = File.CreateText(path_temp1), sw2 = File.CreateText(path_temp2))
                    {
                        StreamWriter active_sw = sw1;
                        StreamWriter inactive_sw = sw2;
                        string row_source = "";
                        int strBufferIndex = 0;
                        while ((row_source = sr.ReadLine()) != null)
                        {
                            if (strBufferIndex >= strBufferSize)
                            {
                                List<string> copiedFile = new List<string>();
                                if (!temp2_empty)
                                {

                                }
                                Array.Clear(strBuffer, 0, strBuffer.Length);
                                strBufferIndex = 0;
                            }
                            strBuffer[strBufferIndex++] = row_source;
                        }
                    }
                }
            }
            return path_temp1;
        }*/

        private static string transpose(string path_source, long rows, long columns, char separator)
        {
            string path_temp = @"C:\Users\Saul Chavez\Documents\Matrix_temp1.txt";
            if (!File.Exists(path_temp))
            {
                using (StreamReader sr = File.OpenText(path_source))
                {
                    //Inizialite StringBuilder array
                    StringBuilder[] sbTempFile = new StringBuilder[columns];
                    for (int i = 0; i < columns; i++)
                        sbTempFile[i] = new StringBuilder();

                    //Retrieving lines of source file. Each line will append a new number to each StringBuilder array element
                    string source_line = "";
                    while ((source_line = sr.ReadLine()) != null)
                    {
                        string[] split_Line = source_line.Split(separator);
                        for (int i = 0; i < columns; i++)
                        {
                            sbTempFile[i].Append(split_Line[i]);
                            sbTempFile[i].Append(separator);
                        }
                    }

                    using (StreamWriter sw = File.CreateText(path_temp))
                    {
                        for (int i = 0; i < columns; i++)
                        {
                            sw.WriteLine(sbTempFile[i]);
                            sbTempFile[i] = null;
                        }
                    }
                }
            }
            else
            {
                path_temp = null;
            }
            return path_temp;
        }
    }
}
