using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneOptionChecker : OptionChecker {
	public BaseElement element;
	public bool idCheck{ get; set; }
	public string filledText{ get; set; }
	public bool attempted{ get; set; }
	AnimationManager animManager;
	void Awake(){
		ItemTargetType=TargetType.Option;
		if (gameObject.GetComponent<AnimationManager> () == null)
			gameObject.AddComponent<AnimationManager> ();
		animManager = gameObject.GetComponent<AnimationManager> ();
	}
	// Use this for initialization
	void Start () {
		idCheck = false;
		attempted = false;
	}
	//Checker function called after the item is dropped in drop zone.
	public bool checkDropZoneItem(string text){
		if (ItemAttemptState != AttemptState.Corrected) {
			attemptEvent ();
			attempted = true;
			bool correct = false;
			Debug.Log ("checkDropZoneItem for checking " + text);
			filledText = text;
			if ((ParentChecker as DropZoneQuestionChecker).checkDropZoneItem ()) {
				correct = true;
			}
			if (correct) {
				correctAnim ();
			} else {
				incorrectAnim ();
				filledText = null;
			}
			return false;
		} else {
			return true;
		}
	}


	// Update is called once per frame
	void Update () {
		if ((TimerAnimGO != null) && (TimerAnimGO.activeSelf)) {
			if (ItemAttemptState != AttemptState.Activated) {
				NGUITools.Destroy (TimerAnimGO);
			}
		}
	}
	//----------------------Animations ----------------------------
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>

	override public void activateAnim(){
		//Animation for selecting the correct option
		Debug.Log("activateAnim");
		if (ItemAttemptState != AttemptState.Activated) {
			base.activateAnim ();
			gameObject.GetComponent<TweenColor> ().enabled = true;
			startTimerAnim ();
		}
	}
	override public void startTimerAnim(){
		//Start Item Timer
		if (ItemAttemptState == AttemptState.Activated) {
			TimerAnimGO = animManager.startTimerAnim(1,ParentChecker.gameObject,new EventDelegate(correctionAnim),true);
		}

	}
	override public void deactivateAnim(){
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		base.deactivateAnim();
		gameObject.GetComponent<TweenColor>().enabled = false;
		nextEvent.Execute ();
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		if (ItemAttemptState == AttemptState.Attempted) {
			base.correctAnim ();
			ParentChecker.notifyManager ( ScoreManager.Result.Correct);
			animManager.correctAnim (1, gameObject.GetComponentInChildren<CustomDragDropItem> ().gameObject, new EventDelegate (deactivateAnim));
		}
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		if (ItemAttemptState == AttemptState.Attempted) {
			base.incorrectAnim ();
			ParentChecker.notifyManager ( ScoreManager.Result.Incorrect);
			animManager.incorrectAnim (2, gameObject, new EventDelegate (correctionAnim), true);
		}
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim"+ItemAttemptState.ToString());

		if ((ItemAttemptState == AttemptState.Activated)||(ItemAttemptState == AttemptState.Checked)) {
			base.correctionAnim ();
			(ParentChecker as DropZoneQuestionChecker).correctionAnim (this);
		}
	}
	override public void autocorrectionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("autocorrectionAnim"+ItemAttemptState.ToString());
		base.correctionAnim ();
		(ParentChecker as DropZoneQuestionChecker).autocorrectionAnim (this, new EventDelegate (deactivateAnim));
	}

}
