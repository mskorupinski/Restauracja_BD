﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        dane logowanie;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Zaloguj_Sie_Click(object sender, RoutedEventArgs e)
        {
             logowanie = new dane();
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

            public string Login
            {
                get {
                    return login;
                }
            }
            public string Hasło
            {
                get
                {
                    return hasło;
                }
            }
            public void Ustaw_login(string l) { login = l; }
            public void Ustaw_haslo(string h) { hasło = h; }
        }

        private void Przycisk_Menu_Szukaj_Click(object sender, RoutedEventArgs e)
        {
            
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
            SqlConnection sqlConn = new SqlConnection("Server = LocalHost;Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            


            if (comboBox.Text == "")
            {
                MessageBox.Show("Wybierz menu", "Błąd");
            }
            else
            {
                sqlCmd.CommandText = "Select Id_menu from Menu where Opis = '" + comboBox.Text + "'";
                int id_menu = (int)sqlCmd.ExecuteScalar();
                sqlCmd.CommandText = "insert into Produkt (Opis_produktu, Cena_produktu, Id_menu) Values('" + Tekst_Menu_Opis.Text + "'," + Int32.Parse(Tekst_Menu_Cena.Text) + "," + id_menu + ");";
                sqlCmd.ExecuteReader();
                sqlConn.Close();

                wyswietlMenu();


            }

           
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (comboBox.Items.Count > 0) comboBox.Items.Clear();
                
            string Sql = "select Opis from Menu";
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-PMVJTLEG\\SQLSTANDARD;Network Library=dbmssocn;Initial Catalog = RESTAURACJA; User ID = marcin ;Password = marcin;");
            conn.Open();
            SqlCommand cmd = new SqlCommand(Sql, conn);
            SqlDataReader DR = cmd.ExecuteReader();

            while (DR.Read())
            {
                comboBox.Items.Add(DR[0]);

            }

            conn.Close();
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            wyswietlMenu();
        }

        public void wyswietlMenu()
        {
            SqlConnection sqlConn = new SqlConnection("Server = LocalHost;Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            SqlDataReader dataReader = null;

            Tekst_Menu_Cena.Text = "";
            Tekst_Menu_Opis.Text = "";
            sqlCmd.CommandText = "select Opis_produktu as Opis , Cena_produktu as Cena  from Produkt as p join  Menu as m on p.Id_menu=m.Id_menu where m.Opis='" + comboBox.Text + "'";

            dataReader = sqlCmd.ExecuteReader();

            if (dataReader != null)
            {
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                Dane_Menu.ItemsSource = dt.DefaultView;

                dataReader.Close();
            }

            sqlConn.Close();
        }
    }
}