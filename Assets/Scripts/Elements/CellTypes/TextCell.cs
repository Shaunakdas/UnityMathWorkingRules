using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Net;
public class TextCell : Cell {

	public string DisplayText {get; set;}

	//Contructor
	public TextCell(string displayText, string type){
		DisplayText = StringWrapper.HtmlToPlainText(displayText);
		getCellType (type);
		prefabName = LocationManager.NAME_LATEX_TEXT_CELL;
	}
	/// <summary>
	/// Initializes a new instance of the TextCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextCell(HtmlNode cell_node){
		Debug.Log ("Initializing TextCell node of type "+ cell_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		DisplayText = StringWrapper.HtmlToPlainText(cell_node.InnerText);
		getCellType (cell_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		prefabName = LocationManager.NAME_LATEX_TEXT_CELL;

//		Debug.Log ("Found TextCell node of content"+ DisplayText);
	}
	public void getCellType(string type_text){
		switch (type_text) {
		case "text": 
			Type = CellType.Text;
			break;
		}
	}
	override public void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
		if (ElementGO.GetComponent<UILabel> () != null) {
			
			ElementGO.GetComponent<UILabel> ().text =(DisplayText) ;
		}
		if (ElementGO.GetComponent<TEXDrawNGUI> () != null) {
			ElementGO.GetComponent<TEXDrawNGUI> ().text =(DisplayText) ;
		}
	}
}
