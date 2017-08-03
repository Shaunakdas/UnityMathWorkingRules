using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionChecker : OptionChecker {
	
	//Holder to hold list of TargetItemChecker
	void awake(){
		ItemTargetType=TargetType.Question;
	}
	virtual public void addToMasterLine(){ 
//		BasicGOOperation.getMasterLineRef(ContainerElem).QuestionList.Add(this);
		Debug.Log(ContainerElem.GetType().ToString());
		addToList (this, BasicGOOperation.getMasterLineRef(ContainerElem).QuestionList);
	}
	virtual public void addToMasterLine(Line _lineRef){ 
//		_lineRef.QuestionList.Add(this);
		addToList (this, _lineRef.QuestionList);
	}
	void addToList(QuestionChecker question,List<QuestionChecker> quesList){
		if (quesList.IndexOf (question) < 0) {
			quesList.Add (question);
		}
	}
}
