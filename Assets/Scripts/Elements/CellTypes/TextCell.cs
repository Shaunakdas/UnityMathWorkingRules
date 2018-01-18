using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Net;
public class TextCell : Cell {
	//-------------Common Attributes -------------------


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Contructor
	public TextCell(string displayText, string type):base(type){
		DisplayText = StringWrapper.HtmlToPlainText(displayText);
		prefabName = LocationManager.NAME_LATEX_TEXT_CELL;
	}
	/// <summary>
	/// Initializes a new instance of the TextCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextCell(HtmlNode cell_node){
//		Debug.Log ("Initializing TextCell node of type "+ cell_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		DisplayText = StringWrapper.HtmlToPlainText(cell_node.InnerText);
		prefabName = LocationManager.NAME_LATEX_TEXT_CELL;
		parseChildNode (cell_node);

//		Debug.Log ("Found TextCell node of content"+ DisplayText);
	}
	/// <summary>
	/// Initializes a new instance of the TextCell class
	/// </summary>
	/// <param name="para">Para.</param>
	public TextCell(string text){
		DisplayText = text;
		prefabName = LocationManager.NAME_LATEX_TEXT_CELL;
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
		if (ElementGO.GetComponent<UILabel> () != null) {
			ElementGO.GetComponent<UILabel> ().text =(DisplayText) ;
		}
		if (ElementGO.GetComponent<TEXDrawNGUI> () != null) {
			ElementGO.GetComponent<TEXDrawNGUI> ().text =(DisplayText) ;
		}
	}
}
