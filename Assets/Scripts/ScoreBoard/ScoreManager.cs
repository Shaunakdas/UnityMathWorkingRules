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
	public void init(ComprehensionBody _body){
		//		scoreCalc.maxIt
		parentBody = _body;
	}
	public void setCurrentPara(Paragraph _para){
		ParagraphRef = _para; 
	}
	public float calcMaxTotalScore(){
//		return (float) ScoreCalculator.MaxTotalScore;
		return 0f;
	}

	public void calcTotalTimeAllotted(){
		
	}

	public void calcParaTimeAllotted(){

	}
	public void setupScoreSettings(List<Paragraph> _paraList){
		foreach (Paragraph para in _paraList) {
			ScoreSettings settings = para.scoreSettings;
			//Maximum Time
//			settings.maxItemTimeAllotted = ScoreCalculator.MaxQuestionTime; settings.minItemTimeAllotted = ScoreCalculator.MaxQuestionTime;
			settings.maxParaTimeAllotted = settings.maxCorrectCount * settings.maxItemTimeAllotted;
			//Maximum Lives
//			settings.maxParaLives = ScoreCalculator.MaxTotalLives;
		}
	}
	//-------------Generate ElementGO -------------------
	public void generateElementGO(GameObject _parentGO){
		GameObject ScoreBoardPF = Resources.Load (LocationManager.COMPLETE_LOC_SCORE_TYPE + prefabName)as GameObject;
		ElementGO = BasicGOOperation.InstantiateNGUIGO(ScoreBoardPF,_parentGO.transform);
		initGOProp (ElementGO);
		setupScoreDisplay(ElementGO,ParagraphRef);
		updateGOProp (ElementGO);
		BasicGOOperation.RepositionChildTables (ElementGO);
	}
	public void initGOProp(GameObject _elementGO){

	}
	public void setupScoreDisplay(GameObject _elementGO,Paragraph _para){
		ScoreDisplayManager scoreDisplay = _elementGO.GetComponent<ScoreDisplayManager> ();
		scoreDisplay.livesPending = _para.scoreSettings.maxParaLives; 
		scoreDisplay.timePending = _para.scoreSettings.maxParaTimeAllotted;
	}
	public void updateGOProp(GameObject _elementGO){
		ScreenManager.SetAsScreenTop (_elementGO);
		
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
