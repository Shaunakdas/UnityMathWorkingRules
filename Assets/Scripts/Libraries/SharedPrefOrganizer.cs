﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SharedPrefOrganizer {

	/// <summary>
	/// Gets the Android activity.
	/// </summary>
	static AndroidJavaObject getAndroidActivity(){
		AndroidJavaClass activityClass;
		activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return activityClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	/// <summary>
	/// Sets the shared prefs in android.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	static void setSharedPrefs (string key, string value) {
		getAndroidActivity ().Call("setPrefsValue",key, value);
	}
	
	/// <summary>
	/// Gets the shared prefs in android.
	/// </summary>
	/// <param name="key">Key.</param>
	static string getSharedPrefs (string key) {
		return (getAndroidActivity ().Call<string>("getPrefsValue",key));
	}
}
