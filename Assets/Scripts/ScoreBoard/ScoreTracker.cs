using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class ScoreTracker {

	//Max Score Variables
	public float maxScore,minScore;
	public float scoreWeightage=1,childScoreWeightageSum=0;

	public int maxLives;
	//Attempt Score Variables
	public float attemptScore;


	//Max Time Variables
	public float idealTime,maxTime,timeAllotted;
	//Attempt Time Variables
	public System.TimeSpan attemptTime;
	public System.DateTime startTimestamp,attemptTimestamp,endTimestamp;


	//Star Calculations
	public int attemptStar=0;
	//DifficultyLevel Star Calculations
	public int currentDiffLevel=0,nextDiffLevel=0;
	public int childCorrectCount = 0;
	//Attempt Status
	public OptionChecker.AttemptState state;
	//Reference
	public TargetEntity entity;
	//-------------Constructor -------------------

	public ScoreTracker(){
	}
	public float calcScore (float timeTaken){
		attemptTime = System.TimeSpan.FromSeconds(timeTaken);
		return ScoreDefaults.correctScoreFormula (attemptTime.Seconds, idealTime, maxTime, minScore, maxScore);
	}

	public void notifyManager (Paragraph para,ScoreManager.Result _result){
		Debug.Log ("Changing Display" + _result.ToString ());
		switch (_result) {
		case ScoreManager.Result.Correct:
			(para.Parent as ComprehensionBody).scoreMan.updateScore (attemptScore);
			break;
		case ScoreManager.Result.Incorrect:
			(para.Parent as ComprehensionBody).scoreMan.updateLives (-1);
			break;
		case ScoreManager.Result.Timeout:
			(para.Parent as ComprehensionBody).scoreMan.updateLives (-1);
			break;
		}
	}
	public void setSharedPrefs(){
		GameOutput output = new GameOutput (attemptScore, (float)attemptTime.TotalSeconds, childCorrectCount, 0, true, true, false);
		output.setOutputPrefs ();
	}
}
