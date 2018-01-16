using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerHolder : DropZoneHolder {
	/// <summary>
	/// Trigger Corect/Incorrect Animation after checking Composite Text.
	/// </summary>
	/// <returns><c>true</c>Returns user input result <c>false</c> otherwise.</returns>
	/// <param name="inputTextList">Input text list.</param>
	public override bool checkCompositeText(List<string> inputTextList){
		getQuestionChecker ();
		//Checking through TargetTextList;
		foreach (string targetText in TargetTextList) {
			Debug.Log("inputText"+inputTextList[0]+"targetText"+targetText);

			// Integer specific Code: Remove Sign before checking
			string targetTextCheck = targetText;
			bool integerCheck = integerTarget (targetText);
			if (integerCheck) {
				targetTextCheck = targetText.Substring(1, targetText.Length-1);
				inputTextList.RemoveAt (0);
			}
			Debug.Log("inputText"+inputTextList[0]+"targetText"+targetTextCheck );

			//Checking for TriedText with same length;
			if (targetTextCheck.Length == inputTextList.Count) {


				//Checking for matching exact text
				if ((noNullElement(inputTextList))&&(string.Concat(inputTextList.ToArray()) == targetTextCheck)) {
					//Removing targetText from targetTextList
					TargetTextList.Remove(targetText);
					return true;
				}
				int inputCharIndex = 0;
				bool foundMisMatch = true;
				//Iterating through whole list of characters of targetText
				foreach (string inputString in inputTextList) {
					foundMisMatch = false;

					// Checking for matching text after removing space
					if ((inputString != null)&&(inputString != targetTextCheck.ToCharArray()[inputCharIndex].ToString())) {
						foundMisMatch = true;
					}
					Debug.Log (foundMisMatch.ToString()+inputString);
					inputCharIndex ++;
					if (foundMisMatch) {
						break;
					}
				}
				if (!foundMisMatch) {
					return true;
				}
			}
		}
		return false;
	}

	void checkSign(){

	}


	void getQuestionChecker(){
		foreach (DropZoneQuestionChecker question in ChildList) {
			Debug.Log (question);
		}
		Debug.Log ("ItemCheckerMaskerList");
		foreach (List<OptionChecker> questionList in ItemCheckerMasterList) {
			foreach (OptionChecker option in questionList) {
				Debug.Log (option);
				Debug.Log (option.ItemAttemptState);
				Debug.Log (option.ParentChecker.ItemAttemptState);
			}
		}
	}

	protected override List<string> getCorrectionOptions(DropZoneOptionChecker _option, DropZoneQuestionChecker _ques){
		List<string> correctionOptions = new List<string> ();
		if (idCheck) {
			return TargetTextList;
		} else {
			List<string> inputTextList = new List<string>();
			addOptionText(inputTextList, _ques);
			//Checking through TargetTextList;
			foreach (string targetText in TargetTextList) {
				Debug.Log ("inputText" + inputTextList [0] + "targetText" + targetText);
				//Checking for TriedText with same length;
				if (targetText.Length == inputTextList.Count) {
					int inputCharIndex = 0;
					bool foundMisMatch = true;
					//Iterating through whole list of characters of targetText
					foreach (string inputString in inputTextList) {
						// Integer specific Code: Not checking sign option for correction
						if (!integerTarget (inputString)) {
							foundMisMatch = false;
							Debug.Log (inputString);
							Debug.Log (targetText.ToCharArray () [inputCharIndex].ToString ());
							// Checking for matching text after removing space
							if ((inputString != null) && (inputString != targetText.ToCharArray () [inputCharIndex].ToString ())) {
								foundMisMatch = true;
							}
							if (foundMisMatch) {
								break;
							}
						}
						inputCharIndex++;
					}
					if (!foundMisMatch) {
						Debug.Log (targetText.ToCharArray () [_option.getSiblingIndex ()].ToString());
						correctionOptions.Add (targetText.ToCharArray () [_option.getSiblingIndex ()].ToString());
					}
				}
			}
		}
		return correctionOptions;
	}
}
