using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour, IPanel
{
    public RawImage mapImg;
    public InputField mapNotes;
    public Text caseNumberText;
    public string apiKey;
    public float xCord, yCord;
    public int zoom;
    public int scaleSize;
    public int imgSize;
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";

    public void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        UIManager.Instance.currentPanel = "locationPanel";
    }

    public IEnumerator Start()
    {
        //starts when locationPanel script is called, checks if location services are enabled by the user and attempts to find GEO coordinates
        if (Input.location.isEnabledByUser == true)
        {
            // start service
            Input.location.Start();

            int maxWait = 20;

            // 20 second timer for location services
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                
                yield return new WaitForSeconds(1.0f);
                maxWait--;
            }

            // if location services has timed out sends message to console and stops coroutine
            if (maxWait < 0)
            {
                Debug.Log("Timed out");
                yield break;
            }

            if(Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine user location");
            }

            // adds the user location to the url string variables
            else
            {
                xCord = Input.location.lastData.latitude;
                yCord = Input.location.lastData.longitude;
            }

            // stops location services
            Input.location.Stop();
        }

        else
        {
            Debug.Log("location services are not enabled");

        }
        //builds the url to download map image of user location

        StartCoroutine(GetLocationRoutine());
    }

    IEnumerator GetLocationRoutine()
    {
        //constructs url to google static maps api service and downloads a raw image to display of users current location

        url = url + "center=" + xCord + "," + yCord + "&zoom=" + zoom + "&size=" + imgSize + "x" + imgSize + "&scale=" + scaleSize + "&key=" + apiKey;

        using (WWW map = new WWW(url))
        {
            yield return map;

            if (map.error != null)
            {
                Debug.LogError("Map error: " + map.error);
            }
            mapImg.texture = map.texture;
        }
    }

    public void ProcessInfo()
    {
        Texture2D convertedPhotoMaps = mapImg.texture as Texture2D;
        byte[] imgDataMap = convertedPhotoMaps.EncodeToPNG();

        UIManager.Instance.activeCase.locationPhoto = imgDataMap;

        if (string.IsNullOrEmpty(mapNotes.text) == false)
        {
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
        }
    }

}
