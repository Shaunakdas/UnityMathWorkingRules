using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSettings {
	//Time Settings
	public float maxParaTimeAllotted = 0f, maxItemTimeAllotted = 0f, minItemTimeAllotted = 0f;
	//Score Settings
	public float maxParaScore = 0f, maxItemScore=0f, minItemScore =0f;
	//Live Settings
	public int maxParaLives = 0;
	public int maxCorrectCount = 0;
	//Display Settings
	public bool timerDisplay = true, livesDisplay = true, scoreDisplay = true;
	//References
	public Paragraph Parent;
	public ScoreSettings(){
	}
	public ScoreSettings(Paragraph _para){
		Parent = _para;
	}
	public float itemScore(ScoreManager.Result _itemResult, float _timeTaken){
		float _itemScore = 0f;
		//		float maxScore = maxItemScore,minScore = minItemScore,maxTime = MAX_ITEM_TIME,minTime = MIN_ITEM_TIME;
		switch (_itemResult) {
		case ScoreManager.Result.Correct:
			_itemScore = ScoreDefaults.correctScoreFormula( _timeTaken, minItemTimeAllotted, maxItemTimeAllotted, minItemScore, maxItemScore);
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
}
