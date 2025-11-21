using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class FlightAPIClient : MonoBehaviour
{
    [Header("Api Settings")]
    public string apiUrl = "https://opensky-network.org/api/states/all";
    public float refrechInterval = 5f;


    public FlightDataManager dataManager;
    void Start()
    {
        StartCoroutine(FetchLoop());
    }

    private IEnumerator FetchLoop()
    {
        while (true)
        {
            yield return FetchData();
            yield return Timer(refrechInterval);
        }
    }


    private IEnumerator FetchData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        Debug.Log("Requesting data");
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Api error");
            yield break;
        }
        string json = request.downloadHandler.text;
        if (!string.IsNullOrEmpty(json))
        {
            RawOpenSkyResponse raw = JsonConvert.DeserializeObject<RawOpenSkyResponse>(json);


            List<FlightState> flights = new List<FlightState>();
            if (raw.states != null)
            {
                foreach (var row in raw.states)
                {
                    var fs = FlightStateConverter.FromRawRow(row);
                    if (fs != null)
                        flights.Add(fs);
                }

                dataManager.UpdateFlights(flights);
            }
            else
            {
                Debug.LogWarning("raw.states is null — no flight data");
            }


            
        }
        else
            Debug.Log("Warning: No aircraft data (states = null)");
    }

    private IEnumerator Timer(float seconds)
    {
        while (seconds > 0)
        {
            Debug.Log("Next update in: " + seconds);
            yield return new WaitForSeconds(1f);
            seconds--;
        }

    }
}
