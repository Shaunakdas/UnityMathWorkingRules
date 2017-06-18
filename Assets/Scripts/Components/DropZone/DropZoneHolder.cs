﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHolder : MonoBehaviour {
	public bool idCheck{ get; set; }
	public bool multipleHolderCheck;
	public GameObject holderListParentGO;
	public List<string> TargetTextList{get; set;}
	void Awake(){
		multipleHolderCheck = false;
	}
	// Use this for initialization
	void Start () {
//		TargetTextList = new List<string> ();
	}
	public bool checkDropZoneItem(string inputText){
		Debug.Log ("Holder: checkDropZoneItem for checking "+inputText);
		bool inputCorrect = false;
		if (idCheck) {
			if (TargetTextList.IndexOf(inputText)!= -1) {
				inputCorrect = true;
			}
		} else {
			if (checkCompositeText(inputText)) {
				inputCorrect = true;
			}
		}
		if (multipleHolderCheck) {
			holderListParentGO.GetComponent<DropZoneHolderParent> ().dropEvent(inputCorrect);
		}
		return inputCorrect;
	}
	public bool checkCompositeText(string inputText){
		
		//Checking through TargetTextList;
		foreach (string targetText in TargetTextList) {
			Debug.Log("inputText"+inputText+"targetText"+targetText);
			//Checking for TriedText with same length;
			if (targetText.Length == inputText.Length) {


				//Checking for matching exact text
				if ((inputText.IndexOf(' ') == -1)&&(inputText == targetText)) {
					//Removing targetText from targetTextList
					TargetTextList.Remove(inputText);
					return true;
				}
				int inputCharIndex = 0;
				//Iterating through whole list of characters of targetText
				foreach (char inputChar in inputText.ToCharArray()) {
					// Checking for matching text after removing space
					if ((inputChar != ' ')&&(inputChar == targetText.ToCharArray()[inputCharIndex])) {
						
						return true;
					}
					inputCharIndex ++;
				}
			}
		}
		return false;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
