using System;
using System.Collections.Generic;
using System.Text;

namespace Aereoporto
{
    public sealed class Singleton
    {
        static Singleton instance;

        // Costruttore
        private Singleton()  { }
        public static Singleton Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }
}
