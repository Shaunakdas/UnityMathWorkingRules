using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTrigger : Singleton<MathTrigger> {

	protected MathTrigger () {} // guarantee this will be always a singleton only - can't use the constructor!

	public string questionText;
	public string sectionCompleteText = "Collect all ingredients";
	public void NextPara(){
		GameObject.Find ("UI Root").GetComponent<HTMLParserTest> ().comprehensionBody.nextParaTrigger ();
	}

	public void PopupScore(int value, Vector3 pos){
		LevelManager.THIS.PopupScore(value, pos, 2);
	}

	public void sectionComplete(){
		GameObject.Find("Canvas").transform.Find("SectionComplete").gameObject.SetActive(true);
	}
}
