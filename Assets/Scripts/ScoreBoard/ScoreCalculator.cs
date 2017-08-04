using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator  {
	public const float DEFAULT_MAX_SCORE=3000f,DEFAULT_MIN_SCORE=3000f,DEFAULT_MAX_QUES_SCORE=1000f,DEFAULT_MIN_QUES_SCORE=1000f,DEFAULT_MAX_OPTION_SCORE=1000f,DEFAULT_MIN_OPTION_SCORE=1000f;
	public const float DEFAULT_SCORE_WEIGHTAGE=1f;
	//Time Tracking
	public const float DEFAULT_MAX_TIME=10f,DEFAULT_IDEAL_TIME=10f,DEFAULT_MAX_QUES_TIME=10f,DEFAULT_IDEAL_QUES_TIME=10f,DEFAULT_MAX_OPTION_TIME=10f,DEFAULT_IDEAL_OPTION_TIME=10f;

	public int MaxTotalScore = 3000,MinTotalScore = 1000;

	public int MaxTotalLives = 3;
	public float maxItemScore=0f,minItemScore=0f,maxParaScore=0f,minParaScore=0f;

	//Time Calculations
	public float MinQuestionTime = 0.33f, MaxQuestionTime = 0.66f, MinOptionTime = 0.66f, MaxOptionTime = 0.66f;
	public float DragGraceTime = 0.1f;

	public ScoreCalculator(){
	}

	public void generateAttrs(int _questionCount){
		maxItemScore = MaxTotalScore / _questionCount;
		minItemScore = maxItemScore / 3;
	}
	public float itemScore(ScoreManager.Result _itemResult, float _timeTaken){
		float _itemScore = 0f;
//		float maxScore = maxItemScore,minScore = minItemScore,maxTime = MAX_ITEM_TIME,minTime = MIN_ITEM_TIME;
		switch (_itemResult) {
		case ScoreManager.Result.Correct:
			_itemScore = correctScoreFormula( _timeTaken, MinQuestionTime, MaxQuestionTime, minItemScore, maxItemScore);
			break;
		case ScoreManager.Result.Incorrect:
			_itemScore = 0f;
			break;
		case ScoreManager.Result.Timeout:
			_itemScore = 0f;
			break;
		}
		return _itemScore;
	}
	public static float correctScoreFormula(float _timeTaken, float _minTime, float _maxTime, float _minScore, float _maxScore){
		return _maxScore - ((_timeTaken - _minTime) * (_maxScore - _minScore) / (_maxTime - _minTime));
	}

}
