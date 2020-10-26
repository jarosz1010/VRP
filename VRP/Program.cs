using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace VRP
{
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
            Console.WriteLine(rozmiar);

            // Zapisanie odleglosci do tablicy
            for (int i = 2; i < rozmiar; i++)
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
            System.Console.WriteLine("There were {0} lines.", counter);
            Console.WriteLine(Odleglosci[0][1]);
        }
    }
}
