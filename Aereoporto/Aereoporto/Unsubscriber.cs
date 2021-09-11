using System;
using System.Collections.Generic;

namespace Aereoporto
{
    internal class Unsubscriber<BaggageInfo> : IDisposable
    {
        // Raccolta osservatori
        private List<IObserver<BaggageInfo>> _observers;

        // Riferimento all'osservatore che viene aggiunto alla raccolta
        private IObserver<BaggageInfo> _observer;

        internal Unsubscriber(List<IObserver<BaggageInfo>> observers, IObserver<BaggageInfo> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        // Verifica se l'osservatore è ancora presente nella raccolta osservatori, in caso affermativo lo rimuove
        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}