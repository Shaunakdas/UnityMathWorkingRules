using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

	override protected void setupDefaultScoreValues (){
		if (scoreTracker.maxScore == 0) { scoreTracker.maxScore = ScoreDefaults.DEFAULT_MAX_QUES_SCORE; }
		if (scoreTracker.minScore == 0) { scoreTracker.minScore = ScoreDefaults.DEFAULT_MIN_QUES_SCORE; }
		if (scoreTracker.maxTime == 0) { scoreTracker.maxTime = ScoreDefaults.DEFAULT_MAX_QUES_TIME; }
		if (scoreTracker.idealTime == 0) { scoreTracker.idealTime = ScoreDefaults.DEFAULT_IDEAL_QUES_TIME; }
		if (scoreTracker.scoreWeightage == 0) { scoreTracker.scoreWeightage = ScoreDefaults.DEFAULT_SCORE_WEIGHTAGE; }
	}
	override protected void adjustForWeightage(){
		//Setting maxScore/minScore based on scoreWeightages
		float sumOfScoreWeightages = ContainerElem.ParagraphRef.scoreTracker.childScoreWeightageSum;
		if (sumOfScoreWeightages == 0) {
			foreach (Line line in ContainerElem.ParagraphRef.LineList) {
				foreach (QuestionChecker ques in line.QuestionList) {
					sumOfScoreWeightages += ques.scoreTracker.scoreWeightage;
				}
			}
		} 
		scoreTracker.maxScore = scoreTracker.scoreWeightage*(ContainerElem.ParagraphRef.scoreTracker.maxScore / sumOfScoreWeightages);
		scoreTracker.minScore = scoreTracker.scoreWeightage*(ContainerElem.ParagraphRef.scoreTracker.minScore / sumOfScoreWeightages);
	}
	override protected void  setupScoreValues(){

		//Setting scoreTracker of options by diving equally
		foreach (OptionChecker option in ChildList) {
			option.scoreTracker.maxScore = scoreTracker.maxScore / ChildList.Count;
			option.scoreTracker.minScore = scoreTracker.minScore / ChildList.Count;
			option.scoreTracker.maxTime = scoreTracker.maxTime / ChildList.Count;
			option.scoreTracker.idealTime = scoreTracker.idealTime / ChildList.Count;
		}
	}

	//----------------------Animation ----------------------------
	public void optionAttemptTracker(OptionChecker option){

	}

}
