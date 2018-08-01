﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTrigger : Singleton<MathTrigger> {

	protected MathTrigger () {} // guarantee this will be always a singleton only - can't use the constructor!

	public string questionText;
	public string sectionCompleteText = "Collect all ingredients";
	public OptionChecker optionchecker =null;
	public void NextPara(){
		GameObject.Find ("UI Root").GetComponent<HTMLParserTest> ().comprehensionBody.nextParaTrigger ();
	}

	public void PopupScore(int value, Vector3 pos){
		pos.x = (pos.x*1200f*ScreenManager.widthScaler) + (Screen.width/2);
		pos.y = (pos.y*1372f*ScreenManager.heightScaler) + (Screen.height/2) ;
		Debug.Log (pos);
		LevelManager.THIS.PopupScore(value, pos, 2);
	}
	public void StartMath(){
		Debug.Log ("Math Started");
		GameObject.Find ("GameObjectHolder").GetComponent<GameObjectHolder>().Dragon.SetActive (true);
//		GameObject.Find ("UI Root").GetComponent<HTMLParserTest> ().comprehensionBody.nextParaTrigger ();
	}

	public void sectionComplete(){
		GameObject.Find("Canvas").transform.Find("SectionComplete").gameObject.SetActive(true);
	}

	//	Current MathGame Elements which are activated
	public Paragraph currentParagaph(){
		ComprehensionBody compr = GameObject.Find ("UI Root").GetComponent<HTMLParserTest> ().comprehensionBody;
		return compr.ParagraphList [compr.CurrentParaCounter];
	}

	public Line currentLine(){
		return currentParagaph ().CurrentChild as Line;
	}

	public QuestionChecker currentQuestion(){
		foreach(QuestionChecker ques in currentLine ().QuestionList){
			if (ques.ItemAttemptState == OptionChecker.AttemptState.Activated){
				return ques;
			}
		}
		return null;
	}

	public OptionChecker currentOption(){
		foreach(OptionChecker _option in currentQuestion().ChildList){
			if (_option.ItemAttemptState == OptionChecker.AttemptState.Activated){
				return _option;
			}
		}
		return null;
	}

	public List<TableLine> currentDragSourceList(){
		return currentParagaph ().DragSourceTableList;
	}

	public List<TextCell> currentDragSourceCellList(){
		List<TextCell> _dragSourceCellList = new List<TextCell> ();
		foreach(TableLine _table in currentDragSourceList()){
			foreach (Row _row in _table.RowList) {
				foreach (Cell _cell in _row.CellList) {
					Debug.Log (_cell.gameObject);
//					_dragSourceCellList.Add (_cell as TextCell);
				}
			}
		}
		return _dragSourceCellList;
	}

	public List<GameObject> currentDragItemList(){
		List<GameObject> _currentDragItemList = new List<GameObject> ();
		foreach (CustomDragDropItem _item in GameObject.Find ("UI Root").GetComponentsInChildren<CustomDragDropItem>()) {
			_currentDragItemList.Add (_item.gameObject);
		}
		return _currentDragItemList;
	}
	public void dragCorrectItem(){
		DropZoneHolder _holder = currentQuestion ().ParentChecker as DropZoneHolder;
		_holder.correctionAnim (currentOption () as DropZoneOptionChecker, currentQuestion () as DropZoneQuestionChecker, null);
	}

	public void startProcess(){
//		StartCoroutine(MyCoroutine());

	}

	public void DragCellTrigger(GameObject _elementGO){
		Rezero.GameController.Instance.characterReachTarget (_elementGO, GUIType.NGUI);
	}
		
	public void QuesActivTrigger(GameObject _elementGO){
		Rezero.GameController.Instance.enemyReachTarget (_elementGO, GUIType.NGUI);
	}

	IEnumerator MyCoroutine()
	{

		yield return new WaitForSeconds(6f);
		dragCorrectItem ();
	}

	public Vector3 convertNGUIToArrow(Vector3 target){
		float arrowWindowStartX = 2f;
		float arrowWindowEndX = 12f;
		float arrowWindowWidthX = arrowWindowEndX - arrowWindowStartX;
		float arrowWindowStartY = -14f;
		float arrowWindowEndY = 14f;
		float arrowWindowWidthY = arrowWindowEndY - arrowWindowStartY;
		float nguiWindowStartX = -0.5f;
		float nguiWindowEndX = 0.5f;
		float nguiWindowWidthX = nguiWindowEndX - nguiWindowStartX;
		float nguiWindowStartY = -1.7f;
		float nguiWindowEndY = 1.7f;
		float nguiWindowWidthY = nguiWindowEndY - nguiWindowStartY;
		target.x = arrowWindowStartX + ((target.x - nguiWindowStartX) * (arrowWindowWidthX/nguiWindowWidthX));
		target.y = arrowWindowStartY + ((target.y - nguiWindowStartY) * (arrowWindowWidthY/nguiWindowWidthY));
		return target;
	}
}
