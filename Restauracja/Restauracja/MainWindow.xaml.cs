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
using System.Data;

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

        private void Przycisk_Menu_Szukaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection("Server = LocalHost;Integrated Security = SSPI; Database = 'Restauracja'");

                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;
                SqlDataReader dataReader = null;
                if (Tekst_Menu_Nr.Text != "")
                {
                    if (Tekst_Menu_Cena.Text != "")
                    {

                        if (Tekst_Menu_Opis.Text != "")
                        {
                            sqlCmd.CommandText = "select * from Produkt where Id_menu=@Nr and Cena_produktu=@Cena and Opis_produktu=@Opis";

                            SqlParameter param1 = new SqlParameter();
                            param1.ParameterName = "@Nr";
                            param1.Value = Tekst_Menu_Nr.Text;

                            SqlParameter param2 = new SqlParameter();
                            param2.ParameterName = "@Cena";
                            param2.Value = Tekst_Menu_Cena.Text;

                            SqlParameter param3 = new SqlParameter();
                            param3.ParameterName = "@Opis";
                            param3.Value = Tekst_Menu_Opis.Text;

                            sqlCmd.Parameters.Add(param1);

                            sqlCmd.Parameters.Add(param2);

                            sqlCmd.Parameters.Add(param3);
                            dataReader = sqlCmd.ExecuteReader();



                        }
                        else
                        {
                            sqlCmd.CommandText = "select * from Produkt where Id_menu=@Nr and Cena_produktu=@Cena";

                            SqlParameter param1 = new SqlParameter();
                            param1.ParameterName = "@Nr";
                            param1.Value = Tekst_Menu_Nr.Text;

                            SqlParameter param2 = new SqlParameter();
                            param2.ParameterName = "@Cena";
                            param2.Value = Tekst_Menu_Cena.Text;

                            sqlCmd.Parameters.Add(param1);

                            sqlCmd.Parameters.Add(param2);

                            dataReader = sqlCmd.ExecuteReader();
                        }

                    }
                    else
                    {
                        if (Tekst_Menu_Opis.Text != "")
                        {
                            sqlCmd.CommandText = "select * from Produkt where Id_menu=@Nr and  Opis_produktu=@Opis";

                            SqlParameter param1 = new SqlParameter();
                            param1.ParameterName = "@Nr";
                            param1.Value = Tekst_Menu_Nr.Text;



                            SqlParameter param3 = new SqlParameter();
                            param3.ParameterName = "@Opis";
                            param3.Value = Tekst_Menu_Opis.Text;

                            sqlCmd.Parameters.Add(param1);


                            sqlCmd.Parameters.Add(param3);
                            dataReader = sqlCmd.ExecuteReader();

                        }
                        else
                        {
                            sqlCmd.CommandText = "select * from Produkt where Id_menu=@Nr ";

                            SqlParameter param1 = new SqlParameter();
                            param1.ParameterName = "@Nr";
                            param1.Value = Tekst_Menu_Nr.Text;



                            sqlCmd.Parameters.Add(param1);


                            dataReader = sqlCmd.ExecuteReader();


                        }
                    }
                }
                else
                {

                    if (Tekst_Menu_Cena.Text != "")
                    {

                        if (Tekst_Menu_Opis.Text != "")
                        {
                            sqlCmd.CommandText = "select * from Produkt where  Cena_produktu=@Cena and Opis_produktu=@Opis";



                            SqlParameter param2 = new SqlParameter();
                            param2.ParameterName = "@Cena";
                            param2.Value = Tekst_Menu_Cena.Text;

                            SqlParameter param3 = new SqlParameter();
                            param3.ParameterName = "@Opis";
                            param3.Value = Tekst_Menu_Opis.Text;



                            sqlCmd.Parameters.Add(param2);

                            sqlCmd.Parameters.Add(param3);
                            dataReader = sqlCmd.ExecuteReader();

                        }
                        else
                        {
                            sqlCmd.CommandText = "select * from Produkt where  Cena_produktu=@Cena ";


                            SqlParameter param2 = new SqlParameter();
                            param2.ParameterName = "@Cena";
                            param2.Value = Tekst_Menu_Cena.Text;



                            sqlCmd.Parameters.Add(param2);

                            dataReader = sqlCmd.ExecuteReader();
                        }

                    }
                    else
                    {
                        if (Tekst_Menu_Opis.Text != "")
                        {
                            sqlCmd.CommandText = "select * from Produkt where  Opis_produktu=@Opis";


                            SqlParameter param3 = new SqlParameter();
                            param3.ParameterName = "@Opis";
                            param3.Value = Tekst_Menu_Opis.Text;


                            sqlCmd.Parameters.Add(param3);
                            dataReader = sqlCmd.ExecuteReader();

                        }
                        else
                        {
                            MessageBox.Show("Musisz podać parametry!", "Błąd");
                        }
                    }


                }
                if (dataReader != null)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dataReader);
                    Dane_Menu.ItemsSource = dt.DefaultView;

                    dataReader.Close();
                }
                sqlConn.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {

                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        private void Tekst_Menu_Nr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Tekst_Menu_Cena_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            if (e.Text[e.Text.Length - 1]=='.')
            {
                e.Handled = false;

            }
        }

        private void Przycisk_Menu_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (Tekst_Menu_Nr.Text != "")
            {
                if (Tekst_Menu_Cena.Text != "")
                {

                    if (Tekst_Menu_Opis.Text == "")
                    {

                        MessageBox.Show("Musisz podać opis!", "Błąd");


                    }
                    else
                    {
                        /*
                        SqlConnection sqlConn=null;
                        try
                        {
                             sqlConn = new SqlConnection("Server = LocalHost; Integrated Security = SSPI; Database = 'Restauracja'");

                            // otwórz połączenie:
                            sqlConn.Open();
                            SqlCommand sqlCmd = new SqlCommand();
                            sqlCmd.CommandText = "INSERT into Produkt (Id_menu, Cena_produktu , Opis_produktu) " +
                   " VALUES ('" + Tekst_Menu_Nr.Text + "', '" + Tekst_Menu_Cena.Text + "', '" + Tekst_Menu_Opis.Text + "');";
                            sqlCmd.Connection = sqlConn;
                            sqlCmd.ExecuteNonQuery();


                            // zamknij połaczenie:
                            sqlConn.Close();
                        }
                        catch
                        {
                            

                            MessageBox.Show("Error to save on database");
                            if (sqlConn != null)
                            {
                                sqlConn.Close();
                            }
                            
                        }*/

                    }


                }
                else
                {
                    if (Tekst_Menu_Opis.Text != "")
                    {
                        MessageBox.Show("Musisz podać cenę!", "Błąd");


                    }
                    else
                    {
                        MessageBox.Show("Musisz podać cenę i opis!", "Błąd");



                    }
                }
            }
            else
            {

                if (Tekst_Menu_Cena.Text != "")
                {

                    if (Tekst_Menu_Opis.Text != "")
                    {


                        MessageBox.Show("Musisz podać numer menu!", "Błąd");


                    }
                    else
                    {
                        MessageBox.Show("Musisz podać numer menu i opis!", "Błąd");

                    }

                }
                else
                {
                    if (Tekst_Menu_Opis.Text != "")
                    {
                        MessageBox.Show("Musisz podać numer menu i cenę!", "Błąd");


                    }
                    else
                    {
                        MessageBox.Show("Musisz podać parametry!", "Błąd");

                    }
                }


            }

        }
    }
}
