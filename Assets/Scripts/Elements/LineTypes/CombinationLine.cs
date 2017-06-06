using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class CombinationLine  : Line {
	public bool OutputVisible{ get; set; }
	public int CorrectAnswer{get; set;}
//	public List<Row> RowList {get; set;}
	//Constructor
	public CombinationLine(string outputVisible, string correctAnswer, string type){
		RowList = new List<Row>();	
		OutputVisible = (outputVisible=="1")?true : false;
		CorrectAnswer = int.Parse (correctAnswer);
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the CombinationLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public CombinationLine(HtmlNode line_node){
		RowList = new List<Row>();
		Debug.Log ("Initializing CombinationLine node of type text"+ line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		CorrectAnswer = int.Parse(line_node.Attributes [HTMLParser.ATTR_ANSWER].Value);
		getLineType (line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		OutputVisible = (line_node.Attributes [HTMLParser.ATTR_OUTPUT_VISIBLE].Value=="1")?true : false;
		getLocationType (line_node.Attributes [HTMLParser.ATTR_LOCATION_TYPE].Value);
	}
	public void getLineType(string type_text){
		switch (type_text) {
		case "combination_product": 
			Type = LineType.CombinationProduct;
			break;
		case "combination_product_sum": 
			Type = LineType.CombinationProductSum;
			break;
		case "combination_sum": 
			Type = LineType.CombinationSum;
			break;
		}
	}
}
