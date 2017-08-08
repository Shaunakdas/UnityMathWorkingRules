using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TextLine : Line {

	//-------------Common Attributes -------------------
	public string DisplayText {get; set;}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Contructor
	public TextLine(string displayText, string type):base(){
		DisplayText = StringWrapper.HtmlToPlainText(displayText);
	}
	/// <summary>
	/// Initializes a new instance of the TextLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextLine(HtmlNode line_node):base(line_node){
		DisplayText = StringWrapper.HtmlToPlainText(line_node.InnerText);

		prefabName = LocationManager.NAME_LATEX_TEXT_LINE;
	}


	//-------------Based on Element Attributes, creating GameObject -------------------
	override public GameObject generateElementGO(GameObject parentGO){
		ElementGO = base.generateElementGO(parentGO);
		if (Type == LineType.PostSubmitText)
			ElementGO.GetComponent<UILabel> ().alpha = 0f;
		return ElementGO;
	}
	override protected void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
//		ElementGO.GetComponent<UILabel> ().text = DisplayText;
		BasicGOOperation.setText(ElementGO,DisplayText);

	}
}
