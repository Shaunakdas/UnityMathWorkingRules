using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionChecker : OptionChecker {
	
	//Holder to hold list of TargetItemChecker
	public List<OptionChecker> TargetOptionCheckerList = new List<OptionChecker>();
	void awake(){
		ItemTargetType=TargetType.Question;
	}
}
