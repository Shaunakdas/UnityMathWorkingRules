	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class Paragraph : BaseElement{
	//-------------Common Attributes -------------------
	//Is the paragraph a comprehension question or question step
	public enum StepType {Comprehension,QuestionStep};
	public StepType ParagraphStep;

	//What is the alignment of Paragraph: Vertically or Horizontally
	public enum AlignType {Vertical,Horizontal};
	static public AlignType ParagraphAlign;


	//How many correct options are present in current questionstep
	public enum CorrectType {SingleCorrect,MultipleCorrect}
	public CorrectType ParagraphCorrect;


	//Number of columns of Table Component
	public int tableCol;

	//List of child Line elements
	public List<Line> LineList{get; set;}


	//-----------------Animation Attributes --------------
	//List of questions
//	static public List<QuestionChecker> QuestionList{get; set;}

	//Text to be displayed after completion of paragraph
	public string postSubmitText{get; set;}

	public List<TableLine> DragSourceTableList = new List<TableLine>();




	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Empty Contructor
	public Paragraph(){
	}

	/// <summary>
	/// Initializes a new instance of the Paragraph class with only correctType attribute
	/// </summary>
	/// <param name="correctType">Correct type.</param>
	public Paragraph(string correctType){
		ParagraphRef = this;
		ParagraphCorrect = (CorrectType)System.Enum.Parse (typeof(CorrectType),StringWrapper.ConvertToPascalCase(correctType),true);
	}
	/// <summary>
	/// Initializes a new instance of the Paragraph class with HTMLNode attribute
	/// </summary>
	/// <param name="para"></param>
	public Paragraph(HtmlNode para_node){
		ParagraphRef = this;htmlNode = para_node;
		LineList = new List<Line> ();
//		Debug.Log ("Initializing paragraph node of type "+para_node.Attributes [AttributeManager.ATTR_TYPE].Value);

		HtmlAttribute align = para_node.Attributes [AttributeManager.ATTR_ALIGN];
		HtmlAttribute correctCount = para_node.Attributes [AttributeManager.ATTR_CORRECT_TYPE];
		switch (para_node.Attributes [AttributeManager.ATTR_TYPE].Value) {
		case "comprehension": 
			ParagraphStep = StepType.Comprehension;
			prefabName = LocationManager.NAME_COMPREHENSION_PARA;
			ParagraphAlign = (AlignType)System.Enum.Parse (typeof(AlignType),StringWrapper.ConvertToPascalCase(align.Value),true);
			tableCol = (ParagraphAlign == AlignType.Vertical)? 1:0;
			break;
		case "question_step": 
			ParagraphStep = StepType.QuestionStep;
			prefabName = LocationManager.NAME_QUESTION_STEP_PARA;
			ParagraphCorrect = (CorrectType)System.Enum.Parse (typeof(CorrectType),StringWrapper.ConvertToPascalCase(correctCount.Value),true);
			ParagraphAlign = (AlignType)System.Enum.Parse (typeof(AlignType),StringWrapper.ConvertToPascalCase(align.Value),true);
			tableCol = (ParagraphAlign == AlignType.Vertical)? 1:0;
			break;
		}
		setAnalyticsIdFromAttr (para_node);
		//Traversing through Child Element - 1
		parseChildNode (para_node);
		//Traversing through Child Element - 2
		setChildParagraphRef ();
	}
	/// <summary>
	/// Parses the Paragraph Node to generate Line nodes
	/// </summary>
	override public void parseChildNode(HtmlNode para_node){
//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = para_node.Elements(AttributeManager.TAG_LINE) ;

		if (node_list != null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode line_node in node_list) {

				string line_type = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;
				if (line_type == "post_submit_text") {
					postSubmitText = StringWrapper.HtmlToPlainText((line_node).InnerText);
				} else {
					Line newLine = new Line (line_node);
//					Debug.Log ("Line type " + line_type);
					switch (line_type) {
					case "text": 
						newLine = new TextLine (line_node);
						break;
					case "incorrect_submit_text": 
						newLine = new TextLine (line_node);
						break;
					case "table": 
						newLine = new TableLine (line_node);
						break;
					case "tex_table": 
						newLine = new TexTableLine (line_node);
						break;
					case "range_table": 
						newLine = new RangeTableLine (line_node);
						break;
					case "prime_division": 
						newLine = new PrimeDivisionLine (line_node);
						break;
					case "number_line_drop": 
						newLine = new NumberLineDropLine (line_node);
						break;
					case "number_line_select": 
						newLine = new NumberLineDropLine (line_node);
						break;
					case "combination": 
						newLine = new CombinationLine (line_node);
						break;
					}
					//Populate Child Row nodes inside Line Node
//				newLine.parseChildNode (line_node);
					newLine.Type = (Line.LineType)System.Enum.Parse (typeof(Line.LineType),StringWrapper.ConvertToPascalCase(line_type),true);
					newLine.Parent = this;
					//Add Line node into LineList
					LineList.Add (newLine);
				}
			}
		}

	}
	override public void  setChildParagraphRef(){
		foreach (Line line in LineList) {
			line.ParagraphRef = this.ParagraphRef;
			line.setChildParagraphRef ();
		}
	}
	/// <summary>
	/// (Based on matching id) Populates TargetText of DropZone Row Element with Reference of corresponding DragSourceCell 
	/// </summary>
	public void populateCellTargetText(){
//		Debug.Log ("Populating Target Text of each drop zone cell");
		//Tracking all DropZone Cell TargetText
		List<DropZoneRowCell> dropZoneCellList = new List<DropZoneRowCell> ();
		List<DragSourceCell> dragSourceCellList = new List<DragSourceCell> ();
		//Parse through whole line list and its contents to get DragZoneRow Cell with valid id
		foreach (Line line in LineList) {
			dropZoneCellList = getDropZoneCellList(dropZoneCellList,line);
//			foreach (Row row in line.RowList) {
//				foreach (var cell in row.CellList) {
//					//					Debug.Log ("Traversing through cell List current type" + cell.GetType ().ToString () + cell.CellId);
//					if ((cell.GetType () == typeof(DropZoneRowCell))&& (cell.CellId != null)) {
//						//						Debug.Log ("Found one Drop zone Cell");
//						DropZoneRowCell dropCell = (DropZoneRowCell)cell;
//						Debug.Log ("Found one Drop zone Cell" + dropCell.CellId);
//						dropZoneCellList.Add (dropCell);
//					}
//				}
//			}
		}
		dragSourceCellList = getDragSourceCellList (dragSourceCellList, this);
//		foreach (Line line in LineList) {
//			foreach (Row row in line.RowList) {
//				foreach (var cell in row.CellList) {
//					if ((cell.GetType () == typeof(DragSourceCell)) && (cell.CellId != null)) {
//						DragSourceCell dragCell = (DragSourceCell)cell;
//						Debug.Log ("Found one Drag source Cell"+ dragCell.CellId);
//						dragSourceCellList.Add (dragCell);
//					}
//				}
//			}
//		}
//		foreach (DragSourceCell dragCell in dragSourceCellList) {
//			DropZoneRowCell dropZone = dropZoneCellList.Find (x => x.CellId == dragCell.CellId);
//			Debug.Log ("Changing Target Text of id" + dragCell.DisplayText +dropZone.CellId);
//			dropZone.TargetText = dragCell.DisplayText;
//		}
		foreach (DropZoneRowCell dropZone in dropZoneCellList) {
			DragSourceCell dragCell = dragSourceCellList.Find (x => x.CellId == dropZone.CellId);
			Debug.Log ("Changing Target Text of id" + dragCell.DisplayText +dropZone.CellId);
			dropZone.TargetText = dragCell.DisplayText;
		}
	}

	protected List<DropZoneRowCell> getDropZoneCellList(List<DropZoneRowCell> _dropZoneCellList, Line rowParent){
		foreach (Row row in rowParent.RowList) {
			foreach (var cell in row.CellList) {
				//					Debug.Log ("Traversing through cell List current type" + cell.GetType ().ToString () + cell.CellId);
				if ((cell.GetType () == typeof(DropZoneRowCell))&& (cell.CellId != null)) {
					//						Debug.Log ("Found one Drop zone Cell");
					DropZoneRowCell dropCell = (DropZoneRowCell)cell;
					Debug.Log ("Found one Drop zone Cell" + dropCell.CellId);
					_dropZoneCellList.Add (dropCell);
				}
				if (cell.GetType () == typeof(FractionCell)) {
					_dropZoneCellList = getDropZoneCellList (_dropZoneCellList, cell);
				}
			}
		}
		return _dropZoneCellList;
	}

	protected List<DragSourceCell> getDragSourceCellList(List<DragSourceCell> _dragSourceCellList, Paragraph _para){
		foreach (Line line in _para.LineList) {
			foreach (Row row in line.RowList) {
				foreach (var cell in row.CellList) {
					if ((cell.GetType () == typeof(DragSourceCell)) && (cell.CellId != null)) {
						DragSourceCell dragCell = (DragSourceCell)cell;
						Debug.Log ("Found one Drag source Cell"+ dragCell.CellId);
						_dragSourceCellList.Add (dragCell);
					}
				}
			}
		}
		return _dragSourceCellList;
	}
	//-------------Based on Element Attributes, creating GameObject -------------------


	/// <summary>
	/// Generates the corresponding GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	override public GameObject generateElementGO(GameObject parentGO){

//		QuestionList = new List<QuestionChecker> ();
		//Setting targetText of child drop zone cell;
		populateCellTargetText ();
		GameObject QuestionStepParaPF = Resources.Load (LocationManager.COMPLETE_LOC_PARAGRAPH_TYPE + prefabName)as GameObject;
		GameObject ParaContentTableGO = null;
		GameObject CenterContentScrollViewPF = Resources.Load (LocationManager.COMPLETE_LOC_OTHER_TYPE + LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW)as GameObject;
		if (ParagraphStep == Paragraph.StepType.QuestionStep) {
//		if(true){
			//Paragraph is of type QuestionStep

			//Adding QuestionStepParaPF to the root GameObject
			GameObject QuestionStepParaGO = BasicGOOperation.InstantiateNGUIGO (QuestionStepParaPF, parentGO.transform);
			ElementGO = QuestionStepParaGO;
			initGOProp (ElementGO);
			//Setting height of Paragraph ElementGO
			ParaContentTableGO = BasicGOOperation.getChildGameObject (QuestionStepParaGO, "ParaContentTable");
			ParaContentTableGO.GetComponent<UITable> ().columns = tableCol;
			//Checking for Center Content Scroll View
			bool isCenterContentPresent = false;
			GameObject CenterContentGO = parentGO;
//			addStartWorkingRuleBtn (ParaContentTableGO); 
			foreach (Line line in LineList)
				if (line.LineLocation == Line.LocationType.Default) {
					CenterContentGO = BasicGOOperation.InstantiateNGUIGO (CenterContentScrollViewPF, ParaContentTableGO.transform);
					GameObject LineTableGO = BasicGOOperation.getChildGameObject (CenterContentGO, "LineTablePF");
					isCenterContentPresent = true;
					break;
				}

			foreach (Line line in LineList) {
				line.generateElementGO (ParaContentTableGO);
			}
			if (isCenterContentPresent)
				resizeCenterContent (CenterContentGO, ParaContentTableGO);
//			Debug.Log ("Post Line List Element Generation CheckAndRepositionTable"+);
			updateGOProp (ElementGO);
			BasicGOOperation.RepositionChildTables (ParaContentTableGO);
//			setUpChildActiveAnim (targetItemCheckerList);

			if (ParagraphStep == Paragraph.StepType.QuestionStep) {
				hideElementGO ();
				displayElementGO ();
			}
		} else {
			//Adding QuestionStepParaPF to the root GameObject
			GameObject QuestionStepParaGO = BasicGOOperation.InstantiateNGUIGO(QuestionStepParaPF,parentGO.transform);
			MathTrigger.Instance.questionText = (LineList [0] as TextLine).DisplayText;
			ElementGO = QuestionStepParaGO;
		}

		//Traversing through Child Element - 3
		setChildAnalyticsId ();
		UserAction.AddParaChild( getParaChild(this));
		//Traversing through Child Element - 4
		setChildScoreValues ();
		return ParaContentTableGO;
	}
	override protected void initGOProp(GameObject _elementGO){
		if(ParagraphStep == Paragraph.StepType.QuestionStep){
			Parent.setCurrentChild (this);
		}
	}
	override protected void updateGOProp(GameObject _elementGO){
		ScreenManager.resizeTexDraw (this);
		ScreenManager.resizeChildren (ElementGO);
//		if (ParagraphStep == Paragraph.StepType.Comprehension) {
////			ScreenManager.SetAsScreenSize (ElementGO);
//		} else if (ParagraphStep == Paragraph.StepType.QuestionStep) {
//			ScreenManager.resizeChildren (ElementGO);
//		}
	}
	/// <summary>
	/// Resizes the content of the Center Scroll View.
	/// </summary>
	/// <param name="CenterContentGO">Center content Scroll View GameObject.</param>
	/// <param name="ParaContentTableGO">Paragraph content table Gameobject.</param>
	public void resizeCenterContent(GameObject CenterContentGO,GameObject ParaContentTableGO){
		Vector3 scale = BasicGOOperation.scale;
		Vector3 remainingSize = ParaContentTableGO.transform.parent.gameObject.GetComponent<UISprite> ().CalculateBounds().size;
//		Debug.Log ("Bound of Parent GameObject "+ParaContentTableGO.transform.parent.gameObject.name+remainingSize.size.x+" - "+remainingSize.size.y);
		//Calculate remaining width after subtracting from Parent height/width of PAraContentTableGO 
		foreach (Transform childTransform in ParaContentTableGO.transform) {
			if (childTransform.gameObject != CenterContentGO) {
				//Check for size of other GameObjects other than Default Types
				if (childTransform.gameObject.GetComponent<TEXDrawNGUI> () != null) {
//					Debug.Log (BasicGOOperation.ElementSize(childTransform.gameObject).y);
				}

				Vector3 childSize = NGUIMath.CalculateAbsoluteWidgetBounds(childTransform).size;
				childSize.x = childSize.x / scale.x; childSize.y = childSize.y / scale.y;
//				Debug.Log ("Bound of GameObject "+childTransform.gameObject.name+(childSize.x)+" - "+(childSize.y));
				remainingSize = remainingSize - childSize;
//				Debug.Log ("Remaining Bound" + remainingSize.x +" - "+ remainingSize.y);
			}
		}
		resizeCenterContentChild (CenterContentGO, ParagraphAlign, remainingSize);
	}
	/// <summary>
	/// Resizes the dimensions of Child GameObjects of Center Scroll View.
	/// </summary>
	/// <param name="CenterContentGO">Center content GameObject</param>
	/// <param name="align">Align Type</param>
	/// <param name="newSize">New size of Center Scroll View</param>
	public void resizeCenterContentChild(GameObject CenterContentGO, AlignType align, Vector3 newSize){
		if (align == AlignType.Horizontal) {
			//Change CenterContentGO.width
			GameObject ContainerGO = BasicGOOperation.getChildGameObject(CenterContentGO,"Container");
			ContainerGO.GetComponent<UIWidget> ().width = (int)newSize.x;

			GameObject LineTable = BasicGOOperation.getChildGameObject(CenterContentGO,"LineTablePF");
			foreach (Transform childTransform in LineTable.transform) {
				if (childTransform.gameObject.GetComponent<TEXDrawNGUI>() != null) {
					childTransform.gameObject.GetComponent<TEXDrawNGUI> ().width = (int)newSize.x;
				}else if (childTransform.gameObject.GetComponent<UILabel>() != null) {
					childTransform.gameObject.GetComponent<UILabel> ().width = (int)newSize.x;
				}
			}
		} else {
			//Change CenterContentGO.height
			GameObject ContainerGO = BasicGOOperation.getChildGameObject(CenterContentGO,"Container");
			ContainerGO.GetComponent<UIWidget> ().height = (int)newSize.y;
			//Calculating Delta Height for Container child and self panel
			float scale_y = ((float)ScreenManager.ScreenHeight() / 900);
			float delta_h = - ((int)newSize.y / 2)*scale_y;
			//Changing Container Height = - Half of its size
			Vector3 containerPosition = ContainerGO.transform.localPosition;
			containerPosition.y = delta_h;
			ContainerGO.transform.localPosition = containerPosition;
			//Changing Panel Height = - Half of its size
			float height = CenterContentGO.GetComponent<UIPanel> ().height;
			float width = CenterContentGO.GetComponent<UIPanel> ().width;
			CenterContentGO.GetComponent<UIPanel> ().SetRect (0, containerPosition.y, width, ContainerGO.GetComponent<UIWidget>().height);
		}
	}
	public void addStartWorkingRuleBtn(GameObject LineTableGO){
		if (GameObject.Find ("PauseImgBtn")) {
			EventDelegate.Set(GameObject.Find ("PauseImgBtn").GetComponent<UIButton>().onClick, delegate() { (this.Parent as ComprehensionBody).nextParaTrigger(); });
		}
		if (ParagraphStep == Paragraph.StepType.Comprehension) {

			GameObject StartWorkingRuleBtnPF = Resources.Load (LocationManager.COMPLETE_LOC_OTHER_TYPE + LocationManager.NAME_START_WORKING_RULE_BTN)as GameObject;
			GameObject StartWorkingRuleBtn = BasicGOOperation.InstantiateNGUIGO(StartWorkingRuleBtnPF,LineTableGO.transform);
			EventDelegate.Set(StartWorkingRuleBtn.GetComponentInChildren<UIButton>().onClick, delegate() { (this.Parent as ComprehensionBody).nextParaTrigger(); });
//			if (ParagraphAlign == AlignType.Vertical) {
//				ScreenManager.SetAsScreenWidth (StartWorkingRuleBtn,30);
//			}
		}
	}

	override public void  setChildAnalyticsId(){
		int index = 1;
		foreach (Line line in LineList) {
			line.AnalyticsId = index;
			line.setChildAnalyticsId ();
			index++;
		}
	}

	//----------------------Score Values ----------------------------
	override public void  setChildScoreValues(){
		setupDefaultScoreValues ();
		adjustForWeightage();
		setupScoreValues ();
		foreach (Line line in LineList) {
			line.setChildScoreValues ();
		}
	}
	override protected void setupDefaultScoreValues(){
		
		if (htmlNode != null) {
			//scoreWeightage
			if (htmlNode.Attributes [AttributeManager.SCORE_WEIGHTAGE] != null) {
				scoreTracker.scoreWeightage = int.Parse(htmlNode.Attributes [AttributeManager.SCORE_WEIGHTAGE].Value);
			} 
			if (htmlNode.Attributes [AttributeManager.MAX_SCORE] == null) {
				scoreTracker.maxScore = (Parent as ComprehensionBody).scoreTracker.maxScore / (Parent as ComprehensionBody).ParagraphList.Count;
			} else {
				scoreTracker.maxScore = float.Parse(htmlNode.Attributes [AttributeManager.MAX_SCORE].Value);
			}
			if (htmlNode.Attributes [AttributeManager.MIN_SCORE] == null) {
				scoreTracker.maxScore = (Parent as ComprehensionBody).scoreTracker.maxScore / (Parent as ComprehensionBody).ParagraphList.Count;
			} else {
				scoreTracker.maxScore = float.Parse(htmlNode.Attributes [AttributeManager.MAX_SCORE].Value);
			}
		}
	}
	override protected void adjustForWeightage(){
		//Setting maxScore/minScore based on scoreWeightages
		ComprehensionBody body = (Parent as ComprehensionBody);
		float sumOfScoreWeightages = body.scoreTracker.childScoreWeightageSum;
		if (sumOfScoreWeightages == 0) {
			foreach (Paragraph para in body.ParagraphList) {
				sumOfScoreWeightages += para.scoreTracker.scoreWeightage;
			}
		} 
		scoreTracker.maxScore = scoreTracker.scoreWeightage*(body.scoreTracker.maxScore / sumOfScoreWeightages);
		scoreTracker.minScore = scoreTracker.scoreWeightage*(body.scoreTracker.minScore / sumOfScoreWeightages);
	}
	//---------------------- Analytics----------------------------
	public TargetEntity getParaChild(Paragraph para){
		TargetEntity paraEntity = new TargetEntity("Paragraph", para.AnalyticsId);

		//Adding Lines
		foreach (Line line in para.LineList) {
			TargetEntity lineEntity = new TargetEntity("Line", line.AnalyticsId);
//			Debug.Log ("Line"+line.AnalyticsId.ToString());
			//Adding Questions
			foreach (QuestionChecker ques in line.QuestionList) {
				TargetEntity quesEntity = new TargetEntity("Question", ques.AnalyticsId);
//				Debug.Log ("Question"+ques.AnalyticsId.ToString());
				//Adding Options
				foreach (OptionChecker option in ques.ChildList) {
					TargetEntity optionEntity = new TargetEntity("Option", option.AnalyticsId);
					quesEntity.ChildEntityList.Add (optionEntity);
//					Debug.Log ("Option"+option.AnalyticsId.ToString());
				}

				lineEntity.ChildEntityList.Add (quesEntity);
			}

			paraEntity.ChildEntityList.Add (lineEntity);
		}

		return paraEntity;
	}
	//----------------------Animations ----------------------------
	override public void hideElementGO(){
		BasicGOOperation.hideElementGO (ElementGO);
	}
	override public void displayElementGO(){
		BasicGOOperation.displayElementGO (ElementGO);
		for (int i = 0; i < LineList.Count; i++) {
			LineList[i].hideElementGO ();
		}
		LineList[0].displayElementGO ();
	}
	/// <summary>
	/// Sets up first Getting active animation.
	/// </summary>
	/// <param name="_questionList">Question list.</param>
//	public void setUpChildActiveAnim(List<QuestionChecker> _questionList){
//		Debug.Log ("setUpChildActiveAnim"+_questionList.Count.ToString());
//		if (_questionList.Count > 0)
//			_questionList [0].activateAnim ();
//	}
	/// <summary>
	/// Sets up the next target trigger
	/// </summary>
	/// <param name="questionChecker">Question checker.</param>
//	public void nextTargetTrigger(QuestionChecker questionChecker){
//		Debug.Log (QuestionList.Count);
//		int currentCounter = QuestionList.IndexOf (questionChecker);
//		if (currentCounter < QuestionList.Count-1) {
//			QuestionList [currentCounter + 1].activateAnim ();
//		} else {
//			finishQuestionStep ();
//		}
//	}
	/// <summary>
	/// Finishes the question step.
	/// </summary>
	public void finishQuestionStep(){
		Debug.Log ("QuestionStep finished");
		GameObject ParaContentTable = BasicGOOperation.getChildGameObject (ElementGO, "ParaContentTable");
		ParaContentTable.SetActive (false);
		postEntityOps ();
	}

	//-------------Post Entity -------------------
	override public void postEntityOps(){
		postScoreCalc ();
		GameObject PostSubmitTablePF = Resources.Load (LocationManager.COMPLETE_LOC_PARAGRAPH_TYPE + LocationManager.NAME_POST_SUBMIT_TABLE) as GameObject;
		GameObject PostSubmitTableGO = BasicGOOperation.InstantiateNGUIGO (PostSubmitTablePF, ElementGO.transform);
		PostSubmitTableGO.GetComponentInChildren<TEXDrawNGUI> ().text = postSubmitText;
		EventDelegate.Set(PostSubmitTableGO.GetComponentInChildren<UIButton>().onClick, delegate() { (this.Parent as ComprehensionBody).nextParaTrigger(); });
		if (!(this.Parent as ComprehensionBody).checkForNextPara ()) {
			PostSubmitTableGO.GetComponentInChildren<UIButton> ().gameObject.GetComponentInChildren<UILabel>().text ="Working Rule finished";
		}

	}
	protected void postScoreCalc(){
		foreach (Line line in LineList) {
			foreach (QuestionChecker ques in line.QuestionList) {
				scoreTracker.attemptScore += ques.scoreTracker.attemptScore;
			}
		}
	}

}
