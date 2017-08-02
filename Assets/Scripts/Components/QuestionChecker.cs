using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionChecker : OptionChecker {
	
	//Holder to hold list of TargetItemChecker
	void awake(){
		ItemTargetType=TargetType.Question;
	}
	public void addToMasterLine(){ 
		BasicGOOperation.getMasterLineRef(ContainerElem).QuestionList.Add(this);
	}
}
