using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetItemChecker : TargetOptionChecker {
	
	//Holder to hold list of TargetItemChecker
	public List<TargetOptionChecker> TargetOptionCheckerList = new List<TargetOptionChecker>();
	void awake(){
		ItemTargetType=TargetType.Item;
	}
}
