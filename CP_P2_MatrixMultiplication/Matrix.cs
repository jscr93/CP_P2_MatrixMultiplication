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

        public static void createMatrixFile(string path, long rows, long columns, char separator, int randomSeed)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    Random rnd = new Random(randomSeed);
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


        public static void multiplicationSequential(string path1, string path2, string path_result, long rows_m1, long columns_m1, char separator)
        {
            string m1_path = path1;
            string m2_path = transpose(path2, columns_m1, rows_m1, separator);

            File.Delete(path_result);            
            using (StreamReader sr1 = File.OpenText(m1_path))
            {
                using (StreamWriter sw = File.CreateText(path_result))
                {
                    for (int i = 0; i < rows_m1; i++)
                    {
                        string matrix1_row = sr1.ReadLine();
                        
                        var m1_rowElements = matrix1_row.Split(separator).Select(Int32.Parse).ToArray();
                        
                        StringBuilder mr_row = new StringBuilder();
                        using (StreamReader sr2 = File.OpenText(m2_path))
                        {
                            for (int i_m2 = 0; i_m2 < rows_m1; i_m2++)
                            {
                                string matrix2_row = sr2.ReadLine();
                                var m2_rowElements = matrix2_row.Split(separator).Select(Int32.Parse).ToArray();
                                int mr_element = 0;
                                for (int j = 0; j < columns_m1; j++)
                                {
                                    mr_element += m1_rowElements[j] * m2_rowElements[j];
                                }
                                mr_row.Append(mr_element);
                                mr_row.Append(separator);
                            }
                        }
                        mr_row.Length -= 1;
                        sw.WriteLine(mr_row);
                    }
                }
            }
            File.Delete(m2_path);
        }

        private static string transpose(string path_source, long rows, long columns, char separator)
        {
            string path_temp = @"C:\CP_P2\Matrix_temp1.txt";
            File.Delete(path_temp);
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
                        sbTempFile[i].Length -= 1;
                        sw.WriteLine(sbTempFile[i]);
                        sbTempFile[i] = null;
                    }
                }
            }
            return path_temp;
        }
    }
}
