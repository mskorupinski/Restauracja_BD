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
using System.Data.SqlClient;

namespace Restauracja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Zaloguj_Sie_Click(object sender, RoutedEventArgs e)
        {
            dane logowanie = new dane();
            SqlConnection sqlConn = new SqlConnection("Data Source=LAPTOP-PMVJTLEG\\SQLSTANDARD;Network Library=dbmssocn;Initial Catalog = RESTAURACJA; User ID = " + Text_Logowanie.Text + ";Password = " + Text_Haslo.Password + ";");
            try
            {

                // otwórz połączenie:
                sqlConn.Open();
                logowanie.Ustaw_login(Text_Logowanie.ToString());
                logowanie.Ustaw_haslo(Text_Haslo.ToString());

                sqlConn.Close();
                //asgjhasfgjas
                Logowanie.Visibility = Visibility.Hidden;
                Panel.Visibility = Visibility.Visible;
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                MessageBox.Show(se.Message, se.Source);

            }
        }

        class dane
        {
            string login = "";
            string hasło = "";
            public void Ustaw_login(string l) { login = l; }
            public void Ustaw_haslo(string h) { hasło = h; }
        }

 
    }
}
