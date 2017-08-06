using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneQuestionChecker : QuestionChecker {

	//----------------------Common attributes ----------------------------


	//Holder to hold list of TargetItemChecker
	void awake(){
		ItemTargetType=TargetType.Question;
	}


	//----------------------Animations ----------------------------

	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void activateAnim(){
		Debug.Log ("activateAnim of DropZoneRowCell");
		base.activateAnim ();
		for (int i = 0; i < ChildList.Count; i++) {
			EventDelegate nextItemEvent = nextEvent;
			if (i < ChildList.Count - 1) {
				nextItemEvent = new EventDelegate (ChildList [i + 1].activateAnim);
			}
			ChildList[i].nextEvent = nextItemEvent;
		}
		ChildList [0].activateAnim();
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void deactivateAnim(){
		base.deactivateAnim ();
		Debug.Log ("deactivateAnim of DropZoneRowCell");
		foreach (DropZoneOptionChecker itemChecker in ChildList) {
			itemChecker.deactivateAnim ();
		}
	}
}
