using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TextLine : Line {

	//-------------Common Attributes -------------------
	public string DisplayText {get; set;}
	float fontSizeMultiplier{get; set;}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Contructor
	public TextLine(string displayText, string type):base(){
		DisplayText = StringWrapper.HtmlToPlainText(displayText);
		fontSizeMultiplier = 1.0f;
	}
	/// <summary>
	/// Initializes a new instance of the TextLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextLine(HtmlNode line_node):base(line_node){
		DisplayText = StringWrapper.HtmlToPlainText(line_node.InnerText);

		prefabName = LocationManager.NAME_LATEX_TEXT_LINE;
		HtmlAttribute attr_fontSize = line_node.Attributes [AttributeManager.ATTR_FONT_SIZE];
		if (attr_fontSize != null) {
			fontSizeMultiplier = float.Parse (attr_fontSize.Value);
		} else {
			fontSizeMultiplier = 1.0f;
		}
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

		TEXDrawNGUI texComponent = ElementGO.GetComponent<TEXDrawNGUI> ();
		if ( texComponent!= null) {
			//If it has TEX comnponent
			if (fontSizeMultiplier!=null){
				//If fontSizeMultiplier is provided
				texComponent.size = fontSizeMultiplier * texComponent.size;
			}
		}
		BasicGOOperation.setText(ElementGO,DisplayText);
	}
}
