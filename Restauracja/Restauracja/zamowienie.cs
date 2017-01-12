using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja

{
    class pozycja
    {
        private int produkt_id = 0;
        private int ilosc = 0;
        private string szczegoly = "";
        public pozycja(int p, int i, string s)
        {
            produkt_id = p;
            ilosc = i;
            szczegoly = s;
        }
        public int Produkt_id
        {
            get
            {
                return produkt_id;
            }
            set
            {
                produkt_id = value;
            }

        }
        public int Ilosc
        {
            get
            {
                return ilosc;
            }
            set
            {
                ilosc = value;
            }

        }

        public string Szczegoly
        {
            get
            {
                return szczegoly;
            }
            set
            {
                szczegoly = value;
            }

        }

    }
    class zamowienie
    {
     private int numer_stolika=0;
     private int id_klienta=0;
     private int id_rezerwacji=0;
     private int id_stolika = 0;
    
    public zamowienie(int n, int ik,int ir, int ids)
        {
            numer_stolika = n;
            id_klienta = ik;
            id_rezerwacji = ir;
            id_stolika = ids;
        }
     private List<pozycja> zamowione=new List<pozycja>();

     public int Numer_stolika
        {
            get
            {
                return numer_stolika;
            }
            set
            {
                numer_stolika = value;
            }

        }

     public int Id_klienta
        {
            get
            {
                return id_klienta;
            }
            set
            {
                id_klienta = value;
            }

        }

     public int Id_rezerwacji
        {
            get
            {
                return id_rezerwacji;
            }
            set
            {
                id_rezerwacji = value;
            }

        }
        public int Id_stolika
        {
            get
            {
                return id_stolika;
            }
            set
            {
                id_stolika = value;
            }

        }
        public void Dodaj(int p, int i, string s)
        {
            zamowione.Add(new pozycja(p, i, s));
        }
    public void Usun(int p, int i, string s)
    {
        int z = 0;
        int licznik = 0;
        bool jest = false;
        foreach (pozycja temp in zamowione)
        {
            if (temp.Produkt_id == p && temp.Ilosc == i && temp.Szczegoly.Equals(s))
            {
                jest = true;
                licznik = z;
            }
            z++;
        }
        if (jest == true) { zamowione.RemoveAt(licznik); }
    }

    }
}
