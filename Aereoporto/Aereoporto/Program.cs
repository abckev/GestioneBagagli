using System;

namespace Aereoporto
{
    class Program
    {
        static void Main(string[] args)
        {
            BaggageHandler provider = new BaggageHandler();
            ArrivalsMonitor observer1 = new ArrivalsMonitor("Scarico dei bagagli");     // monitor scarico dei bagagli
            ArrivalsMonitor observer2 = new ArrivalsMonitor("Ritiro dei bagagli");      // monitor ritiro dei bagagli

            provider.BaggageStatus(712, "Bologna", 2); 
            observer1.Subscribe(provider);      // L'osservatore che desidera ricevere informazioni aggiornate richiamano questo metodo

            provider.BaggageStatus(712, "Milano", 3); 
            provider.BaggageStatus(400, "Roma", 4); 
            provider.BaggageStatus(712, "Bologna", 2); 
            observer2.Subscribe(provider);

            provider.BaggageStatus(511, "Ancona", 1); 
            provider.BaggageStatus(712);
            observer2.Unsubscribe();

            provider.BaggageStatus(400);
            observer2.Unsubscribe();    // Ritiro un bagaglio con num. del nastro 400

            provider.LastBaggageClaimed();  // Visualizzo l'ultimo bagaglio ritirato

            Singleton s1 = Singleton.Instance();
            Singleton s2 = Singleton.Instance();

            // Verifica l'instanza
            if (s1 == s2)
                Console.WriteLine("--- Istanza Singleton Unica ---");
            else
                Console.WriteLine("--- Instanza Singleton Differente ---");
        }
    }
}
