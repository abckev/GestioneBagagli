using System;
using System.Collections.Generic;

namespace Aereoporto
{
        public class ArrivalsMonitor : IObserver<BaggageInfo>
        {
            private string name;                                         // Nome
            private List<string> flightInfos = new List<string>();       // Raccolta info di ogni volo
            private IDisposable cancellation;                            // Cancella gli osservatori quando la notifica è completa.
            private string fmt = "{0,-20} {1,5}  {2, 3}";                // Formato

        public ArrivalsMonitor(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("All'osservatore deve essere assegnato un nome.");

            this.name = name;
        }

        // Consente alla classe di salvare l'implementazione di IDisposable restituita dalla chiamata a Subscribe in una variabile privata
        public virtual void Subscribe(BaggageHandler provider)
        {
            cancellation = provider.Subscribe(this);
        }
        // Consente alla classe di annullare la sottoscrizione delle notifiche chiamando l'implementazione di Dispose del provider
        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            flightInfos.Clear();
        }

        // Indica che il provider ha completato l'invio di notifiche
        public virtual void OnCompleted()
        {
            flightInfos.Clear();
        }

        // Informa l'osservatore che si è verificato un errore
        public virtual void OnError(Exception e) { }

        // Aggiornamento informazioni di ogni osservatore
        // Gestisce le informazioni sugli aeroporti di origine per i voli in arrivo e sui nastri su cui sono disponibili i bagagli
        public virtual void OnNext(BaggageInfo info)
        {
            bool updated = false;

            // Il volo ha scaricato il suo bagaglio; rimuovere dal monitor
            if (info.Carousel == 0)
            {
                var flightsToRemove = new List<string>();
                string flightNo = String.Format("{0,5}", info.FlightNumber);

                foreach (var flightInfo in flightInfos)
                {
                    if (flightInfo.Substring(21, 5).Equals(flightNo))
                    {
                        flightsToRemove.Add(flightInfo);
                        updated = true;
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flightInfos.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
            else
            {
                // Aggiunge il volo se non esiste nella collezione
                string flightInfo = String.Format(fmt, info.From, info.FlightNumber, info.Carousel);
                if (!flightInfos.Contains(flightInfo))
                {
                    // Aggiunge le informazioni relative al volo all'elenco
                    flightInfos.Add(flightInfo);
                    updated = true;
                }
            }
            if (updated)
            {
                // Ordinamneto per aeroporto di provenienza in ordine alfabetico
                flightInfos.Sort();
                Console.WriteLine("Informazioni dal monitor: {0}", this.name);
                foreach (var flightInfo in flightInfos)
                    Console.WriteLine(flightInfo);

                Console.WriteLine();
            }
        }
    }
}