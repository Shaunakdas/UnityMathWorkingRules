﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineDisplay : MonoBehaviour {

	//Prefabs
	public GameObject smallMarkerPF,numberMarkerPF;

	//ScreenValues
	public float screenDimensionHeight{get; set;}
	public float screenDimensionWidth{get; set;}
	public float screenPadding{get; set;}
	public float startYPosition{get; set;}

	//Question Values
	public int IntegerCount{get;set;}
//	public int endInteger{get;set;}
	public int numberBreak{get;set;}

	//To be calculated
	public float cellWidth{get; set;}
	public float allocatedHeight{get; set;}
	public float cellHeight{get; set;}
	public int totalBreakerCount{get;set;}

	//Post Questions
	public float correctAnswer{get;set;}
	public float errorTolerance{get; set;}
	public float userSelectorYPosition{get; set;}
	public float correctSelectorYPosition{get; set;}


	public void defaultValues(){
		screenDimensionHeight=936f; screenDimensionWidth=520f;
		IntegerCount=10; numberBreak=5; correctAnswer = 5f;
		screenPadding=30f; startYPosition = -420f;
		errorTolerance = 25f;
	}
	public void initNumberLineCalculations(){
		//To be calculated
		cellWidth = 40f;
		allocatedHeight = (screenDimensionHeight / 2) - screenPadding + Mathf.Abs(startYPosition);
		totalBreakerCount = 1+(numberBreak*(IntegerCount-1));
		cellHeight = allocatedHeight/totalBreakerCount;
	}


}
