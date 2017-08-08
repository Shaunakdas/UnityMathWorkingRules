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
//		Debug.Log ("activateAnim");
		if (ItemAttemptState != AttemptState.Activated) {
			base.activateAnim ();
			//Special handling
			Color defaultCol = this.gameObject.GetComponent<UIButton> ().defaultColor; defaultCol.a = 1.0f;
			this.gameObject.GetComponent<UIButton> ().defaultColor =defaultCol;
			this.gameObject.GetComponent<TweenScale> ().enabled = true;
		}
	}
	override public void deactivateAnim(){
		//Animation for selecting the correct option
		Debug.Log("deactivateAnim");
		base.deactivateAnim();
		this.gameObject.GetComponent<TweenScale> ().enabled = false;

		Debug.Log (nextEvent.ToString()==null);
		if (nextEvent.ToString() != null) {
			nextEvent.Execute ();
		}
	}
	override public void correctAnim(){
		//Animation for selecting the correct option
		Debug.Log("correctAnim");
		base.correctAnim();
		Debug.Log (nextEvent.ToString());
		animManager.correctAnim (1, gameObject, new EventDelegate (deactivateAnim));
	}
	override public void incorrectAnim(){
		//Animation for selecting the wrong option
		Debug.Log("incorrectAnim");
		base.incorrectAnim();
		animManager.incorrectAnim (1, gameObject, new EventDelegate (deactivateAnim),false);
	}
	public void destroyAnim(){
		//Animation for selecting the wrong option
		Debug.Log("destroyAnim");
		base.incorrectAnim();
		animManager.incorrectAnim (6, gameObject, new EventDelegate (deactivateAnim),true);
	}
	override public void correctionAnim(){
		//Animation for ignoring the correct option
		Debug.Log("correctionAnim");
		if ((ItemAttemptState == AttemptState.Activated) || (ItemAttemptState == AttemptState.Checked)) {
			base.correctionAnim ();
			animManager.correctAnim (1, gameObject, new EventDelegate (deactivateAnim));
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
