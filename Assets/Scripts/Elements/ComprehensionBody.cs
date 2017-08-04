using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Linq;

public class ComprehensionBody : BaseElement {
	//-------------Common Attributes -------------------

	//List of child Line elements
	public List<Paragraph> ParagraphList{get; set;}
	public int CurrentParaCounter{ get; set; }
	public Paragraph comprehensionParaElem;
	public ScoreManager scoreMan;
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Empty Contructor
	public ComprehensionBody(){
	}
	/// <summary>
	/// Initializes a new instance of the Paragraph class with HTMLNode attribute
	/// </summary>
	/// <param name="para"></param>
	public ComprehensionBody(HtmlNode body_node){
		htmlNode = body_node;
		ParagraphList = new List<Paragraph> ();

		prefabName = LocationManager.NAME_COMPREHENSION_BODY;
		parseChildNode (body_node);
		scoreMan = new ScoreManager ();


	}

	/// <summary>
	/// Parses the Paragraph Node to generate Line nodes
	/// </summary>
	override public void parseChildNode(HtmlNode body_node){
		//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = body_node.Elements(AttributeManager.TAG_P) ;

		if (node_list != null) {
			//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode para_node in node_list) {
				Paragraph newPara = new Paragraph (para_node);
				newPara.Parent = this;
				//Add Line node into LineList
				ParagraphList.Add (newPara);

			}
		}
	}
	public TargetEntity generateTargetTree(){
		TargetEntity entity = new TargetEntity(this.GetType().ToString(),1);

		//Adding Paragraphs
		foreach (Paragraph para in ParagraphList) {
			TargetEntity paraEntity = new TargetEntity("Paragraph", para.AnalyticsId);

			//Adding Lines
			foreach (Line line in para.LineList) {
				TargetEntity lineEntity = new TargetEntity("Line", line.AnalyticsId);

				//Adding Questions
				foreach (QuestionChecker ques in line.QuestionList) {
					TargetEntity quesEntity = new TargetEntity("Question", ques.AnalyticsId);

					//Adding Options
					foreach (OptionChecker option in ques.ChildList) {
						TargetEntity optionEntity = new TargetEntity("Option", option.AnalyticsId);
						quesEntity.ChildEntityList.Add (optionEntity);
					}

					lineEntity.ChildEntityList.Add (quesEntity);
				}

				paraEntity.ChildEntityList.Add (lineEntity);
			}

			entity.ChildEntityList.Add (paraEntity);
		}
		return entity;
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the corresponding GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	override public GameObject generateElementGO(GameObject parentGO){

		GameObject ComprehensionBodyPF = Resources.Load (LocationManager.COMPLETE_LOC_BODY_TYPE + prefabName)as GameObject;
		ElementGO = BasicGOOperation.InstantiateNGUIGO(ComprehensionBodyPF,parentGO.transform);
		initGOProp (ElementGO);
		if(true){
//			Paragraph is of type QuestionStep
			int StepCounter =1;
			GameObject ParaCounterTableGO = BasicGOOperation.getChildGameObject (BasicGOOperation.getChildGameObject (ElementGO, "ParaTable"), "ParaCounterTable");
			foreach (Paragraph para in ParagraphList) {
				if (para.ParagraphStep == Paragraph.StepType.Comprehension) {
					//					Generate Comprehension Paragraph
					CurrentParaCounter = 0;
					comprehensionParaElem = para;
					para.generateElementGO (ElementGO);
				} else {
					
					GameObject ParaCounterPF = Resources.Load (LocationManager.COMPLETE_LOC_BODY_TYPE + LocationManager.NAME_PARA_COUNTER)as GameObject;
					GameObject ParaCounterGO = BasicGOOperation.InstantiateNGUIGO(ParaCounterPF,ParaCounterTableGO.transform);
					ParaCounterGO.GetComponentInChildren<UILabel> ().text = StepCounter.ToString ();
					StepCounter++;
				}

			}
			ParaCounterTableGO.transform.SetAsLastSibling ();

			//ScoreTable
			scoreMan.init(this);
			updateGOProp(ElementGO);
			setChildAnalyticsId ();
			UserAction.MasterEntity = generateTargetTree ();
			setChildScoreValues ();
			BasicGOOperation.RepositionChildTables (ElementGO);
//			setUpChildActiveAnim (targetItemCheckerList);
		}
		return ElementGO;
	}
	override public void initGOProp (GameObject _elementGO){
		//Setting Background Size
		ScreenManager.SetAsScreenSize (_elementGO); setRootSize (_elementGO);
		//Setting ScoreManager Values
		setupScoreSettings();
		Paragraph firstStepPata = ParagraphList.Where(s => s.ParagraphStep == Paragraph.StepType.QuestionStep).ElementAt(0);
		//Setting MaxParaScore
		scoreMan.currentPara = firstStepPata;
	}
	override public void updateGOProp(GameObject _elementGO){
		//Setting ParaCounterTable as screen bottom
		GameObject ParaTableGO = BasicGOOperation.getChildGameObject (ElementGO, "ParaTable");
		ScreenManager.SetTableAsScreenBottom (ParaTableGO);
	}
	override public void  setChildAnalyticsId(){
		int index = 1;
		foreach (Paragraph para in ParagraphList) {
			para.AnalyticsId = index;
			index++;
		}
	}
	public override void setCurrentChild (BaseElement _childElement)
	{
		base.setCurrentChild (_childElement);
		scoreMan.setCurrentPara(CurrentChild as Paragraph);
	}
	public void setRootSize(GameObject _elementGO){
		UIRoot Root = ElementGO.GetComponentInParent<UIRoot> (); 
		Root.manualHeight = Screen.height; Root.manualWidth = Screen.width; 
	}
	//----------------------Score Values ----------------------------
	override public void  setChildScoreValues(){
		setupScoreValues ();
	}
	override public void setupScoreValues(){
		if (htmlNode != null) {
			if (htmlNode.Attributes [AttributeManager.MAX_SCORE] == null) {
				scoreTracker.maxScore = ScoreCalculator.DEFAULT_MAX_SCORE;
			} else {
				scoreTracker.maxScore = float.Parse(htmlNode.Attributes [AttributeManager.MAX_SCORE].Value);
			}
			if (htmlNode.Attributes [AttributeManager.MIN_SCORE] == null) {
				scoreTracker.maxScore = ScoreCalculator.DEFAULT_MIN_SCORE;
			} else {
				scoreTracker.maxScore = float.Parse(htmlNode.Attributes [AttributeManager.MAX_SCORE].Value);
			}
		}
	}
	//----------------------Analytics ----------------------------
	public void setupScoreSettings(){
		scoreMan.setupScoreSettings (ParagraphList);
	}
	public void setCurrentScoreSettings(Paragraph para){

	}
	//----------------------Animations ----------------------------
	public bool checkForNextPara(){
		return (CurrentParaCounter  < (ParagraphList.Count-1));
	}
	public void nextParaTrigger(){
		if (CurrentParaCounter > 0) {
			ParagraphList [CurrentParaCounter].ElementGO.SetActive (false);
		}
		setQuesVisibility (false);
		if (checkForNextPara()) {
			Paragraph nextPara = ParagraphList [CurrentParaCounter+1];
			nextPara.generateElementGO (BasicGOOperation.getChildGameObject (ElementGO, "ParaTable"));
			GameObject ParaCounterTableGO = BasicGOOperation.getChildGameObject (BasicGOOperation.getChildGameObject (ElementGO, "ParaTable"), "ParaCounterTable");
			ParaCounterTableGO.transform.SetAsLastSibling ();
			NGUITools.FindInParents<UIRoot> (ElementGO).gameObject.GetComponent<HTMLParserTest> ().RepositionAfterEndOfFrame ();
		}
		CurrentParaCounter++;
		changeParaCounterTableDisplay (CurrentParaCounter);
	}

	public void setQuesVisibility(bool displayFlag){
		foreach (UIWidget widget in comprehensionParaElem.ElementGO.GetComponentsInChildren<UIWidget> ()){
			if(displayFlag)
				widget.depth = widget.depth + 6;
			else
				widget.depth = widget.depth - 6;
		}
	}
	public void changeParaCounterTableDisplay(int currentCounter){

		GameObject ParaCounterTableGO = BasicGOOperation.getChildGameObject (ElementGO, "ParaCounterTable");
		foreach (Transform paraCounterTf in ParaCounterTableGO.transform) {
			Color counterColor = paraCounterTf.gameObject.GetComponent<UISprite> ().color;
//			Debug.Log ("color of Counter Sprite"+counterColor.a.ToString());
			if (paraCounterTf.GetSiblingIndex() != CurrentParaCounter - 1) {
				counterColor.a = 0.5f;
			} else {
				counterColor.a = 1.0f;
			}
			paraCounterTf.gameObject.GetComponent<UISprite> ().color = counterColor;
		}
	}
}
