using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

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


        public static long multiplicationSequential(string path1, string path2, string path_result, long rows_m1, long columns_m1, char separator)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
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
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private class RowResult
        {
            public string row;
            public long rowNumber;
        }

        private static readonly object syncLock = new object();
        private static List<RowResult> MatrixResult;

        public static long multiplicationParallel(string path1, string path2, string path_result, long rows_m1, long columns_m1, char separator)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            File.Delete(path_result);
            string m1_path = path1;
            string m2_path = transpose(path2, columns_m1, rows_m1, separator);
            MatrixResult = new List<RowResult>();
            Task[] mTasks = new Task[rows_m1];
            for( int i = 0; i < rows_m1; i++)
            {
                int a = i;
                mTasks[a] = new Task(() => executeRowMultiplication(m1_path, m2_path, rows_m1, columns_m1, separator, a));
            }

            foreach (Task t in mTasks)
                t.Start();

            Task.WaitAll(mTasks);


            RowResult[] sortedMatrix = MatrixResult.OrderBy(s => s.rowNumber).ToArray();
            using (StreamWriter sw = File.CreateText(path_result))
            {
                for (int i = 0; i < sortedMatrix.Length; i++)
                {
                    sw.WriteLine(sortedMatrix[i].row);
                }
            }
            File.Delete(m2_path);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private static void executeRowMultiplication(string path1, string path2, long rows_m1, long columns_m1, char separator, int rowIndex_m1)
        {
            RowResult rowResult = new RowResult();
            string matrix1_row = File.ReadLines(path1).Skip(rowIndex_m1).Take(1).First();
            var m1_rowElements = matrix1_row.Split(separator).Select(Int32.Parse).ToArray();
            StringBuilder sbRowResult = new StringBuilder();
            using (StreamReader sr2 = File.OpenText(path2))
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
                    sbRowResult.Append(mr_element);
                    sbRowResult.Append(separator);
                }
            }
            sbRowResult.Length -= 1;
            rowResult.row = sbRowResult.ToString();
            rowResult.rowNumber = rowIndex_m1;
            lock (syncLock) {
                MatrixResult.Add(rowResult);
            } 
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
