using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class SelectableButtonCell : Cell {

	//-------------Common Attributes -------------------
	public bool correctFlag;


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public SelectableButtonCell():base(){
	}
	public SelectableButtonCell(string type, string answer, string displayText):base(type){
		correctFlag = answer=="1"? true:false;
		DisplayText = displayText;
	}
	public SelectableButtonCell(SelItem item):base(){
		correctFlag = item.correctFlag;
		prefabName = LocationManager.NAME_SELECT_BTN_CELL;
		DisplayText = item.DisplayText;
	}
	/// Initializes a new instance of the SelectableButtonCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public SelectableButtonCell(HtmlNode cell_node):base(cell_node){
		//		CellList = new List<Cell> ();
		correctFlag = (cell_node.Attributes [AttributeManager.ATTR_ANSWER].Value)=="1"? true:false;
		prefabName = LocationManager.NAME_SELECT_BTN_CELL;
		DisplayText = cell_node.InnerText;
	}

	//Analytics and setting ParagraphRef
	override public void  setChildParagraphRef(){
		if(correctFlag){
			ParagraphRef.scoreSettings.maxCorrectCount += 1;
		}
	}
	//-------------Based on Element Attributes, creating GameObject -------------------
	override public GameObject generateElementGO(GameObject parentGO){
		getAlignType ();
		ElementGO =  generateSelBtnCellGO (parentGO, DisplayText);
		Debug.Log (Parent.Parent.GetType ().ToString ());
		SelBtnItemChecker itemChecker = updateItemChecker (ElementGO, correctFlag, Parent.Parent as Line);
		Debug.Log(itemChecker.correctFlag);
		return ElementGO;
	}
	 public GameObject generateSelBtnCellGO(GameObject parentGO, string text){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		updateSelectBtnGO(cellGO, text);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}

	override public void updateGOProp(GameObject ElementGO){
		//		Debug.Log ("Updating Text of Cell" + DisplayText);
		updateSelectBtnGO(ElementGO, DisplayText);
		//Init Select Item Checker
		SelBtnItemChecker itemChecker = updateItemChecker (ElementGO, correctFlag, Parent.Parent as Line);
	}



	//-------------Static methods to create/update Selectable GameObject attributes -------------------
	//Static method which can be used by any class initiating SelectButton
	public  void updateSelectBtnGO(GameObject ElementGO, string text){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		updateText (ElementGO, text);
		//Setting width based on text width
		resizeToFit(ElementGO);
		GameObject checkBoxGO = BasicGOOperation.getChildGameObject (TableGO, "CheckBox");
		SelBtnItemChecker itemChecker = ElementGO.GetComponent<SelBtnItemChecker> ();
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { updateCheckBox(checkBoxGO);itemChecker.changeInputFlag(); });
	}
	public  void updateCheckBox(GameObject checkBoxGO){
//		checkBoxGO.GetComponent<UIToggle> ().value = !checkBoxGO.GetComponent<UIToggle> ().value;
		checkBoxGO.GetComponent<UIToggle> ().Set(!checkBoxGO.GetComponent<UIToggle> ().value,true);
	}
	public  void updateTableCol(GameObject ElementGO, int col){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		TableGO.GetComponent<UITable> ().columns = col;
	}
	public  void updateText(GameObject ElementGO, string text){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject labelGO = BasicGOOperation.getChildGameObject (TableGO, "Label");
		//Setting text
		labelGO.GetComponent<UILabel> ().text = text;
	}
	public  void updateTextSize(GameObject ElementGO, int size){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		GameObject labelGO = BasicGOOperation.getChildGameObject (TableGO, "Label");
		//Setting text
		labelGO.GetComponent<UILabel> ().fontSize = size;
	}
	public  void resizeToFit(GameObject ElementGO){
		GameObject TableGO = BasicGOOperation.getChildGameObject (ElementGO, "Table");
		Debug.Log ("Name of ElemtnGO" + ElementGO.name + "NAme of targetGO" + TableGO.GetComponentInChildren<UILabel>().text);
		TableGO.GetComponent<UITable> ().Reposition ();
		BasicGOOperation.ResizeToFitTargetGO (ElementGO, TableGO);
		ElementGO.GetComponent<UIWidget> ().width = (int)(ElementGO.GetComponent<UIWidget> ().width + 25f);
		ElementGO.GetComponent<UIWidget> ().height = (int)(ElementGO.GetComponent<UIWidget> ().height + 10f);
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	public SelBtnItemChecker updateItemChecker(GameObject _elementGO, bool _correctBool, Line line){
		SelBtnItemChecker itemChecker= _elementGO.GetComponent<SelBtnItemChecker> ();
		//		Debug.Log (_correctBool);
		itemChecker.correctFlag = _correctBool;itemChecker.SelBtnHolderGO = line.ElementGO;
		//Adding SelBtrnGolder script to parent TableLine
		if (line.ElementGO.GetComponent<SelBtnHolder>() == null)
			line.ElementGO.AddComponent<SelBtnHolder>();
		line.ElementGO.GetComponent<SelBtnHolder>().addSelectBtn (_elementGO, _correctBool);
		line.ElementGO.GetComponent<SelBtnHolder>().addToTargetList ();
		line.ElementGO.GetComponent<SelBtnHolder> ().ParagraphRef = this.ParagraphRef;

		return itemChecker;
	}

}
