using System.Collections;
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
	public DropZoneRowCell():base(){

	}
	public DropZoneRowCell(Paragraph paraRef):base(){
		ParagraphRef = paraRef;
	}
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
	/// Checking for "Tag" tag and populating CellTag with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	void checkForTargetTag(HtmlNode cell_node){
		HtmlAttribute attr_tag = cell_node.Attributes [AttributeManager.ATTR_TAG];
		if (attr_tag != null) {
			CellTag =  (attr_tag.Value);
		}
	}

	/// <summary>
	/// Checking for Id tag and populating TargetIdList with its value if present
	/// </summary>
	/// <param name="cell_node">Cell node.</param>
	void checkForTargetId(HtmlNode cell_node){
		HtmlAttribute attr_id = cell_node.Attributes [AttributeManager.ATTR_ID];
		if (attr_id != null) {
			generateTargetIdList (attr_id.Value);
		}
	}
	void generateTargetIdList(string targetId){
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
	void checkForTargetText(HtmlNode cell_node){
		HtmlAttribute attr_answer = cell_node.Attributes [AttributeManager.ATTR_ANSWER];
		if (attr_answer != null) {
			generateTargetTextList (attr_answer.Value);
		}
	}
	List<string> generateTargetTextList(string targetText){
//		Debug.Log ("generateTargetTextList");
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
	//Analytics and setting ParagraphRef
	override public void  setChildParagraphRef(){
		foreach (string targetText in TargetTextList) {
			if (idPresent) {
				ParagraphRef.scoreTracker.childCorrectCount += 1;
			} else {
				ParagraphRef.scoreTracker.childCorrectCount += targetText.ToCharArray ().Count();
			}
		}
	}
	//-------------Based on Element Attributes, creating GameObject -------------------

	override public GameObject generateElementGO(GameObject parentGO){
		GameObject _elementGO =   generateDropZoneHolderGO (parentGO, TargetTextList, idPresent);
		ElementGO = _elementGO;
		//_elementGO add to list of aniamtion
		return _elementGO;
	}

	public GameObject generateDropZoneHolderGO(GameObject parentGO, List<string> _targetTextList, bool idPresent){
		TargetTextList = _targetTextList;
		DropZoneHolder dropZoneholder = initDropZoneHolder (parentGO, _targetTextList, idPresent);
		generateDropZoneQuestionList (dropZoneholder,_targetTextList);
		BasicGOOperation.CheckAndRepositionTable (dropZoneholder.gameObject);
		return dropZoneholder.gameObject;
	}

	//Generating List of DropZone Items
	void generateDropZoneQuestionList(DropZoneHolder _dropZoneholder,List<string> _targetTextList){
		foreach (string targetText in _targetTextList){
			//Initing List of DropZoneItemChecker to then add to ItemCheckerMasterList of DropZoneHolder
			List<DropZoneOptionChecker> itemCheckerList =  new List<DropZoneOptionChecker>();

			DropZoneQuestionChecker dropZoneQuestion = initDropZoneQuestion(_dropZoneholder);
			int tableWidth = 0;
			if (idPresent) {
				tableWidth = dropZoneIdOptionLength(targetText,dropZoneQuestion,itemCheckerList);
			} else {
				tableWidth = dropZoneValueOptionLength(targetText,dropZoneQuestion,itemCheckerList);
			}
			dropZoneQuestion.gameObject.GetComponent<UISprite>().width = tableWidth;

			//Adding itemCheckerList to ItemCheckerMasterList of DropZoneHolderGO
			_dropZoneholder.ItemCheckerMasterList.Add(itemCheckerList); 

			GameObject tableGO = BasicGOOperation.getChildGameObject (dropZoneQuestion.gameObject, "Table");
			BasicGOOperation.CheckAndRepositionTable (tableGO);
		}
	}

	//Generating DropZone Items referenced using id
	int dropZoneIdOptionLength(string _targetText,DropZoneQuestionChecker _question,List<DropZoneOptionChecker> _questionList){
		//id attribute is present. So there is no need to divide drop zone into individual item
		//Generating DropZone Option
		DropZoneOptionChecker option =  initDropZoneOption(_question,true);
		_questionList.Add (option);
		//Calculating Length of DropZone Option
		int contentWidth = _targetText.ToCharArray ().Length * 30;
		option.gameObject.GetComponent<UISprite> ().width = contentWidth;
		return contentWidth+10;
	}

	//Generating DropZone Items referenced using value
	int dropZoneValueOptionLength(string _targetText,DropZoneQuestionChecker _question,List<DropZoneOptionChecker> _questionList){
		int tableWidth = 0;
		//id attribute is not present. So we have to divide drop zone into individual item
		foreach (char targetChar in _targetText.ToCharArray().ToList()) {
			//Calculating Length of DropZoneItem
			tableWidth += 50 + 5;
			if (targetChar == '+' || targetChar == '-') {
				SelectableSignCell selectCell = new SelectableSignCell (targetChar.ToString());
				GameObject tableGO = BasicGOOperation.getChildGameObject (_question.gameObject, "Table");
				GameObject signBtnGO = selectCell.generateSelBtnCellGO (tableGO, "-");
				selectCell.updateItemChecker (signBtnGO, _question);
			} else {
				//Generating DropZone Option
				DropZoneOptionChecker option = initDropZoneOption (_question, false);
				_questionList.Add (option);
			}
		}
		return tableWidth;
	}
	override protected void updateGOProp(GameObject _elementGO){
		Debug.Log ("Updating Properties of Drop Zone Cell");
//		float cellWidth = _elementGO.GetComponent<UISprite> ().localSize.x;
		if (TargetText != null) { 
			Debug.Log ("Updating Target Text of Drop Zone Cell" + TargetText + Mathf.Max (70f, BasicGOOperation.getNGUITextSize (TargetText) + 40f).ToString());
			_elementGO.GetComponent<UISprite>().width =  (int)Mathf.Max (70f, BasicGOOperation.getNGUITextSize (TargetText)	);
		}
	}


	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//Creating Drop Zone Holder
	DropZoneHolder initDropZoneHolder(GameObject parentGO, List<string> _targetTextList,bool _idPresent){
		GameObject dropZoneHolderPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_HOLDER_CELL)as GameObject;
		GameObject holderGO = BasicGOOperation.InstantiateNGUIGO (dropZoneHolderPF, parentGO.transform);

		DropZoneHolder holder = holderGO.GetComponent<DropZoneHolder> ();
		//Reference Variables
		holder.ContainerElem = this;
		//DropZone specific Variables
		holder.TargetTextList = _targetTextList;
		holder.idCheck = _idPresent;
		return holder;
	}
	//Creating Drop Zone Questions
	DropZoneQuestionChecker initDropZoneQuestion(DropZoneHolder holder){
		GameObject dropZoneTablePF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_TABLE_CELL)as GameObject;
		GameObject questionGO = BasicGOOperation.InstantiateNGUIGO (dropZoneTablePF, holder.gameObject.transform);

		DropZoneQuestionChecker question = questionGO.GetComponent<DropZoneQuestionChecker> ();
		//Reference Variables
		question.addParentChecker(holder);
		question.ContainerElem = this;
		question.addToMasterLine ();
		return question;
	}

	//Creating Drop Zone Options
	DropZoneOptionChecker initDropZoneOption(DropZoneQuestionChecker question, bool idPresent){
		GameObject dropZoneOptionPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_OPTION_CELL)as GameObject;
		GameObject tableGO = BasicGOOperation.getChildGameObject (question.gameObject, "Table");
		GameObject optionGO = BasicGOOperation.InstantiateNGUIGO (dropZoneOptionPF, tableGO.transform);

		DropZoneOptionChecker option = optionGO.GetComponent<DropZoneOptionChecker> ();
		//Reference Variables
		option.addParentChecker( question);
		option.ContainerElem = this;
		//Dropzone Specific Variables
		option.idCheck = (idPresent!=null)?idPresent:false;
		return option;
	}
	public void addDropZoneHolder(GameObject parentGO,GameObject dropZoneHolderGO){
		dropZoneHolderGO.GetComponent<DropZoneHolder> ().holderListParentGO = parentGO;

		Debug.Log ("PARAGRAPH_REF 1"+ParagraphRef.ElementGO.name);
		dropZoneHolderGO.GetComponent<DropZoneHolder> ().ContainerElem = this;
		if (parentGO.GetComponent<DropZoneHolderParent>() == null)
			parentGO.AddComponent<DropZoneHolderParent> ();
		parentGO.GetComponent<DropZoneHolderParent> ().ContainerElem = this;
		parentGO.GetComponent<DropZoneHolderParent> ().addDropZoneHolder (dropZoneHolderGO);
		parentGO.GetComponent<DropZoneHolderParent> ().addToMasterLine ();
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
