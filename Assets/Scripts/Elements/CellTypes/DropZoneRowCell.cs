using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DropZoneRowCell : Cell {
	public CellType DropZoneRowType;

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
	//Constructor
	public DropZoneRowCell(string type, string displayText){
		DisplayText = displayText;
		if(type == "drop_zone_row") DropZoneRowType = CellType.DropZoneRow;
		switch (type) {
		case "drop_zone_row": 
			DropZoneRowType = CellType.DropZoneRow;
			break;
		case "drop_zone": 
			DropZoneRowType = CellType.DropZone;
			break;
		}
		//By default user should not be able to touch the drop zone, if id or answer tage is present then only user can drop anything into the drop zone
		Touchable = false; Dropable = false;
	}
	public void generateTargetTextList(string targetText){
		//If answer tag is present. user should be able to drop an element.
		Touchable = true;Dropable = true;
		//If answer=''. user should be able to drop an element but the element will jump back.
		if (targetText == "") {
			Dropable = false;
		} else {
//			TargetTextList = targetText.Split (';').ToList ();
		}
	}
	public void generateTargetIdList(string targetId){
		//If id tag is present. user should be able to drop an element.
		Touchable = true;Dropable = true;
		//If id=''. user should be able to drop an element but the element will jump back.
		if (targetId == "") {
			Dropable = false;
		} else {
//			TargetIdList = targetId.Split (';').ToList ();
		}
	}

}
