using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelBtnHolder : MonoBehaviour {
	public List<GameObject> SelBtnGOList;
	public int correctCount;
	public void addSelectBtn(GameObject selBtnGO, bool correctFlag){
		if (correctFlag)
			correctCount++;
		SelBtnGOList.Add (selBtnGO);
	}
	public void setParentCorrectCount(Paragraph para, Line line){
		if (correctCount > 1) {
			para.ParagraphCorrect = Paragraph.CorrectType.MultipleCorrect;
			line.addSubmitBtn ();
		}else
			para.ParagraphCorrect = Paragraph.CorrectType.SingleCorrect;
	}
	public void checkChildCorrect(){
		//Check for user selected Buttons
		foreach (GameObject selBtnGO in SelBtnGOList) {
			SelBtnItemChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
			if (itemChecker.userInputFlag) {
				//CheckAnimation if userinputFlag is correct. Wait
				if (itemChecker.correctFlag) {
					itemChecker.correctOptionSelected ();
				} else {
					itemChecker.incorrectOptionSelected ();
				}
			}
		}
		//check in not user selected buttons
		foreach (GameObject selBtnGO in SelBtnGOList) {
			SelBtnItemChecker itemChecker = selBtnGO.GetComponent<SelBtnItemChecker> ();
			if ((!itemChecker.userInputFlag)&&(itemChecker.correctFlag)) {
				//Animation for showing the correct options
				itemChecker.correctOptionIgnored();
			}
		}
	}
	// Use this for initialization
	void Start () {
		correctCount = 0;SelBtnGOList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
