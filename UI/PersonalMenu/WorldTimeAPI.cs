using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WorldTimeAPI : PersistentSingleton<WorldTimeAPI>
{
    const string API_URL = "https://worldtimeapi.org/api/ip";
    public bool IsTimeLoaded = false;
    private DateTime currentDateTime = DateTime.Now; //Default device datetime in case if there's a problem in the internet connection

    public Text WorldTime;

    void Start()
    {
        //Debug.Log(currentDateTime.ToString("yyyy-MM-dd:HH:mm:ss"));
        StartCoroutine(GetRealDateTimeFromAPI());
    }

    void Update()
    {
        if (IsTimeLoaded)
        {
            //Debug.Log("Now time is " + GetCurrentDateTime().ToString("yyyy-MM-dd:HH:mm:ss"));
            //WorldTime.text = GetCurrentDateTime().ToString();
            WorldTime.text = GetCurrentDateTime().ToString("HH:mm:ss  yyyy-MM-dd");
        }
    }

    #region Singleton class: WorldTimeAPI

    /*API (json)
  {
  "abbreviation": "HKT",
  "client_ip": "203.198.205.57",
  "datetime": "2022-08-10T16:16:07.554560+08:00",
  "day_of_week": 3,
  "day_of_year": 222,
  "dst": false,
  "dst_from": null,
  "dst_offset": 0,
  "dst_until": null,
  "raw_offset": 28800,
  "timezone": "Asia/Hong_Kong",
  "unixtime": 1660119367,
  "utc_datetime": "2022-08-10T08:16:07.554560+00:00",
  "utc_offset": "+08:00",
  "week_number": 32
}
     We only need "datatime" property.
    */

    #endregion

    //json container
    public struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //..
    }

    public DateTime GetCurrentDateTime()
    {
        //dont need to get the datetime from the server again
        //just add the elapsed time since the game start to currentDateTime

        return currentDateTime.AddSeconds(Time.realtimeSinceStartup);
    }



    IEnumerator GetRealDateTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        Debug.Log("getting real datetime...");
        webRequest.timeout = 2;
        yield return webRequest.SendWebRequest();  //wait for connecting to the website

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error: " + webRequest.error);
        }
        else
        {
            //Success
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
            //Debug.Log("Downloadertext" + webRequest.downloadHandler.text);
            //Debug.Log("Timedata" + timeData.datetime);
            //convert the json downloaded to the struct timeData
            //timeData.datatime value is : 2022-08-10T08:43:04.163907+00:00

            currentDateTime = ParseDateTime(timeData.datetime);
            IsTimeLoaded = true;
            //Debug.Log(currentDateTime.ToString("yyyy-MM-dd:HH:mm:ss"));
            Debug.Log("CurrentTime" + currentDateTime.ToString());
        }
    }

    //return a datetime
    //2022-08-10T08:43:04.163907+00:00
    DateTime ParseDateTime(string datetime)
    {
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }


}
