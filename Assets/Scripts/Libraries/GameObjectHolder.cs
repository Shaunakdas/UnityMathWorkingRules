using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using System.Text;
using System;

public class GameObjectHolder : MonoBehaviour {

	public GameObject Dragon,Jelly,UIRoot;

	// Use this for initialization
	void Start () {
        StartCoroutine(postWWW());
	}
	
    IEnumerator postWWW()
    {
        var jsonString = "{\"email\":\"shaunakdas2020+lino@gmail.com\",\"password\":\"password\"}";
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://drona-be-1540053061.ap-south-1.elb.amazonaws.com/api/v1/login/email", "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        unityWebRequest.downloadHandler = downloadHandlerBuffer;

        yield return unityWebRequest.Send();

        string response = unityWebRequest.downloadHandler.text;

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }
        Debug.Log(response);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
