using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager  {
	//-------------Common Attributes -------------------

	public const int PARA_LIVE_COUNT = 3;
	public const bool PARA_LIVE_DISPLAY = true;
	public const bool PARA_TIME_DISPLAY = true;
	public const bool SCORE_DISPLAY = true;

	public enum Result{Correct,Incorrect,Seen,Timeout}
	public Paragraph currentPara;
	public ComprehensionBody parentBody;

	public float currentScore = 0f;
	public int livesPending = 0;

	public ScoreCalculator scoreCalc;

	public GameObject ElementGO;
	public string prefabName;

	//-------------Constructor and Calculation -------------------
	public ScoreManager(){
		scoreCalc = new ScoreCalculator ();
		prefabName = LocationManager.NAME_SCORE_BOARD;
	}
	public void init(int paraCount){
		//		scoreCalc.maxIt
	}
	public float calcMaxTotalScore(){
		return 0f;
	}

	public float calcMaxParaScore(int paraCount, float maxTotalSocre){
		return maxTotalSocre / paraCount;
	}

	public void calcTotalTimeAllotted(){
		
	}

	public void calcParaTimeAllotted(){

	}
	//-------------Generate ElementGO -------------------
	public void generateElementGO(GameObject _parentGO){
		GameObject ScoreBoardPF = Resources.Load (LocationManager.COMPLETE_LOC_SCORE_TYPE + prefabName)as GameObject;
		ElementGO = BasicGOOperation.InstantiateNGUIGO(ScoreBoardPF,_parentGO.transform);
		ScreenManager.SetAsScreenTop (ElementGO);
	}

	//-------------Animations -------------------
	public void correctAnim(float _timeTaken){
		float deltaScore = scoreCalc.itemScore (Result.Correct, _timeTaken);
		ElementGO.GetComponent<ScoreDisplayManager> ().updateScore ((int)deltaScore);
	}
	public void incorrectAnim(){
		ElementGO.GetComponent<ScoreDisplayManager> ().updateLives (-1);
	}
}
