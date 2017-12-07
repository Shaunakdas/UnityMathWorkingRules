using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager:BaseElement  {
	//-------------Common Attributes -------------------

	public const int PARA_LIVE_COUNT = 3;
	public const bool PARA_LIVE_DISPLAY = true;
	public const bool PARA_TIME_DISPLAY = true;
	public const bool SCORE_DISPLAY = true;

	public enum Result{Correct,Incorrect,Seen,Timeout}
	public ComprehensionBody parentBody;


//	public ScoreDefaults scoreCalc;
	public ScoreDisplayManager scoreDisplay;
	public int difficultyLevel = 1;
	public int resultStar = 0;


	//-------------Constructor and Calculation -------------------
	public ScoreManager(){
		prefabName = LocationManager.NAME_SCORE_BOARD;
	}
	public ScoreManager(ComprehensionBody _body){
		prefabName = LocationManager.NAME_SCORE_BOARD;
		parentBody = _body;
	}
	 void init(ComprehensionBody _body){
		//		scoreCalc.maxIt
		parentBody = _body;
	}
	 void setCurrentPara(Paragraph _para){
		ParagraphRef = _para; 
	}
	 float calcMaxTotalScore(){
//		return (float) ScoreCalculator.MaxTotalScore;
		return 0f;
	}

	 void calcTotalTimeAllotted(){
		
	}

	 void calcParaTimeAllotted(){

	}
	//-------------Generate ElementGO -------------------
	override public GameObject generateElementGO(GameObject _parentGO){
		GameObject ScoreBoardPF = Resources.Load (LocationManager.COMPLETE_LOC_SCORE_TYPE + prefabName)as GameObject;
		ElementGO = BasicGOOperation.InstantiateNGUIGO(ScoreBoardPF,_parentGO.transform);
		initGOProp (ElementGO);
		setupScoreDisplay(ElementGO);
		updateGOProp (ElementGO);
		BasicGOOperation.RepositionChildTables (ElementGO);
		return ElementGO;
	}
	override protected void initGOProp(GameObject _elementGO){

	}
	override protected void updateGOProp(GameObject _elementGO){
		setupScoreDisplay(_elementGO);
		ScreenManager.SetAsScreenTop (_elementGO);
		ScreenManager.SetAsScreenLeft(BasicGOOperation.getChildGameObject (_elementGO, "LeftTable"));
	}
	void setupScoreDisplay(GameObject _elementGO){
		scoreDisplay = _elementGO.GetComponent<ScoreDisplayManager> ();
	}
	//-------------Score Display Changes -------------------
	public void setupTimer(float _startTime){
		scoreDisplay.setupTimer (_startTime);
	}
	public void updateScore(float _deltaScore){
		scoreDisplay.updateScore (_deltaScore);
	}
	public void setupLives(int _deltaLives){
		scoreDisplay.setupLives (_deltaLives);
	}
	public void updateLives(int _deltaLives){
		scoreDisplay.updateLives (_deltaLives);
	}

	//-------------Animations -------------------
	public void correctAnim(float _timeTaken){
//		float deltaScore = scoreCalc.itemScore (Result.Correct, _timeTaken);
		float deltaScore = 0f;
		ElementGO.GetComponent<ScoreDisplayManager> ().updateScore ((int)deltaScore);
	}
	public void correctAnim(int _attemptScore){
		if (_attemptScore != 0) {
			ElementGO.GetComponent<ScoreDisplayManager> ().updateScore (_attemptScore);
		}
	}
	public void incorrectAnim(){
		ElementGO.GetComponent<ScoreDisplayManager> ().updateLives (-1);
	}

}
