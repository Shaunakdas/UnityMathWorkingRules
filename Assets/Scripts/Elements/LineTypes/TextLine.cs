﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TextLine : Line {
	
	public string DisplayText {get; set;}

	//Contructor
	public TextLine(string displayText, string type){
		RowList = new List<Row>();	
		DisplayText = displayText;
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TextLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextLine(HtmlNode line_node){
		RowList = new List<Row> ();
		Debug.Log ("Initializing TextLine node of type " + line_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		DisplayText = line_node.InnerText;
		getLineType (line_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		if (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE] != null) {
			getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);
		} else {
			getLocationType ("");
		}
		prefabName = LocationManager.NAME_TEXT_LINE;
		parseChildNode (line_node);
	}
	override public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.Text;
			break;
		case "post_submit_text": 
			Type = LineType.PostSubmitText;
			break;
		case "incorrect_submit_text": 
			Type = LineType.IncorrectSubmitText;
			break;
		}
	}

	override public void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
		ElementGO.GetComponent<UILabel> ().text = DisplayText;
	}
}
