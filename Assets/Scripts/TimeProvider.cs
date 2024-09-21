using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Gets current Moscow time
public class TimeProvider
{
    public Action<TimeResponse> OnNewTimeReceived;

    private const string GetCurrentTimeURL = "https://www.timeapi.io/api/time/current/zone?timeZone=Europe%2FMoscow";

    public IEnumerator RequestCurrentTimeCoroutine()
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(GetCurrentTimeURL))
        {
            yield return webRequest.SendWebRequest();

            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                TimeResponse currentTime = JsonUtility.FromJson<TimeResponse>(webRequest.downloadHandler.text);
                OnNewTimeReceived.Invoke(currentTime);
            }
            else
            {
                Debug.LogError($"Could not load current time. Error: {webRequest.result}");
            }
        }
    }
}

