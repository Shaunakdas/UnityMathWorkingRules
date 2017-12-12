using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AndroidOrganizer {

	/// <summary>
	/// Gets the Android activity.
	/// </summary>
	static AndroidJavaObject getAndroidActivity(){
		AndroidJavaClass activityClass;
		activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return activityClass.GetStatic<AndroidJavaObject>("currentActivity");
	}
	/// <summary>
	/// Is currently being tested in android
	/// </summary>
	/// <returns><c>true</c>, if android was ised, <c>false</c> otherwise.</returns>
	public static bool isAndroid(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		return true;
		#endif
		return false;
	}

	/// <summary>
	/// Sets the shared prefs in android.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static void setSharedPrefs (string key, string value) {
		Debug.Log ("Setting Shared Preferences. Key: "+key+", Value :"+value);
		if (isAndroid ()) {
			getAndroidActivity ().Call ("setPrefsValue", key, value);
		}
	}
	
	/// <summary>
	/// Gets the shared prefs in android.
	/// </summary>
	/// <param name="key">Key.</param>
	public static string getSharedPrefs (string key) {
		string value = "";
		if (isAndroid ()) {
			value = (getAndroidActivity ().Call<string> ("getPrefsValue", key));
		}
		Debug.Log ("Getting Shared Preferences. Key: "+key+", Value :"+value);
		return value;
	}
	/// <summary>
	/// Calls Given method in UnityActivity. Used for exiting Unity
	/// </summary>
	/// <param name="key">Key.</param>
	public static void callMethod(string key){
		Debug.Log ("Calling Android Activity method. Name: "+key);
		if (isAndroid ()) {
			getAndroidActivity ().Call (key);
		}
	}
}
