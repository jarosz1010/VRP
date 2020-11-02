using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters;

namespace VRP
{
    // Przyjeto ograniczenie max 3 miasta na ciezarowke
   public class Wyrzazanie
    {
       public List<List<double>> tablica;
   
        public List<int> permutacja_list;
        public List<int> Centralne = new List<int>();
        public List<double> Sumy = new List<double>();
        public int centralny;
       public int ilosc;
        public int blad = 0;

        public void SwapValues(int index1, int index2)
        {
            int temp = permutacja_list[index1];
            permutacja_list[index1] = permutacja_list[index2];
            permutacja_list[index2] = temp;
        }
        public double Cmax()
        {
            int miasta_na_pojazd = 0;
            double Suma1 = 0;
            
            blad = 0;
            Centralne.Clear();
            Sumy.Clear();
         
            for(int i = 1; i<(permutacja_list.Count); i++)
            {
               
                
                //Ograniczenie 1 - maksymalnie 4 miasta na jeden pojazd
                /*
                if(permutacja_list[i] != centralny)
                {
                    miasta_na_pojazd++;
                }
                if (miasta_na_pojazd > 4) blad = 1;
                else if (miasta_na_pojazd < 5 && permutacja_list[i] == centralny) miasta_na_pojazd = 0;
                
           */
                // Liczenie sumy kilometrow
                int first = permutacja_list[i];
                int second = permutacja_list[i - 1];
                
                Suma1 = Suma1 + tablica[second][first];
                // Ograniczenie 2 - suma kilometrow nie wieksza niz 1000
                Sumy.Add(Suma1);
              
               if (permutacja_list[i] == centralny) Centralne.Add(i);

                
                
            }
           
                for (int k = 1; k < Centralne.Count; k++)
            {
                
                double Roznica = Sumy[Centralne[k]-1] - Sumy[Centralne[k - 1]-1];
                if (Roznica > 1800)  blad = 2;
             
            }
            return Suma1;
        }
        
        public double Annealing()
        {
            double T = 1000;
            int i, j;
            double cmax_tmp = 0;
            double cmax_start;
            double r, error, diff;
            int L = ilosc;
           
            double a = 0.95;
            double Tend = 0.001;
            cmax_start = Cmax();
            while (T > Tend)
            {
                for (int k = 1; k < L; k++)
                {
                    Random rnd = new Random();
                    i = rnd.Next(0,ilosc);
                    j = rnd.Next(0, ilosc);
                    
                        SwapValues(i, j);
                        cmax_tmp = Cmax();
                    if (blad == 0)
                    {
                        if (cmax_tmp > cmax_start)
                        {
                            r = rnd.Next(0, 100);
                            r = r / 100;
                            diff = cmax_start - cmax_tmp;
                            double podziel = diff / T;
                            error = Math.Exp(podziel);
                           
                            if (r >= error) SwapValues(i, j);

                        }
                    }
                    // Tutaj zmienic kod bledu w zaleznosci od ograniczenia
                    else if (blad == 2) SwapValues(i, j);
                    
                }

                T = a * T;
            }
            for (int it = 0; it < permutacja_list.Count; it++)
                Console.WriteLine(permutacja_list[it]);
            return cmax_tmp;
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> Plik = new List<string>();
            List<List<double>> Odleglosci = new List<List<double>>();
            int counter = 0;
            string line;
            int rozmiar;
            string b;
            System.IO.StreamReader file =
    new System.IO.StreamReader(@"C:\Users\Michał\source\repos\VRP\VRP\PL.csv");
            while ((line = file.ReadLine()) != null)
            {
                Plik.Add(line);
                counter++;
            }

            // Zapisujemy ile jest miast uprzednio pozbywajac sie srednikow
           String[] rozmiar_st =  Plik[0].Split(';');
            rozmiar = Convert.ToInt32(rozmiar_st[0]);
     

            // Zapisanie odleglosci do tablicy
            for (int i = 2; i < rozmiar+2; i++)
            {
                List<double> pom = new List<double>();
                String[] elements = Plik[i].Split(';');
                foreach (var element in elements)
                {

                    pom.Add(Convert.ToDouble(element));
                }
                Odleglosci.Add(pom);

            }
            file.Close();
            double kilometry = 0;
            double kilometry_ann = 0;
            
            List<int> permutacja_list_tmp = new List<int>();
            Wyrzazanie test = new Wyrzazanie();
           
            test.tablica = Odleglosci;
            test.centralny = 7;
            test.ilosc = rozmiar;
            int do_dodawania = 0;
          
           
            for (int i = 0; i < (rozmiar-1)/3; i++)
            {
                permutacja_list_tmp.Add(test.centralny);
                for (int j = 0; j < 3; j++)
                {
                    
                    if (do_dodawania == test.centralny)
                        do_dodawania++;
                    if (do_dodawania != test.centralny)
                    {
                        permutacja_list_tmp.Add(do_dodawania);
                        
                        do_dodawania++;
                    }
                }
               
            }
            permutacja_list_tmp.Add(test.centralny);
          
            test.permutacja_list = permutacja_list_tmp;

         
           kilometry = test.Cmax();
            kilometry_ann = test.Annealing();
        Console.WriteLine("Ilosc kilometrow:" + kilometry);
            Console.WriteLine("Ilosc kilometrow po wyzarzaniu:" + kilometry_ann);

        }
    }
}
