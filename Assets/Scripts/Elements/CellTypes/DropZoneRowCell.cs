﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HtmlAgilityPack;

public class DropZoneRowCell : Cell {

	//-------------Common Attributes -------------------
	public bool idPresent{ get; set; }
	//Dropable
	public bool Dropable{get; set;}
	public bool Touchable{get; set;}

	//Target Text
	string _targetText;
	public string TargetText{ 
		get { return _targetText; }
		set { _targetText = value; TargetTextList = generateTargetTextList (value); }
	}
	public List<string> TargetTextList{ get; set; }

	//Target Id
	string _targetId;
	public string TargetId{ 
		get { return _targetId; }
		set { _targetId = value; generateTargetIdList (value); }
	}
	public List<string> TargetIdList{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------

	/// <summary>
	/// Initializes a new instance of the DropZoneRowCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public DropZoneRowCell(HtmlNode cell_node):base(cell_node){
		TargetIdList = new List<string> ();TargetTextList = new List<string> ();
		DisplayText = cell_node.InnerText;
		HtmlAttribute id_attr = cell_node.Attributes [AttributeManager.ATTR_ID];
		if (id_attr != null) {
			idPresent = true;
			Debug.Log ("Initializing id of DropZoneRowCell"+id_attr.Value);
			TargetId = id_attr.Value; CellId = id_attr.Value;
		}
		HtmlAttribute ans_attr = cell_node.Attributes [AttributeManager.ATTR_ANSWER];
		if (ans_attr != null) {
			TargetText = ans_attr.Value;
		}

		//By default user should not be able to touch the drop zone, if id or answer tage is present then only user can drop anything into the drop zone
		Touchable = false; Dropable = false;
		prefabName = LocationManager.NAME_DROP_ZONE_HOLDER_CELL;
	}
	//Constructor
	public DropZoneRowCell(string type, string displayText):base(type){
		TargetIdList = new List<string> ();TargetTextList = new List<string> ();
		DisplayText = displayText;
		//By default user should not be able to touch the drop zone, if id or answer tage is present then only user can drop anything into the drop zone
		Touchable = false; Dropable = false;
	}
	/// <summary>
	/// Set Cell  Type
	/// </summary>
	new public void getCellType(string type_text){
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
	/// Checking for "Tag" tag and populating CellTag with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	public void checkForTargetTag(HtmlNode cell_node){
		HtmlAttribute attr_tag = cell_node.Attributes [AttributeManager.ATTR_TAG];
		if (attr_tag != null) {
			CellTag =  (attr_tag.Value);
		}
	}

	/// <summary>
	/// Checking for Id tag and populating TargetIdList with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	public void checkForTargetId(HtmlNode cell_node){
		HtmlAttribute attr_id = cell_node.Attributes [AttributeManager.ATTR_ID];
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
		HtmlAttribute attr_answer = cell_node.Attributes [AttributeManager.ATTR_ANSWER];
		if (attr_answer != null) {
			generateTargetTextList (attr_answer.Value);
		}
	}
	public List<string> generateTargetTextList(string targetText){
		Debug.Log ("generateTargetTextList");
		List<string> _targetTextList = new List<string> ();
		//If answer tag is present. user should be able to drop an element.
		Touchable = true;Dropable = true;
		//If answer=''. user should be able to drop an element but the element will jump back.
		if (targetText == "") {
			Dropable = false;
		} else {
			_targetTextList = StringWrapper.splitTargetText(targetText);
		}
		return _targetTextList;
	}

	//-------------Based on Element Attributes, creating GameObject -------------------

	override public GameObject generateElementGO(GameObject parentGO){
		GameObject _elementGO =   generateDropZoneHolderGO (parentGO, TargetTextList, idPresent);
		ElementGO = _elementGO;
		//_elementGO add to list of aniamtion
		return _elementGO;
	}
	static public GameObject generateDropZoneHolderGO(GameObject parentGO, List<string> _targetTextList, bool idPresent){
		GameObject dropZoneHolderPrefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_HOLDER_CELL)as GameObject;
		GameObject dropZoneTableprefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_TABLE_CELL)as GameObject;

		GameObject holderGO = BasicGOOperation.InstantiateNGUIGO (dropZoneHolderPrefab, parentGO.transform);

		DropZoneHolder dropZoneHolder = holderGO.GetComponent<DropZoneHolder> ();
		dropZoneHolder.TargetTextList = _targetTextList;
		dropZoneHolder.idCheck = idPresent;
		dropZoneHolder.addToTargetList ();

		Debug.Log ("TargetText list"+_targetTextList.Count+idPresent);
		foreach (string targetText in _targetTextList){
			Debug.Log ("TargetText"+targetText.ToString());
			GameObject backgroundGO = BasicGOOperation.InstantiateNGUIGO (dropZoneTableprefab, holderGO.transform);
			Debug.Log ("backgroundGO "+backgroundGO.name.ToString());

			int tableWidth = 0;
			GameObject tableGO = BasicGOOperation.getChildGameObject (backgroundGO, "Table");
			if (idPresent) {
				//id attribute is present. So there is no need to divide drop zone into individual item
				GameObject tableItemGO =  initDropZoneTableItem(tableGO,idPresent);
//				tableItemGO.GetComponent<DropZoneItemChecker>().element = this;
				int contentWidth = targetText.ToCharArray ().Length * 30;
				tableItemGO.GetComponent<UISprite> ().width = contentWidth;
				tableWidth = contentWidth+10;
			} else {
				foreach (char targetChar in targetText.ToCharArray().ToList()) {
					tableWidth += 50 + 5;
					if (targetChar == '+' || targetChar == '-') {
						GameObject signBtnGO = SelectableButtonCell.generateSelBtnCellGO (tableGO, "-");
					} else {
						GameObject tableItemGO = initDropZoneTableItem (tableGO, idPresent);
//						tableItemGO.GetComponent<DropZoneItemChecker>().element = this;
					}
				}
			}
//			BasicGOOperation.ResizeToFitChildGO (backgroundGO);
			backgroundGO.GetComponent<UISprite>().width = tableWidth;
			//			updateGOProp (backgroundGO);
			BasicGOOperation.CheckAndRepositionTable (tableGO);
		}
		BasicGOOperation.CheckAndRepositionTable (holderGO);
		return holderGO;
	}
	override public void updateGOProp(GameObject _elementGO){
		Debug.Log ("Updating Properties of Drop Zone Cell");
		float cellWidth = _elementGO.GetComponent<UISprite> ().localSize.x;
		if (TargetText != null) { 
			Debug.Log ("Updating Target Text of Drop Zone Cell" + TargetText + Mathf.Max (70f, BasicGOOperation.getNGUITextSize (TargetText) + 40f).ToString());
			_elementGO.GetComponent<UISprite>().width =  (int)Mathf.Max (70f, BasicGOOperation.getNGUITextSize (TargetText)	);
		}
	}


	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	static public GameObject initDropZoneTableItem(GameObject parentGO, bool idPresent){
		GameObject dropZoneTableItemprefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_TABLE_ITEM_CELL)as GameObject;
		GameObject tableItemGO = BasicGOOperation.InstantiateNGUIGO (dropZoneTableItemprefab, parentGO.transform);
		tableItemGO.GetComponent<DropZoneItemChecker> ().idCheck = (idPresent!=null)?idPresent:false;
		tableItemGO.GetComponent<DropZoneItemChecker> ().DropZoneHolderGO = parentGO.transform.parent.parent.gameObject;
		return tableItemGO;
	}
	static public void addDropZoneHolder(GameObject parentGO,GameObject dropZoneHolderGO){
		dropZoneHolderGO.GetComponent<DropZoneHolder> ().holderListParentGO = parentGO;
		parentGO.AddComponent<DropZoneHolderParent> ();
		parentGO.GetComponent<DropZoneHolderParent> ().addDropZoneHolder (dropZoneHolderGO);
		parentGO.GetComponent<DropZoneHolderParent> ().addToTargetList ();
	}

	//-------------Animations-------------------

	/// <summary>
	/// Correct animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	public void correctAnim(GameObject _elementGO){
		Debug.Log ("DropZoneRowCell CorrectAnim");
//		Paragraph.nextTargetTrigger (this);
	}
	/// <summary>
	/// Incorrect animation.
	/// </summary>
	/// <param name="_elementGO">Element G.</param>
	public void incorrectAnim(GameObject _elementGO){
		Debug.Log ("DropZoneRowCell incorrectAnim");
//		Paragraph.nextTargetTrigger (this);
	}
}
