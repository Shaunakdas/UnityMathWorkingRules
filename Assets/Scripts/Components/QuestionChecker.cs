using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionChecker : OptionChecker {
	
	//Holder to hold list of TargetItemChecker
	void awake(){
		ItemTargetType=TargetType.Question;
	}
	virtual public void addToMasterLine(){ 
//		BasicGOOperation.getMasterLineRef(ContainerElem).QuestionList.Add(this);
		Debug.Log(ContainerElem.GetType().ToString());
		addToContainerElemList (ContainerElem);
		addToList (this, BasicGOOperation.getMasterLineRef(ContainerElem).QuestionList);
	}
	virtual public void addToMasterLine(Line _lineRef){ 
//		_lineRef.QuestionList.Add(this);
		addToContainerElemList (ContainerElem);
		addToList (this, _lineRef.QuestionList);
	}
	void addToContainerElemList(BaseElement _containerElem){
		if(_containerElem.GetType ().IsSubclassOf(typeof(Cell))){
			addToList (this, (_containerElem as Cell).QuestionList);
		}
	}
	void addToList(QuestionChecker question,List<QuestionChecker> quesList){
		if (quesList.IndexOf (question) < 0) {
			quesList.Add (question);
		}
	}

	//----------------------Score Values ----------------------------

	override public void  setChildScoreValues(){
		setupDefaultScoreValues ();
		setupScoreValues ();
		foreach (OptionChecker option in ChildList) {
			option.setChildScoreValues ();
		}
	}
	void setupDefaultScoreValues (){
		if (scoreTracker.maxScore == null) { scoreTracker.maxScore = ScoreCalculator.DEFAULT_MAX_QUES_SCORE; }
		if (scoreTracker.minScore == null) { scoreTracker.maxScore = ScoreCalculator.DEFAULT_MIN_QUES_SCORE; }
		if (scoreTracker.maxTime == null) { scoreTracker.maxScore = ScoreCalculator.DEFAULT_MAX_QUES_TIME; }
		if (scoreTracker.idealTime == null) { scoreTracker.maxScore = ScoreCalculator.DEFAULT_IDEAL_QUES_TIME; }
		if (scoreTracker.scoreWeightage == null) { scoreTracker.maxScore = ScoreCalculator.DEFAULT_SCORE_WEIGHTAGE; }
	}
	override public void  setupScoreValues(){
		//Setting maxScore/minScore based on scoreWeightages
		float sumOfScoreWeightages = ContainerElem.ParagraphRef.scoreTracker.childScoreWeightageSum;
		if (sumOfScoreWeightages == null) {
			sumOfScoreWeightages = 0;
			foreach (Line line in ContainerElem.ParagraphRef.LineList) {
				foreach (QuestionChecker ques in line.QuestionList) {
					sumOfScoreWeightages += ques.scoreTracker.scoreWeightage;
				}
			}
		} 

		//Setting scoreTracker of options by diving equally
		foreach (OptionChecker option in ChildList) {
			option.scoreTracker.maxScore = scoreTracker.maxScore / ChildList.Count;
			option.scoreTracker.minScore = scoreTracker.minScore / ChildList.Count;
			option.scoreTracker.maxTime = scoreTracker.maxTime / ChildList.Count;
			option.scoreTracker.idealTime = scoreTracker.idealTime / ChildList.Count;
		}
	}
}
