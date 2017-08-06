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
		return ScoreDefaults.correctScoreFormula (timeTaken, idealTime, maxTime, minScore, maxScore);
	}

	public void notifyScoreDisplay (float timeTaken, Paragraph para){
		
	}
}
