using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public class LoginNEtworkController : MonoBehaviour {
	public static readonly string domain = "http://69c7e723.ngrok.io";

	IEnumerator getQAListNetworkCall() {
		string getQuesListUrl = domain + "/api/login/user_data?name="+"name"+"&email="+"email";
		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get (getQuesListUrl);
		yield return www.Send ();

		if (www.isNetworkError) {
			Debug.Log (www.error);
			//getQAListNetworkCallResponse = false;
		} else {
			//getQAListNetworkCallResponse = true;
			setQAListJSON (www.downloadHandler.text);
		}
	}


	public void setQAListJSON(string qaJSONText){
		var N = JSON.Parse(qaJSONText);
		string user_id =( N ["user_id"].Value);
		PlayerPrefs.SetString ("user_id", user_id);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
