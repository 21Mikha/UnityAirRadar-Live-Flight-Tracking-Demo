using System.Collections.Generic;
using UnityEngine;

public class FlightDataManager : MonoBehaviour
{
    public Dictionary<string, FlightState> allFlights = new Dictionary<string, FlightState>();


    public void UpdateFlights(List<FlightState> freshData)
    {
        allFlights.Clear();

        foreach (FlightState flight in freshData)
        {
            if(string.IsNullOrEmpty(flight.icao24)==false)
            {
                allFlights[flight.icao24] = flight;
            }
        }
        Debug.Log("Stored " + allFlights.Count + " flights");
    }

}
