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


	public ScoreCalculator scoreCalc;

	public GameObject ElementGO;
	public string prefabName;




	//-------------Constructor and Calculation -------------------
	public ScoreManager(){
		scoreCalc = new ScoreCalculator ();
		prefabName = LocationManager.NAME_SCORE_BOARD;
	}
	public void init(ComprehensionBody _body){
		//		scoreCalc.maxIt
		parentBody = _body;
	}
	public void setCurrentPara(Paragraph _para){
		currentPara = _para; 
	}
	public float calcMaxTotalScore(){
//		return (float) ScoreCalculator.MaxTotalScore;
		return 0f;
	}

	public float calcMaxParaScore(int paraCount, float maxTotalSocre){
		scoreCalc.maxParaScore = maxTotalSocre / paraCount;
		return scoreCalc.maxParaScore;
	}

	public float calcMaxItemScore(int targetCount, float maxParaScore){
		scoreCalc.maxItemScore = maxParaScore / targetCount;
		return scoreCalc.maxItemScore;
	}

	public void calcTotalTimeAllotted(){
		
	}

	public void calcParaTimeAllotted(){

	}
	public void setupScoreSettings(List<Paragraph> _paraList){
		foreach (Paragraph para in _paraList) {
			ScoreSettings settings = para.scoreSettings;
			//Maximum Score
			settings.maxParaScore = scoreCalc.maxParaScore;
			settings.maxItemScore = scoreCalc.maxParaScore/settings.maxCorrectCount;
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
		setupScoreDisplay(ElementGO,currentPara);
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
		float deltaScore = scoreCalc.itemScore (Result.Correct, _timeTaken);
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
