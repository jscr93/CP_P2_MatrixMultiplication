using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace CP_P2_MatrixMultiplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string path1 = @"C:\Users\Saul Chavez\Documents\Matrix_1.txt";
            string path2 = @"C:\Users\Saul Chavez\Documents\Matrix_2.txt";
            string path_result = @"C:\Users\Saul Chavez\Documents\Matrix_Result.txt";
            char separator = ',';
            Matrix.multiplicationSequential(path1, path2, path_result, 6, 3, separator);
        }

        private void btnNewMatrices_Click(object sender, RoutedEventArgs e)
        {
            string path1 = @"C:\Users\Saul Chavez\Documents\Matrix_1.txt";
            string path2 = @"C:\Users\Saul Chavez\Documents\Matrix_2.txt";
            char separator = ',';
            DisableAll();
            File.Delete(path1);
            File.Delete(path2);
            Matrix.createMatrixFile(path1, 6, 3, separator,2);
            Matrix.createMatrixFile(path2, 3, 6, separator,3);
            EnableAll();
        }

        private void DisableAll()
        {
            btnNewMatrices.IsEnabled = false;
        }

        private void EnableAll()
        {
            btnNewMatrices.IsEnabled = true;
        }
    }
}
