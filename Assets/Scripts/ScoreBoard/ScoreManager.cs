using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager  {

	public const int PARA_LIVE_COUNT = 3;
	public const bool PARA_LIVE_DISPLAY = true;
	public const bool PARA_TIME_DISPLAY = true;
	public const bool SCORE_DISPLAY = true;


	public Paragraph currentPara;
	public ComprehensionBody parentBody;

	public float currentScore = 0f;
	public int livesPending = 0;

	public ScoreCalculator scoreCalc;

	public void init(int paraCount){
//		scoreCalc.maxIt
	}

	public ScoreManager(){
		scoreCalc = new ScoreCalculator ();
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
}
