using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class Paragraph : BaseElement{
	public enum StepType {Comprehension,QuestionStep};
	public StepType ParagraphStep;

	//For all types
	public List<Line> LineList{get; set;}

	//For QuestionStep 
	public enum CorrectType {SingleCorrect,MultipleCorrect}
	public CorrectType ParagraphCorrect;

	/// <summary>
	/// Initializes a new instance of the Paragraph class with correctType attribute
	/// </summary>
	/// <param name="correctType">Correct type.</param>
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
	/// <summary>
	/// Initializes a new instance of the Paragraph class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public Paragraph(HtmlNode para_node){
		LineList = new List<Line> ();
		Debug.Log ("Initializing paragraph node of type "+para_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		switch (para_node.Attributes [HTMLParser.ATTR_TYPE].Value) {
		case "single_correct": 
			ParagraphStep = StepType.Comprehension;
			break;
		case "multiple_correct": 
			ParagraphStep = StepType.QuestionStep;
			switch (para_node.Attributes [HTMLParser.ATTR_CORRECT_TYPE].Value) {
			case "comprehension": 
				ParagraphCorrect = CorrectType.SingleCorrect;
				break;
			case "question_step": 
				ParagraphCorrect = CorrectType.MultipleCorrect;
				break;
			}
			break;
		}

	}
	/// <summary>
	/// Parses the Paragraph Node to generate Line nodes
	/// </summary>
	public void parseParagraph(HtmlNode para_node){
//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = para_node.Elements(HTMLParser.LINE_TAG) ;

		if (node_list != null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode line_node in node_list) {
				Line newLine = new Line (line_node);
				string line_type = line_node.Attributes [HTMLParser.ATTR_TYPE].Value;
				switch (line_type) {
				case "text": 
					newLine = new TextLine (line_node);
					break;
				case "post_submit_text": 
					newLine = new TextLine (line_node);
					break;
				case "incorrect_submit_text": 
					newLine = new TextLine (line_node);
					break;
				case "table": 
					newLine = new TableLine (line_node);
					break;
				case "prime_division": 
					newLine = new PrimeDivisionLine (line_node);
					break;
				case "number_line_drop": 
					newLine = new NumberLineDropLine (line_node);
					break;
				case "combination": 
					newLine = new CombinationLine (line_node);
					break;
				}
				//Populate Child Row nodes inside Line Node
				newLine.parseLine (line_node);

				newLine.Parent = this;
				//Add Line node into LineList
				LineList.Add (newLine);

			}
		}
	}
}
