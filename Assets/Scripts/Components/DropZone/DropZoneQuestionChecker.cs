using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DropZoneQuestionChecker : QuestionChecker {

	//----------------------Common attributes ----------------------------


	//Holder to hold list of TargetItemChecker
	void awake(){
		ItemTargetType=TargetType.Question;
	}
	public bool checkDropZoneItem(){
		return (ParentChecker as DropZoneHolder).checkDropZoneItem (this);
	}

	//----------------------Animations ----------------------------

	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void activateAnim(){
		//		Debug.Log ("activateAnim of DropZoneRowCell");
		base.activateAnim ();
		for (int i = 0; i < ChildList.Count; i++) {
			EventDelegate nextOptionEvent = nextEvent;
			if (i < ChildList.Count - 1) {
				nextOptionEvent = new EventDelegate (ChildList [i + 1].activateAnim);
			}
			ChildList[i].nextEvent = nextOptionEvent;
		}
		ChildList [0].activateAnim();
	}
	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	virtual public void questionFinish(){
		deactivateAnim ();
		nextEvent.Execute ();
	}
	/// <summary>
	/// Getting correction animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	public void correctionAnim(DropZoneOptionChecker _option){
		Debug.Log ("correctionAnim of DropZoneRowCell"+ChildList.Count);
		base.correctionAnim ();
//		(ParentChecker as DropZoneHolder).correctionAnim (_option,this, _nextEvent);
		List<OptionChecker> PendingOptionList = ChildList.Where (x => x.ItemAttemptState != AttemptState.Deactivated).ToList();
		for (int i = 0; i < PendingOptionList.Count; i++) {
			EventDelegate nextOptionEvent = new EventDelegate(questionFinish);
			if (i < PendingOptionList.Count - 1) {
				nextOptionEvent = new EventDelegate (PendingOptionList [i + 1].autocorrectionAnim);
			}
			PendingOptionList[i].nextEvent = nextOptionEvent;
		}
		Debug.Log (PendingOptionList [0]);
		PendingOptionList [0].autocorrectionAnim();
	}
	public void autocorrectionAnim(DropZoneOptionChecker itemChecker,EventDelegate _nextEvent){
		Debug.Log ("correctionAnim of DropZoneRowCell");
		base.correctionAnim ();
		(ParentChecker as DropZoneHolder).correctionAnim (itemChecker,this, _nextEvent);
	}
	override public void notifyManager (ScoreManager.Result _result){
		if (_result == ScoreManager.Result.Correct) {
			List<OptionChecker> unactivatedOptions = ChildList.Where (x => x.ItemAttemptState == AttemptState.Unseen).ToList();

			Debug.Log("Updating Display : adding score"+unactivatedOptions.Count.ToString());
			if (unactivatedOptions.Count == 0) {
				foreach (OptionChecker option in ChildList) {
					scoreTracker.attemptScore += option.scoreTracker.attemptScore;
				}
				scoreTracker.notifyManager (ContainerElem.ParagraphRef, _result);
			}
		} else {
			scoreTracker.notifyManager (ContainerElem.ParagraphRef, _result);
		}
	}
	void onLastOptionCheck(){
		  
	}

}
