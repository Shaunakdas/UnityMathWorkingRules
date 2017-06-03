using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paragraph {
	public enum ParagraphType {Comprehension,QuestionStep};

	//For all types
	public List<Line> LineList{get; set;}

	//For QuestionStep 
	public enum CorrectType {SingleCorrect,MultipleCorrect}
	public CorrectType ParagraphCorrect;
	//Constructor
	public Paragraph(string correctType){
		switch (correctType) {
		case "single_correct": 
			ParagraphCorrect = CorrectType.SingleCorrect;
			break;
		case "multiple_correct": 
			ParagraphCorrect = CorrectType.MultipleCorrect;
			break;
		}
	}
}
