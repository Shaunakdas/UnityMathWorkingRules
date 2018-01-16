using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IntegerQuestionChecker : DropZoneQuestionChecker
{

	/// <summary>
	/// Getting active animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	override public void activateAnim(){
		//		Debug.Log ("activateAnim of DropZoneRowCell");
		Debug.Log("activateAnim"+(ChildList[0] as SelSignOptionChecker).correctFlag);
		base.activateAnim ();
		int first = -1;
		for (int i = 0; i < ChildList.Count; i++) {
			if (i == 0) {
				//For SelSignOption
				ChildList [0].activateAnim();
				//Sign Check is last event trigger which will trigger questionFinish
				ChildList [0].nextEvent = new EventDelegate(questionFinish);
			} else {
				//Last Drop Zone Check will trigger Sign Check
				EventDelegate nextOptionEvent = new EventDelegate(signCheck);
				if (i < ChildList.Count - 1) {
					nextOptionEvent = new EventDelegate (ChildList [i + 1].activateAnim);
				}
				ChildList [i].nextEvent = nextOptionEvent;
			}
		}
		//Looking for SelSignOption and starting after that option
		ChildList [1].activateAnim();
	}

	protected void signCheck(){
		(ChildList [0] as SelSignOptionChecker).inputCorrectTrigger ();
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
}

