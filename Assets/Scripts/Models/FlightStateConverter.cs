using System;
using System.Collections.Generic;

public static class FlightStateConverter
{
    public static FlightState FromRawRow(List<object> raw)
    {
        if (raw == null || raw.Count < 17)
            return null;

        FlightState fs = new FlightState();

        fs.icao24 = SafeString(raw[0]);
        fs.callsign = SafeString(raw[1]).Trim();
        fs.originCountry = SafeString(raw[2]);

        fs.timePosition = SafeInt(raw[3]);
        fs.lastContact = SafeInt(raw[4]) ?? 0;

        fs.longitude = SafeFloat(raw[5]);
        fs.latitude = SafeFloat(raw[6]);
        fs.baroAltitude = SafeFloat(raw[7]);
        fs.onGround = SafeBool(raw[8]) ?? false;
        fs.velocity = SafeFloat(raw[9]);
        fs.heading = SafeFloat(raw[10]);
        fs.verticalRate = SafeFloat(raw[11]);

        fs.geoAltitude = SafeFloat(raw[13]);
        fs.squawk = SafeString(raw[14]);
        fs.spi = SafeBool(raw[15]) ?? false;
        fs.positionSource = SafeInt(raw[16]) ?? 0;

        return fs;
    }

    private static string SafeString(object o) =>
        o == null ? "" : o.ToString();

    private static int? SafeInt(object o)
    {
        if (o == null) return null;
        if (int.TryParse(o.ToString(), out int result))
            return result;
        return null;
    }

    private static float? SafeFloat(object o)
    {
        if (o == null) return null;
        if (float.TryParse(o.ToString(), out float result))
            return result;
        return null;
    }

    private static bool? SafeBool(object o)
    {
        if (o == null) return null;
        if (bool.TryParse(o.ToString(), out bool result))
            return result;
        return null;
    }
}
