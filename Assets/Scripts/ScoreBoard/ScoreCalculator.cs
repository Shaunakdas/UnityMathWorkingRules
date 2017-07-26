using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator  {
	public const int MAX_SCORE_VALUE = 3000,MIN_SCORE_VALUE = 1000;
	public const float MIN_ITEM_TIME = 0.33f;
	public const float MAX_ITEM_TIME = 0.66f;
	public float maxItemScore=0f,minItemScore=0f;
	public enum Result{Correct,Incorrect,Seen,Timeout}

	public ScoreCalculator(){
	}

	public void generateAttrs(int _questionCount){
		maxItemScore = MAX_SCORE_VALUE / _questionCount;
		minItemScore = maxItemScore / 3;
	}
	public float itemScore(Result _itemResult, float _timeTaken){
		float _itemScore = 0f;
//		float maxScore = maxItemScore,minScore = minItemScore,maxTime = MAX_ITEM_TIME,minTime = MIN_ITEM_TIME;
		switch (_itemResult) {
		case Result.Correct:
			_itemScore = correctScoreFormula( _timeTaken, MIN_ITEM_TIME, MAX_ITEM_TIME, minItemScore, maxItemScore);
			break;
		case Result.Incorrect:
			_itemScore = 0f;
			break;
		case Result.Timeout:
			_itemScore = 0f;
			break;
		}
		return _itemScore;
	}
	public float correctScoreFormula(float _timeTaken, float _minTime, float _maxTime, float _minScore, float _maxScore){
		return _maxScore - ((_timeTaken - _minTime) * (_maxScore - _minScore) / (_maxTime - _minTime));
	}

}
