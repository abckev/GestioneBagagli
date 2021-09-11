using System;
using System.Collections.Generic;


// Fornisce informazioni sui voli in arrivo e sui carousel (nastri) in cui possono essere ritirati i bagagli per ogni volo
public class BaggageInfo
{
    private int flightNo;     // Num. di volo
    private string origin;    // Aeroporto provenienza
    private int location;     // Num. del nastro

    internal BaggageInfo(int flight, string from, int carousel)
    {
        this.flightNo = flight;
        this.origin = from;
        this.location = carousel;  
    }

    public int FlightNumber
    {
        get { return this.flightNo; }
    }

    public string From
    {
        get { return this.origin; }
    }

    public int Carousel
    {
        get { return this.location; }
    }
}