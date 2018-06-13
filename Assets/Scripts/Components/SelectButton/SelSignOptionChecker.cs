using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelSignOptionChecker : SelBtnOptionChecker {
//	public string filledText{ get; set; }
	//Target Text
	string _filledText= "";
	public string filledText{ 
		get { return userInputFlag?"-":"+";}
		set { _filledText = value; }
	}

	public void genCorrectFlag(string sign){
		filledText = sign;
		correctFlag = sign=="+"? false:true;
	}
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

	void Awake(){
		ItemTargetType=TargetType.Option;
		if (gameObject.GetComponent<AnimManager> () == null)
			gameObject.AddComponent<AnimManager> ();
		animManager = gameObject.GetComponent<AnimManager> ();
	}
		
	override public void autocorrectionAnim(){
//		activateAnim ();
		//Animation for ignoring the correct option
		Debug.Log("autocorrectionAnim"+ItemAttemptState.ToString());
//		base.correctionAnim ();
		inputCorrectTrigger();
	}

	public override bool inputCorrectTrigger(){
		if (ItemAttemptState != AttemptState.Deactivated) {
			return base.inputCorrectTrigger ();
		} else {
			return false;
		}
	}
}
