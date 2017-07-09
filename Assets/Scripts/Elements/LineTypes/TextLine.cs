using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TextLine : Line {

	//-------------Common Attributes -------------------
	public string DisplayText {get; set;}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Contructor
	public TextLine(string displayText, string type){
		RowList = new List<Row>();	
		DisplayText = StringWrapper.HtmlToPlainText(displayText);
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TextLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextLine(HtmlNode line_node):base(line_node){
		DisplayText = StringWrapper.HtmlToPlainText(line_node.InnerText);

		prefabName = LocationManager.NAME_LATEX_TEXT_LINE;
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


	//-------------Based on Element Attributes, creating GameObject -------------------
	override public GameObject generateElementGO(GameObject parentGO){
		ElementGO = base.generateElementGO(parentGO);
		if (Type == LineType.PostSubmitText)
			ElementGO.GetComponent<UILabel> ().alpha = 0f;
		return ElementGO;
	}
	override public void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
//		ElementGO.GetComponent<UILabel> ().text = DisplayText;
		BasicGOOperation.setText(ElementGO,DisplayText);
	}
}
