using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TextCell : Cell {
	public CellType TextType{get; set;}

	public string DisplayText {get; set;}

	//Contructor
	public TextCell(string displayText, string type){
		DisplayText = displayText;
		getCellType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TextCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TextCell(HtmlNode cell_node){
		Debug.Log ("Initializing TextCell node of type "+ cell_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		DisplayText = cell_node.InnerText;
		getCellType (cell_node.Attributes [HTMLParser.ATTR_TYPE].Value);

//		Debug.Log ("Found TextCell node of content"+ DisplayText);
	}
	public void getCellType(string type_text){
		switch (type_text) {
		case "text": 
			TextType = CellType.Text;
			break;
		}
	}
}
