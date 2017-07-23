using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class CombinationLine  : Line {
	public bool OutputVisible{ get; set; }
	public int CorrectAnswer{get; set;}
//	public List<Row> RowList {get; set;}
	//Constructor
	public CombinationLine(string outputVisible, string correctAnswer, string type):base(){
		OutputVisible = (outputVisible=="1")?true : false;
		CorrectAnswer = int.Parse (correctAnswer);
	}
	/// <summary>
	/// Initializes a new instance of the CombinationLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public CombinationLine(HtmlNode line_node):base(line_node){
		Debug.Log ("Initializing CombinationLine node of type text"+ line_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		CorrectAnswer = int.Parse(line_node.Attributes [AttributeManager.ATTR_ANSWER].Value);
		OutputVisible = (line_node.Attributes [AttributeManager.ATTR_OUTPUT_VISIBLE].Value=="1")?true : false;
		prefabName = LocationManager.NAME_COMBINATION_LINE;
	}
}
