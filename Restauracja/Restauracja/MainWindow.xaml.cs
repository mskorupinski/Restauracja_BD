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
        private String serwer = "E540";
        private void Zaloguj_Sie_Click(object sender, RoutedEventArgs e)
        {
             logowanie = new dane();
            SqlConnection sqlConn = new SqlConnection("Data Source="+serwer+";Network Library=dbmssocn;Initial Catalog = RESTAURACJA; User ID = " + Text_Logowanie.Text + ";Password = " + Text_Haslo.Password + ";");
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



        private void Tekst_Menu_Nr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Tekst_Menu_Cena_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            if (e.Text[e.Text.Length - 1]==',')
            {
                e.Handled = false;

            }
        }

        private void Przycisk_Menu_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection("Server = "+serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            


            if (comboBox.Text == "")
            {
                MessageBox.Show("Wybierz menu", "Błąd");
            }
            else
            {
                if (Tekst_Menu_Cena.Text != "")
                {
                    sqlCmd.CommandText = "Select Id_menu from Menu where Opis = '" + comboBox.Text + "'";
                    int id_menu = (int)sqlCmd.ExecuteScalar();
                    string result = Tekst_Menu_Cena.Text.Replace(",", ".");
                    
                    string insert= "Select count(*) from Produkt where Opis_produktu = '" + Tekst_Menu_Opis.Text + "' and Cena_produktu='" + result+"'";
                    sqlCmd.CommandText = insert.Replace(",", ".");
                    int liczba_wierszu = (int)sqlCmd.ExecuteScalar();
                    if (liczba_wierszu == 0)
                    {

                        string temp = "insert into Produkt (Opis_produktu, Cena_produktu, Id_menu,Aktualnosc) Values('" + Tekst_Menu_Opis.Text + "','" + result + "'," + id_menu + ", 'aktualny');";

                        sqlCmd.CommandText = temp;
                        sqlCmd.ExecuteReader();
                    }
                    if (liczba_wierszu == 1)
                    {
                      
                        string temp  = "update Produkt set Aktualnosc='aktualny' where Opis_produktu='" + Tekst_Menu_Opis.Text + "' and Cena_produktu= " + result + " and Id_menu=" + id_menu;
                        sqlCmd.CommandText = temp;
                        sqlCmd.ExecuteReader();

                    }
                    sqlConn.Close();

                    wyswietlMenu();

                }
            }

           
        }

        private void Przycisk_Menu_Usun_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;

            DataRowView row = (DataRowView)Dane_Menu.SelectedItem;

            if (row != null)
            {


                string alter = "Select Count(*) from Pozycja_Produkt where Opis ='" + row[0].ToString() + "' and Cena =" + row[1].ToString().Replace(",", ".");
                sqlCmd.CommandText = alter;
                int liczba_wierszu = (int)sqlCmd.ExecuteScalar();
                if (liczba_wierszu == 0)
                {
                    sqlCmd.CommandText = "Delete From Produkt Where Opis_produktu = '" + row[0].ToString() + "' and Cena_produktu = " + row[1].ToString().Replace(",", ".");
                    sqlCmd.ExecuteReader();

                }
                if (liczba_wierszu >= 1)
                {
                    string temp = "update Produkt set Aktualnosc='nieaktualny' where Opis_produktu='" + row[0].ToString() + "' and Cena_produktu= " + row[1].ToString().Replace(",", ".");
                    sqlCmd.CommandText = temp;
                    sqlCmd.ExecuteReader();

                }
                sqlConn.Close();

                wyswietlMenu();

                
            }
        }


     

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (comboBox.Items.Count > 0) comboBox.Items.Clear();
                
            string Sql = "select Opis from Menu";
            SqlConnection conn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");
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
            SqlConnection sqlConn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            SqlDataReader dataReader = null;

            Tekst_Menu_Cena.Text = "";
            Tekst_Menu_Opis.Text = "";
            sqlCmd.CommandText = "select Opis, Cena from Wyswietl_Menu where Rodzaj='" + comboBox.Text + "'";

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

        public void wyswietlStolik()
        {
            SqlConnection sqlConn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            SqlDataReader dataReader = null;
            
            Tekst_Stoliki_Numer.Text = "";
            Tekst_Stoliki_Miejsca.Text = "";
            sqlCmd.CommandText = "select Numer, Miejsca from Wyswietl_Stolik where not Numer=0";

            dataReader = sqlCmd.ExecuteReader();

            if (dataReader != null)
            {
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                Dane_Stoliki.ItemsSource = dt.DefaultView;

                dataReader.Close();
            }

            sqlConn.Close();
        }
       

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Tekst_Menu_Cena_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Tekst_Stoliki_Numer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
           
        }

        private void Tekst_Stoliki_Miejsca_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Przycisk_Stoliki_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (Tekst_Stoliki_Numer.Text == "" || Convert.ToInt32(Tekst_Stoliki_Numer.Text) == 0)
            {
                MessageBox.Show("Nie podano numeru stolika", "Błąd");
            }
            else if (Tekst_Stoliki_Miejsca.Text == "" || Convert.ToInt32(Tekst_Stoliki_Miejsca.Text) == 0)
            {
                MessageBox.Show("Nie podano ilości miejsc", "Błąd");
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection("Server= " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;




                string alter = "Select Count(*) from Stolik where Numer_stolika =" + Tekst_Stoliki_Numer.Text.ToString();
                sqlCmd.CommandText = alter;
                int liczba_wierszu = (int)sqlCmd.ExecuteScalar();
                if (liczba_wierszu == 0)
                {
                    sqlCmd.CommandText = "insert into Stolik (Ilosc_miejsc, Numer_stolika) Values(" + Tekst_Stoliki_Miejsca.Text.ToString() + "," + Tekst_Stoliki_Numer.Text.ToString() + ")";
                    sqlCmd.ExecuteReader();

                }
                if (liczba_wierszu >= 1)
                {
                    MessageBox.Show("Stolik o podanym numerze już istnieje","Błąd");

                }
                wyswietlStolik();
                sqlConn.Close();

            }

            }

        private void Przycisk_Stoliki_Usun_Click(object sender, RoutedEventArgs e) // trzeba sprawdzić na danych z rezerwacja bez rezerwacji 
        {

            SqlConnection sqlConn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;

            DataRowView row = (DataRowView)Dane_Stoliki.SelectedItem;

            if (row != null)
            {


                DateTime pora = DateTime.Now;
                string alter = "Select Count(*) from Stolik_Rezerwacja where Numer =" + row[0].ToString() + " and Data>='" + pora.Year + "-" + pora.Month + "-" + pora.Day + "' and Czas>='" + pora.Hour + ":" + pora.Minute + ":" + pora.Second+"'";
                sqlCmd.CommandText = alter;
                int liczba_wierszy = (int)sqlCmd.ExecuteScalar();
                //jeżeli istnieje rezerwacja na kolejne dni nie mozna usunąć 
                if (liczba_wierszy == 0)
                {
                    //zlicza czy kiedykolwiek byla rezerwacja na ten stolik
                    string alter3 = "Select Count(*) from Stolik_Rezerwacja where Numer =" + row[0].ToString() + " and Data<'" + pora.Year + "-" + pora.Month + "-" + pora.Day + "' and Czas>='" + pora.Hour + ":" + pora.Minute + ":" +pora.Second+"'";
                    sqlCmd.CommandText = alter3;
                    int liczba_wierszy3 = (int)sqlCmd.ExecuteScalar();
                    alter = "Select Count(*) from Stolik_Zamowienie where Numer =" + row[0].ToString();
                    sqlCmd.CommandText = alter;
                    int liczba_wierszy2 = (int)sqlCmd.ExecuteScalar();
                    liczba_wierszy2 += liczba_wierszy3;
                    //jezeli nie bylo rezerwacji dla tego stolika ani nie przyszedl klient bez rezerwacji  mozna usunac 
                    if (liczba_wierszy2 == 0)
                    {
                        sqlCmd.CommandText = "delete Stolik where Numer_stolika = " + row[0].ToString();
                        sqlCmd.ExecuteReader();
                    }
                    // jezeli byla kiedy rezerwacja albo przyszedl klient bez stolika to nie tracimy informacji o liczbie miejsc
                    else if (liczba_wierszy2 >= 1)
                    {

                        string temp = "update Stolik set Numer_stolika = 0 where Numer_stolika= " + row[0].ToString();
                        sqlCmd.CommandText = temp;
                        sqlCmd.ExecuteReader();
                    }
                }
                else
                {

                    MessageBox.Show("Stolik zarezerwowany-nie można go usunąć", "Błąd");

                }


                }
                else
                {

                    MessageBox.Show("Wybierz stolik", "Błąd");

                }
                wyswietlStolik();
                sqlConn.Close();



            


        }

        private void Przycisk_Stoliki_Edytuj_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection("Server = " + serwer + ";Integrated Security = SSPI; Database = 'Restauracja'");

            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;

            DataRowView row = (DataRowView)Dane_Stoliki.SelectedItem;

            if (row != null)
            {

                if (Tekst_Stoliki_Numer.Text == "" || Tekst_Stoliki_Numer.Text == "0") { MessageBox.Show("Nie podano numeru", "Błąd"); }



                else
                {

                    DateTime pora = DateTime.Now;
                    string alter = "Select Count(*) from Wyswietl_Stolik where Numer =" + Tekst_Stoliki_Numer.Text; 
                    sqlCmd.CommandText = alter;
                    int liczba_wierszy = (int)sqlCmd.ExecuteScalar();
                    if (liczba_wierszy == 0)
                    {
                        

                            string temp = "update Stolik set Numer_stolika = "+ Tekst_Stoliki_Numer.Text+" where Numer_stolika= " + row[0].ToString();
                            sqlCmd.CommandText = temp;
                            sqlCmd.ExecuteReader();
                        
                    }
                    else
                    {

                        MessageBox.Show("Istenie już stolik o podanym numerze", "Błąd");

                    }






                }

            }
                else
                {

                    MessageBox.Show("Wybierz stolik", "Błąd");

                }
                wyswietlStolik();
                sqlConn.Close();



            }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TabControl tabControl = sender as TabControl; // e.Source could have been used instead of sender as well
            TabItem item = tabControl.SelectedValue as TabItem;
            if (item.Name == "stolik")
            {
                wyswietlStolik();
            }


        }
    }
    }
    

