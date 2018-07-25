using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class DragSourceCell : Cell {

	//-------------Common Attributes -------------------
	public enum SeriesType{Integer,Prime};
	public SeriesType SourceType;

	public delegate void Dragged();
	public event Dragged DroppedOnSurface;
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public DragSourceCell(string type, string id, string displayText):base(type){
		CellId =  (id);
		DisplayText = displayText;
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}
	//Constructor
	public DragSourceCell(string type, string displayText):base(type){
		DisplayText = displayText;
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}
	/// <summary>
	/// Initializes a new instance of the DragSourceCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public DragSourceCell(HtmlNode cell_node):base(cell_node){
		//		CellList = new List<Cell> ();
		HtmlAttribute attr_tag = cell_node.Attributes [AttributeManager.ATTR_ID];
		if (attr_tag != null) {
			CellId = cell_node.Attributes [AttributeManager.ATTR_ID].Value;
		}
		DisplayText = StringWrapper.changeString(cell_node.InnerText);
		prefabName = LocationManager.NAME_DRAG_SOURCE_CELL;
	}
	public void getSourceType(string source_type){
		SourceType = (SeriesType)System.Enum.Parse (typeof(SeriesType),StringWrapper.ConvertToPascalCase(source_type),true);
	}


	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void updateGOProp(GameObject ElementGO){
//		Debug.Log ("Updating Text of Cell" + DisplayText);
		GameObject labelGO = BasicGOOperation.getChildGameObject (ElementGO, "Label");
		labelGO.GetComponent<UILabel> ().text = DisplayText;
		ElementGO.name = ElementGO.name + "_"+ generateStandardName(DisplayText);
		ElementGO.GetComponent<CustomDragDropItem> ().restriction = (DragAlign == Paragraph.AlignType.Horizontal) ? UIDragDropItem.Restriction.Horizontal : UIDragDropItem.Restriction.Vertical;
		// If Cell is child of DragSource Row, its scroll should be modified to math parent scroll for horizontal dragging
		if (ElementGO.transform.parent.gameObject.name == "Grid") {
			
			GameObject containerGO = BasicGOOperation.getChildGameObject (BasicGOOperation.getChildGameObject (this.Parent.ElementGO, "ScrollView"), "Container");
			ElementGO.GetComponent<CustomDragDropItem> ().DragScrollView = containerGO;
		} else {
			// If Cell is not a child of DragSource Row
//			ElementGO.GetComponent<TEXDrawNGUI>().Redraw();
		}
		EventDelegate.Set(ElementGO.GetComponent<UIButton>().onClick, delegate() { MathTrigger.Instance.DragCellTrigger(ElementGO); });
	}
	public string generateStandardName(string text){
		int number; string newText = text;
		if (int.TryParse (text, out number)) {
			number = number + 100;
			newText = number.ToString (); int rem = 3 - newText.Length; 
			for (int i = 0; i < rem; i++) {
				newText = "0" + newText;
			}
		}
		return newText;
	}
	//-------------Animations -------------------
	public GameObject ScrollPanelGO(){
		return ElementGO.transform.parent.parent.gameObject;
	}
	public void dragToDropZone(GameObject DropZoneHolderGO, EventDelegate _nextEvent){
		Debug.Log ("Correct DragItem location "+BasicGOOperation.NGUIPosition(ElementGO.transform).x);
		Vector3 posDiff = BasicGOOperation.NGUIPosition (DropZoneHolderGO.transform) - BasicGOOperation.NGUIPosition (ElementGO.transform);
		//Dragging ScrollView to put correct drag item just below dropzone
		Vector3 ScrollPosDiff = new Vector3 ( posDiff.x, 0f,0f);
		Vector3 DragPosDiff = new Vector3 ( 0f, posDiff.y,0f);
		//Dragging DragItem to put correct drag item inside dropzone
		SpringPanel.Begin (ScrollPanelGO (), ScrollPosDiff, 8f).onFinished += delegate{
			
			ElementGO.GetComponent<CustomDragDropItem>().StartDragging();
			GameObject ElementCloneGO = ElementGO.GetComponent<CustomDragDropItem>().itemClone;
			Vector3 newPos = ElementCloneGO.transform.localPosition;
			newPos = BasicGOOperation.NGUIPosition (DropZoneHolderGO.transform) - BasicGOOperation.NGUIPosition (ElementCloneGO.transform) + newPos;


			FastSpringPosition.Begin (ElementCloneGO, newPos, 8f).onFinished += delegate{
				Debug.Log("SpringPosition finished");
				Debug.Log("DroppedOnSurface finished"+(DroppedOnSurface != null));
				ElementCloneGO.GetComponent<CustomDragDropItem>().StopDragging(DropZoneHolderGO);
				if (DroppedOnSurface != null)
					DroppedOnSurface();
				if(_nextEvent != null)
					_nextEvent.Execute();
			};
//			verticalDragAnim(ElementGO,DropZoneHolderGO);
		};

	}
	public void verticalDragAnim(GameObject ElementGO, GameObject targetGO){
		ElementGO.AddComponent<TweenPosition> ();
//		ElementGO.AddComponent<TweenPosition>().
		ElementGO.GetComponent<TweenPosition> ().duration = 4f;
		ElementGO.GetComponent<TweenPosition> ().enabled = false;
		ElementGO.GetComponent<TweenPosition> ().from =  ElementGO.transform.localPosition;
		Vector3 newPos = ElementGO.transform.localPosition;
		newPos = BasicGOOperation.NGUIPosition (targetGO.transform) - BasicGOOperation.NGUIPosition (ElementGO.transform) + newPos;
//		Debug.Log(targetGO.transform.position);
//		Debug.Log(ElementGO.transform.position);
//		Debug.Log(targetGO.transform.localPosition);
//		Debug.Log(ElementGO.transform.localPosition);
//		Debug.Log(BasicGOOperation.NGUIPosition(targetGO.transform));
//		Debug.Log(BasicGOOperation.NGUIPosition(ElementGO.transform));
//		Debug.Log(BasicGOOperation.ScaledVector(targetGO.transform.localPosition));
//		Debug.Log(BasicGOOperation.ScaledVector(ElementGO.transform.localPosition));
		ElementGO.GetComponent<TweenPosition> ().to = newPos;
		ElementGO.GetComponent<TweenPosition> ().Play (true);
	}
}
