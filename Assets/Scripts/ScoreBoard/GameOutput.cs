using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public class GameOutput {
	float score {get; set;}
	float time_taken{get; set;}
	int correct_count{get; set;}
	int incorrect_count{get; set;}
	bool seen{get; set;}
	bool passed{get; set;}
	bool failed{get; set;}

	public GameOutput(){
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="GameOutput"/> class with all score fields.
	/// </summary>
	/// <param name="_score">Score.</param>
	/// <param name="_time_taken">Time taken.</param>
	/// <param name="_correct_count">Correct count.</param>
	/// <param name="_incorrect_count">Incorrect count.</param>
	/// <param name="_seen">If set to <c>true</c> seen.</param>
	/// <param name="_passed">If set to <c>true</c> passed.</param>
	/// <param name="_failed">If set to <c>true</c> failed.</param>
	public GameOutput(float _score, float _time_taken, int _correct_count, int _incorrect_count, bool _seen, bool _passed, bool _failed){
		score = _score;
		time_taken = _time_taken;
		correct_count = _correct_count;
		incorrect_count = _incorrect_count;
		seen = _seen;
		passed	= _passed;
		failed = _failed;
	}
	/// <summary>
	/// Convert GameOutput into JSON and stringify it.
	/// </summary>
	/// <returns>stringified GameOutput JSON</returns>
	string outputString(){
		JSONNode node = new JSONClass();
		node["score"]=score.ToString();		
		node["time_taken"]=time_taken.ToString();		
		node["correct_count"]=correct_count.ToString();		
		node["incorrect_count"]=incorrect_count.ToString();		
		node["seen"]=seen.ToString();
		node["passed"]=passed.ToString();
		node["failed"]=failed.ToString();
		Debug.Log("Generating output String as "+node.ToString());
		return node.ToString();
	}
	/// <summary>
	/// Set GameResult Prefs to outputString() and ShowResult to 1
	/// </summary>
	public void setOutputPrefs(){
		AndroidOrganizer.setSharedPrefs ("GameResult", outputString ());
		AndroidOrganizer.setSharedPrefs ("ShowResult", "1");
	}

}
