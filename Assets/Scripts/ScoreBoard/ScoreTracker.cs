using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class ScoreTracker {

	//Max Score Variables
	public float maxScore,minScore;
	public float scoreWeightage=1,childScoreWeightageSum=0;
	//Attempt Score Variables
	public float attemptScore;
	//Max Time Variables
	public float idealTime,maxTime;
	//Attempt Time Variables
	public System.DateTime attemptTime;
	public System.DateTime startTimestamp,attemptTimestamp,endTimestamp;
	//Attempt Status
	public OptionChecker.AttemptState state;
	//Reference
	public TargetEntity entity;
	//-------------Constructor -------------------

	public ScoreTracker(){
	}
	//-------------Setters and Getters -------------------
	public void setMaxScore(HtmlNode node,string attr){
		if (node.Attributes [attr] != null) {
			maxScore = float.Parse(node.Attributes [attr].Value);
		} 
	}
	public void updateMaxScore(float value){
		if (maxScore == null) {
			maxScore = value;
		} 
	}
	public void setMinScore(HtmlNode node,string attr){
		if (node.Attributes [attr] != null) {
			minScore = float.Parse(node.Attributes [attr].Value);
		} 
	}
	public void updateMinScore(float value){
		if (minScore == null) {
			minScore = value;
		} 
	}
	public void setScoreWeightage(HtmlNode node,string attr){
		if (node.Attributes [attr] != null) {
			scoreWeightage = float.Parse(node.Attributes [attr].Value);
		} 
	}
	public void setIdealTime(HtmlNode node,string attr,float defaultValue){
		if (node.Attributes [attr] != null) {
			idealTime = float.Parse (node.Attributes [attr].Value);
		} else {
			idealTime = defaultValue;
		}
	}
	public void setMaxTime(HtmlNode node,string attr,float defaultValue){
		if (node.Attributes [attr] != null) {
			maxTime = float.Parse(node.Attributes [attr].Value);
		}  else {
			maxTime = defaultValue;
		}
	}
}
