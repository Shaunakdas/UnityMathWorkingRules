using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityCaller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventDelegate.Add (gameObject.GetComponent<UIButton> ().onClick, StartPackage );
	}

	// Update is called once per frame
	void Update () {

	}
	void StartPackage(){    
		AndroidJavaClass activityClass;
		AndroidJavaObject activity, packageManager;
		AndroidJavaObject launch;


		activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Calling unityActivity");
//		activity.Call("Call");
		string key = "key";
		Debug.Log(activity.Call<string>("getPrefsValue",key));
		activity.Call("setPrefsValue",key, "value");
		Debug.Log(activity.Call<string>("getPrefsValue",key));
	}
}
