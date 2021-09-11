using System;
using System.Collections.Generic;

namespace Aereoporto
{
    // Responsabile della ricezione di informazioni sui voli in arrivo e sui nastri per il ritiro dei bagagli 
    public class BaggageHandler : IObservable<BaggageInfo>
    {
        private List<IObserver<BaggageInfo>> observers;        // raccolta di client che riceveranno informazioni aggiornate
        private List<BaggageInfo> flights;                     // raccolta di voli e dei nastri ad essi assegnati

        public BaggageHandler()
        {
            observers = new List<IObserver<BaggageInfo>>();
            flights = new List<BaggageInfo>();
        }

        public IDisposable Subscribe(IObserver<BaggageInfo> observer)
        {
            // Controlla se l'osservatore è già registrato. In caso contrario lo aggiungo
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Fornisce all'osservatore i dati esistenti
                foreach (var item in flights)
                    observer.OnNext(item);
            }
            return new Unsubscriber<BaggageInfo>(observers, observer);
        }

        // Chiamata per indicare che tutti i bagagli sono stati scaricati
        public void BaggageStatus(int flightNo)
        {
            BaggageStatus(flightNo, String.Empty, 0);
        }

        public void BaggageStatus(int flightNo, string from, int carousel)
        {
            var info = new BaggageInfo(flightNo, from, carousel);

            // Il nastro è assegnato, quindi aggiungo un nuovo oggetto info all'elenco.
            if (carousel > 0 && !flights.Contains(info))
            {
                flights.Add(info);
                foreach (var observer in observers)
                    observer.OnNext(info);
            }
            else if (carousel == 0)
            {
                // Il ritiro del bagaglio per il volo è stato effettuato
                var flightsToRemove = new List<BaggageInfo>();
                foreach (var flight in flights)
                {
                    if (info.FlightNumber == flight.FlightNumber)
                    {
                        flightsToRemove.Add(flight);
                        foreach (var observer in observers)
                            observer.OnNext(info);
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flights.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
        }

        // Indica che tutte le notifiche sono state completate, quindi cancella la raccolta observers.
        public void LastBaggageClaimed()
        {
            foreach (var observer in observers)
                observer.OnCompleted();

            observers.Clear();
        }
    }
}