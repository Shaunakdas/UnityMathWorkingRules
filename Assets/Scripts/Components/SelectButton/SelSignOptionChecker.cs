using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelSignOptionChecker : SelBtnOptionChecker {
	/// <summary>
	/// Changes the input flag.
	/// </summary>
	override public void changeInputFlag(){
		Debug.Log ("OVERRIDE");
		attemptEvent ();
		userInputFlag = !userInputFlag;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
