using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnOptionChecker : OptionChecker {
	public bool correctFlag = false;
	public bool userInputFlag = false;
//	public GameObject SelBtnHolderGO;
	public string DisplayText = "";
	public bool HighlightOnCorrectSelection = true;
	AnimationManager animManager;

	public void changeInputFlag(){
		attemptEvent ();
		userInputFlag = !userInputFlag;
		(ParentChecker as SelBtnQuestionChecker).optionSelected ();
	}
	override public void activateAnim(){
		//Animation for activate select button option
		Debug.Log ("activateAnim");
		if (ItemAttemptState != AttemptState.Activated) {
			base.activateAnim ();
			this.gameObject.GetComponent<TweenColor> ().enabled = true;
		}
	}
	override public void deactivateAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.deactivateAnim();
		this.gameObject.GetComponent<TweenColor> ().enabled = false;
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.correctAnim();
		animManager.correctAnim (1, gameObject, nextEvent);
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		base.incorrectAnim();
		animManager.incorrectAnim (1, gameObject, nextEvent,false);
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
		if ((ItemAttemptState == AttemptState.Activated) || (ItemAttemptState == AttemptState.Checked)) {
			base.correctionAnim ();
			animManager.correctAnim (1, gameObject, nextEvent);
		}
	}
	void Awake(){
		ItemTargetType=TargetType.Option;
		if (gameObject.GetComponent<AnimationManager> () == null)
			gameObject.AddComponent<AnimationManager> ();
		animManager = gameObject.GetComponent<AnimationManager> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
