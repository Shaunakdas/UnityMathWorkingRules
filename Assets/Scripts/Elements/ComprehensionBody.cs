using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class ComprehensionBody : BaseElement {
	//-------------Common Attributes -------------------

	//List of child Line elements
	public List<Paragraph> ParagraphList{get; set;}
	public int CurrentParaCounter{ get; set; }
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Empty Contructor
	public ComprehensionBody(){
	}
	/// <summary>
	/// Initializes a new instance of the Paragraph class with HTMLNode attribute
	/// </summary>
	/// <param name="para"></param>
	public ComprehensionBody(HtmlNode body_node){
		ParagraphList = new List<Paragraph> ();

		prefabName = LocationManager.NAME_COMPREHENSION_BODY;
		Debug.Log ("Initializing paragraph node of type "+body_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		parseChildNode (body_node);
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

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the corresponding GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	override public GameObject generateElementGO(GameObject parentGO){

		GameObject ComprehensionBodyPF = Resources.Load (LocationManager.COMPLETE_LOC_BODY_TYPE + prefabName)as GameObject;
		ElementGO = BasicGOOperation.InstantiateNGUIGO(ComprehensionBodyPF,parentGO.transform);
		if(true){
//			Paragraph is of type QuestionStep
			int StepCounter =1;
			foreach (Paragraph para in ParagraphList) {
				if (para.ParagraphStep == Paragraph.StepType.Comprehension) {
					//					Generate Comprehension Paragraph
					CurrentParaCounter = 0;
					para.generateElementGO (ElementGO);
				} else {
					GameObject ParaCounterTableGO = BasicGOOperation.getChildGameObject (ElementGO, "ParaCounterTable");
					GameObject ParaCounterPF = Resources.Load (LocationManager.COMPLETE_LOC_BODY_TYPE + LocationManager.NAME_PARA_COUNTER)as GameObject;
					GameObject ParaCounterGO = BasicGOOperation.InstantiateNGUIGO(ParaCounterPF,ParaCounterTableGO.transform);
					ParaCounterGO.GetComponentInChildren<UILabel> ().text = StepCounter.ToString ();
					StepCounter++;
				}

			}

//			setUpChildActiveAnim (targetItemCheckerList);
		}
		return ElementGO;
	}


	//----------------------Animations ----------------------------
	public void nextParaTrigger(){
		CurrentParaCounter++;
		Paragraph nextPara = ParagraphList [CurrentParaCounter];
		nextPara.generateElementGO (ElementGO);
	}
}
