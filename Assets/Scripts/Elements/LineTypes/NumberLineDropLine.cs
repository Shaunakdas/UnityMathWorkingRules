using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineDropLine  : Line {

	//-------------Common Attributes -------------------
	public int LabelCount{ get; set; }
	NumberLineDisplay displayCtrl;

	List<GameObject> NumberMarkerList{ get; set; }
	//Flag to display Number Markers Title
	public bool displayNumber{get; set;}
	//For NumberLineDropJump
	public int DropStartIndex{get; set;}
	public int DropCount{ get; set; }
	public int JumpSize{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	///<summary>
	/// Constructor for NumberLine and NumberLineDrop
	/// </summary>
	public NumberLineDropLine(string labelCount, string type):base(){	
		LabelCount = int.Parse(labelCount);
	}

	///<summary>
	/// Constructor for NumberLineDropLine based on individual vaues
	/// </summary>
	public NumberLineDropLine(string labelCount,string dropStartIndex,string dropCount,string jumpSize, string type):base(){
		NumberMarkerList = new List<GameObject> ();
		LabelCount = int.Parse(labelCount);
		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (dropStartIndex);
			DropCount = int.Parse (dropCount);
			JumpSize = int.Parse (jumpSize);
		}
	}
	/// <summary>
	/// Initializes a new instance of the NumberLineDropLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineDropLine(HtmlNode line_node):base(line_node){
		NumberMarkerList = new List<GameObject> ();displayCtrl = new NumberLineDisplay ();
		string type = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;
		Debug.Log ("Initializing NumberLineDropLine node of type text"+ type);
		LabelCount = int.Parse(line_node.Attributes [AttributeManager.ATTR_LABEL_COUNT].Value);
		getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);

		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (line_node.Attributes [AttributeManager.ATTR_DROP_START_INDEX].Value);
			DropCount = int.Parse (line_node.Attributes [AttributeManager.ATTR_DROP_COUNT].Value);
			JumpSize = int.Parse (line_node.Attributes [AttributeManager.ATTR_JUMP_SIZE].Value);
		}

		prefabName = LocationManager.NAME_NUM_LINE_DROP_LINE;
	}



	//-------------Based on Element Attributes, creating GameObject -------------------
	override public void initGOProp(GameObject elementGO){
		ElementGO = elementGO;
		GameObject numberLineGrid = BasicGOOperation.getChildGameObject (ElementGO, "NumberLineGrid");
		initNumberLineGO (numberLineGrid);
	}
	override public void updateGOProp(GameObject elementGO){
	}
	/// <summary>
	/// Sets dimensions of numberLineGrid based on calculation of NumberLine Class
	/// </summary>
	/// <param name="numberLineGrid">Number line grid.</param>
	public void initNumberLineGO(GameObject numberLineGrid){
		Debug.Log ("adding initNumberLineGameObject ");
		//Initialising Default Values
		displayCtrl.defaultValues ();
		//Changing values based on curent values
		displayCtrl.IntegerCount = LabelCount;
		Debug.Log (displayCtrl.IntegerCount);
		displayCtrl.screenDimensionHeight =this.Parent.ElementGO.GetComponent<UISprite>().height;

		Debug.Log("screendimension height"+displayCtrl.screenDimensionHeight);
		//Calculatig dimensions
		displayCtrl.initNumberLineCalculations();

		numberLineGrid.GetComponent<UIGrid> ().cellHeight = displayCtrl.cellHeight;
		numberLineGrid.GetComponent<UIGrid> ().cellWidth = displayCtrl.cellWidth;
		numberLineGrid.GetComponent<UIGrid> ().Reposition ();
		GameObject smallMarkPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_NUM_LINE_MARK_SMALL_CELL)as GameObject;
		GameObject bigMarkPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_NUM_LINE_MARK_BIG_CELL)as GameObject;

		initNumberLineMarkers (numberLineGrid, smallMarkPF, bigMarkPF);
	}
	/// <summary>
	/// Adds all small and big number markers.
	/// </summary>
	/// <param name="numberLineGrid">Number line grid GameObject</param>
	/// <param name="smallMarkerPF">Small marker Prefab</param>
	/// <param name="numberMarkerPF">Number marker Prefab</param>
	public void initNumberLineMarkers(GameObject numberLineGrid, GameObject smallMarkerPF, GameObject numberMarkerPF){
//		Debug.Log ("Names of gameObjects" + numberMarkerPF.name);
		//First Number Marker
		GameObject firstNumberMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (numberMarkerPF,numberLineGrid.transform,"NumberMarker_"+0);
		NumberMarkerList.Add (firstNumberMarkerGO);
		for (int i = 1; i < displayCtrl.IntegerCount; i++) {
			//First fill small markers
			for (int j = 1; j < displayCtrl.numberBreak; j++) {
				GameObject smallMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (smallMarkerPF,numberLineGrid.transform,"NumberMarker_"+i+"SmallMarker_"+i+"_"+j);
			}
			//Fill Number Marker
			GameObject numberMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (numberMarkerPF,numberLineGrid.transform,"NumberMarker_"+i);
			NumberMarkerList.Add (numberMarkerGO);
		}
		addDefaultItemsToBigMarker ();
	}
	/// <summary>
	/// Adds the default items to every big marker in number line 
	/// </summary>
	public void addDefaultItemsToBigMarker(){
//		GameObject selectBtnPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
		GameObject itemGO=null;int index=0;
		//In order to track target Cell
		Row row = new Row ();

		foreach (GameObject numberMarkerItem in NumberMarkerList){
			if (displayNumber) numberMarkerItem.GetComponentInChildren<TEXDrawNGUI> ().text = ( index).ToString() ;
			//To access DropZoneRowCell methods
			DropZoneRowCell dropCell = new DropZoneRowCell (this.ParagraphRef);
			switch (Type) {
			case LineType.NumberLineDrop:
				List<string> _targetTextList = StringWrapper.splitTargetText (" ");
				itemGO = dropCell.generateDropZoneHolderGO (numberMarkerItem, _targetTextList, false);
				dropCell.ParagraphRef = this.ParagraphRef;
				itemGO.GetComponent<UITable> ().pivot = UIWidget.Pivot.Center;
				itemGO.GetComponentInChildren<UISprite> ().height = itemGO.GetComponentInChildren<UISprite> ().height - 20;
				DropZoneRowCell dropZoneRowCell = new DropZoneRowCell (this.ParagraphRef);
				dropZoneRowCell.addDropZoneHolder (ElementGO, itemGO);
				row.CellList.Add (dropCell);
				//Set target text
				break;
			case LineType.NumberLineSelect:
//				itemGO = BasicGOOperation.InstantiateNGUIGO (selectBtnPF, numberMarkerItem.transform);
				//To access SelectableButtonCell methods
				SelectableButtonCell selectCell = new SelectableButtonCell ();
				itemGO = selectCell.generateSelBtnCellGO(numberMarkerItem,"  ");
				selectCell.updateTableCol (itemGO, 0);
				selectCell.updateTextSize (itemGO, 40);
				selectCell.resizeToFit (itemGO);
				row.CellList.Add (selectCell);
				break;
			case LineType.NumberLineDropJump:
				itemGO = dropCell.generateDropZoneHolderGO (numberMarkerItem,StringWrapper.splitTargetText (""), false);
				row.CellList.Add (dropCell);
				break;
			}
			RowList.Add (row);
			itemGO.transform.localPosition = new Vector3 (-60f, 0f,0f);
			index++;
		}
		 
	}
	/// <summary>
	/// Based on NumberLabelCell values, add items to Each big marker.
	/// </summary>
	/// <returns>GameObject at the required index</returns>
	/// <param name="index">Index.</param>
	/// <param name="text">Text.</param>
	/// <param name="cellLabel">Cell label type</param>
	public GameObject addItemToBigMarker(int index, string text, NumberLineLabelCell.LabelType cellLabel){
		int displayIndex = displayCtrl.IntegerCount - index - 1;
		GameObject itemGO=null;
		if (cellLabel == NumberLineLabelCell.LabelType.Label) {
			itemGO = NumberMarkerList [index];
			switch (Type) {
			case LineType.NumberLineDrop:
				itemGO.GetComponentInChildren<TEXDrawNGUI>().text = text; 
				//Set target text
				break;
			case LineType.NumberLineSelect:
//				GameObject selBtnGO = BasicGOOperation.getChildGameObject (itemGO ,LocationManager.NAME_SELECT_BTN_CELL);
//				SelectableButtonCell.updateText (selBtnGO, text);
				//To access SelectableButtonCell methods
				SelectableButtonCell selectCell = new SelectableButtonCell ();
				GameObject selBtnGO = selectCell.generateSelBtnCellGO(itemGO,text);
				selectCell.updateTextSize (selBtnGO, 40);
				selectCell.resizeToFit (selBtnGO);
				break;
			case LineType.NumberLineDropJump:
				itemGO.GetComponentInChildren<TEXDrawNGUI>().text = text; 
				break;
			}
		} else if (cellLabel == NumberLineLabelCell.LabelType.LabelAnswer) {
			itemGO = NumberMarkerList [displayIndex];
			switch (Type) {
			case LineType.NumberLineDrop:

				itemGO.GetComponentInChildren<DropZoneHolder> ().TargetTextList = StringWrapper.splitTargetText (text); 
				itemGO.GetComponentInChildren<DropZoneHolder> ().ParagraphRef = this.ParagraphRef;
				//Set target text
				break;
			case LineType.NumberLineSelect:
				//				GameObject selBtnGO = BasicGOOperation.getChildGameObject (itemGO ,LocationManager.NAME_SELECT_BTN_CELL);
				//				SelectableButtonCell.updateText (selBtnGO, text);
				SelBtnItemChecker itemChecker = itemGO.GetComponentInChildren<SelBtnItemChecker> ();
				SelectableButtonCell selectBtnCell = new SelectableButtonCell ();
				//To access SelectableButtonCell methods
				SelectableButtonCell selectCell = new SelectableButtonCell ();
				selectCell.updateItemChecker (itemChecker.gameObject, true, this);

				break;
			case LineType.NumberLineDropJump:
				itemGO.GetComponentInChildren<TEXDrawNGUI>().text = text; 
				break;
			}
		}
		return itemGO;
	}
}
