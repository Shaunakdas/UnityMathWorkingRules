using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class RangeTableLine  : TableLine {

	//-------------Common Attributes -------------------
	public int rangeStart = 0;
	public int rangeEnd = 1;
	public int rangeCount = 1;

	public enum Sort{Increasing,Decreasing,Random}
	public Sort sortOrder = Sort.Increasing;

	public enum ItemType{Prime,Multiple,Factor}
	public ItemType correctItemType = ItemType.Prime;

	public List<int> correctTargetList;

	public bool eliminate = false;

	public int correctionCount = 1;

	public List<SelBtnItemChecker> SelItemList;
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public RangeTableLine(){
	}
	//Constructor
	public RangeTableLine(string type):base(type){
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public RangeTableLine(HtmlNode line_node):base(line_node){
		rangeStart = StringWrapper.HtmlAttrValueInt (line_node, AttributeManager.ATTR_START);
		rangeEnd = StringWrapper.HtmlAttrValueInt (line_node, AttributeManager.ATTR_END);
		rangeCount = StringWrapper.HtmlAttrValueInt (line_node, AttributeManager.ATTR_COUNT);
		rangeCount = (rangeCount == null)? (rangeEnd - rangeStart): rangeCount;

		if (line_node.Attributes [AttributeManager.ATTR_SORT_ORDER] != null) {
			sortOrder = (Sort)System.Enum.Parse (typeof(Sort),StringWrapper.ConvertToPascalCase(line_node.Attributes [AttributeManager.ATTR_SORT_ORDER].Value),true);
		}
		if (line_node.Attributes [AttributeManager.ATTR_CORRECT_ITEM_TYPE] != null) {
			correctItemType = (ItemType)System.Enum.Parse (typeof(ItemType),StringWrapper.ConvertToPascalCase(line_node.Attributes [AttributeManager.ATTR_CORRECT_ITEM_TYPE].Value),true);
		}

		correctTargetList=StringWrapper.splitTargetTextToInt(StringWrapper.HtmlAttrValue (line_node, AttributeManager.ATTR_CORRECT_TARGET));
		eliminate =  StringWrapper.HtmlAttrValueBool (line_node, AttributeManager.ATTR_ELIMINATE);
		correctionCount =  StringWrapper.HtmlAttrValueInt (line_node, AttributeManager.ATTR_CORRECTION_COUNT);
		generateSelItemList ();
	}
	override public void parseChildNode(HtmlNode line_node){
		base.parseChildNode(line_node);
		//Adding a row
		Row row = new Row ();
		row.Parent = this;
		//Adding cells to row
		row.CellList = generateSelBtnList(SelItemList,row);
		RowList.Add(row);
	}
	public void generateSelItemList(){
		//Range coverage
		SelItemList = generateRangeContent(rangeStart,rangeEnd,rangeCount,sortOrder);
		SelItemList = setCorrectFlag (SelItemList, correctItemType, correctTargetList);
	}
	public List<SelBtnItemChecker> generateRangeContent(int _rangeStart, int _rangeEnd, int _rangeCount, Sort _sort){
		List<SelBtnItemChecker> _selItemList = new List<SelBtnItemChecker> ();
		if (_sort == Sort.Random) {
			List<int> RandomNum = StringWrapper.generateRandInRange (_rangeStart, _rangeEnd, _rangeCount);
			for (int i = 0; i < _rangeCount; i++) {
				SelBtnItemChecker selItem = null;
				selItem.DisplayText = RandomNum [i].ToString ();
				_selItemList.Add (selItem);
			}
		} else {
			for (int i = 0; i < _rangeCount; i++) {
				SelBtnItemChecker selItem = null;
				float unitDiff = (_rangeEnd - _rangeStart) / _rangeCount;
				switch (_sort) {
				case Sort.Increasing:
					selItem.DisplayText = ((int)_rangeStart + (unitDiff * i)).ToString ();
					break;
				case Sort.Decreasing:
					selItem.DisplayText = ((int)_rangeEnd - (unitDiff * i)).ToString ();
					break;
				}
				_selItemList.Add (selItem);
			}
		}
		switch (_sort) {
		case Sort.Increasing:
			_selItemList [0].DisplayText = _rangeStart.ToString (); _selItemList [SelItemList.Count - 1].DisplayText = _rangeEnd.ToString ();
			break;
		case Sort.Decreasing:
			_selItemList [0].DisplayText = _rangeEnd.ToString (); _selItemList [SelItemList.Count - 1].DisplayText = _rangeStart.ToString ();
			break;
		}
		return _selItemList;
	}

	public List<SelBtnItemChecker> setCorrectFlag(List<SelBtnItemChecker> _selItemList, ItemType _itemType, List<int> _targetList){
		for (int i = 0; i < _selItemList.Count; i++) {
			bool correctFlag = false; int content = int.Parse (_selItemList[i].DisplayText);int _targetCount = _targetList.Count;
			switch (_itemType) {
			case ItemType.Multiple:
				int lcm = (_targetCount == 1)?_targetList[0]:PrimeDivision.lcm (_targetList[0],_targetList[1]);
				correctFlag = ((content % lcm) == 0) ? true : false;
				break;
			case ItemType.Prime:
				correctFlag = PrimeDivision.isPrime (content);
				break;
			case ItemType.Factor:
				int gcd = (_targetCount == 1)?_targetList[0]:PrimeDivision.gcd (_targetList[0],_targetList[1]);
				correctFlag = ((content % gcd) == 0) ? true : false;
				break;
			}
			_selItemList [i].correctFlag = correctFlag;_selItemList [i].HighlightOnCorrectSelection = !eliminate;
		}
		return _selItemList;
	}
	public List<Cell> generateSelBtnList(List<SelBtnItemChecker> _selItemList, Row row){
		List<Cell> selBtnList = new List<Cell>();
		for (int i = 0; i < _selItemList.Count; i++) {
			SelectableButtonCell selBtnCell = new SelectableButtonCell (_selItemList[i]);
			selBtnCell.Parent = row;
			selBtnList.Add (selBtnCell);
		}
		return selBtnList;
	}
	//-------------Based on Element Attributes, creating GameObject -------------------
	override public void updateGOProp(GameObject _elementGO){
		base.updateGOProp(_elementGO);
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//For Selectable Button Holder
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		base.updateSelBtnHolder (_selBtnGO, _selBtnBool);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
