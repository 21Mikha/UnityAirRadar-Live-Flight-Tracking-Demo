using UnityEngine;

[System.Serializable]
public class FlightState
{
    public string icao24;
    public string callsign;
    public string originCountry;

    public int? timePosition;
    public int lastContact;

    public float? longitude;
    public float? latitude;
    public float? baroAltitude;

    public bool onGround;
    public float? velocity;
    public float? heading;
    public float? verticalRate;

    public float? geoAltitude;
    public string squawk;
    public bool spi;
    public int positionSource;

    public Vector2 worldPos;
}
