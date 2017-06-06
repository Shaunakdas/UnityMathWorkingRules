using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HtmlAgilityPack;

public class DropZoneRowCell : Cell {

	public string CellId{ get; set; } 
	public string CellTag{ get; set; }
	public string DisplayText{ get; set; }

	//Dropable
	public bool Dropable{get; set;}
	public bool Touchable{get; set;}

	//Target Text
	string _targetText;
	public string TargetText{ 
		get { return _targetText; }
		set { _targetText = value; generateTargetTextList (value); }
	}
	public List<string> TargetTextList{ get; set; }

	//Target Id
	string _targetId;
	public string TargetId{ 
		get { return _targetId; }
		set { _targetId = value; generateTargetIdList (value); }
	}
	public List<string> TargetIdList{ get; set; }
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	public void getCellType(string type_text){
		switch (type_text) {
		case "drop_zone_row": 
			Type = CellType.DropZoneRow;
			break;
		case "drop_zone": 
			Type = CellType.DropZone;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the DropZoneRowCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public DropZoneRowCell(HtmlNode cell_node){
		TargetIdList = new List<string> ();TargetTextList = new List<string> ();
		string type_text = cell_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing DropZoneRowCell node of type "+type_text);
		getCellType (type_text);
		DisplayText = cell_node.InnerText;
		//By default user should not be able to touch the drop zone, if id or answer tage is present then only user can drop anything into the drop zone
		Touchable = false; Dropable = false;
	}
	//Constructor
	public DropZoneRowCell(string type, string displayText){
		TargetIdList = new List<string> ();TargetTextList = new List<string> ();
		DisplayText = displayText;
		getCellType (type);
		//By default user should not be able to touch the drop zone, if id or answer tage is present then only user can drop anything into the drop zone
		Touchable = false; Dropable = false;
	}
	/// <summary>
	/// Checking for "Tag" tag and populating CellTag with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	public void checkForTargetTag(HtmlNode cell_node){
		HtmlAttribute attr_tag = cell_node.Attributes [HTMLParser.ATTR_TAG];
		if (attr_tag != null) {
			CellTag =  (attr_tag.Value);
		}
	}

	/// <summary>
	/// Checking for Id tag and populating TargetIdList with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	public void checkForTargetId(HtmlNode cell_node){
		HtmlAttribute attr_id = cell_node.Attributes [HTMLParser.ATTR_ID];
		if (attr_id != null) {
			generateTargetIdList (attr_id.Value);
		}
	}
	public void generateTargetIdList(string targetId){
		//If id tag is present. user should be able to drop an element.
		Touchable = true;Dropable = true;
		//If id=''. user should be able to drop an element but the element will jump back.
		if (targetId == "") {
			Dropable = false;
		} else {
			TargetIdList = targetId.Split (';').ToList ();
		}
	}

	/// <summary>
	/// Checking for Answer tag and populating TargetTextList with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	public void checkForTargetText(HtmlNode cell_node){
		HtmlAttribute attr_answer = cell_node.Attributes [HTMLParser.ATTR_ANSWER];
		if (attr_answer != null) {
			generateTargetTextList (attr_answer.Value);
		}
	}
	public void generateTargetTextList(string targetText){
		//If answer tag is present. user should be able to drop an element.
		Touchable = true;Dropable = true;
		//If answer=''. user should be able to drop an element but the element will jump back.
		if (targetText == "") {
			Dropable = false;
		} else {
			TargetTextList = targetText.Split (';').ToList ();
		}
	}

}
