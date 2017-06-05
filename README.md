# UnityMathWorkingRules

## Git handling of Unity Projects
* [SOF: Git Unity Procedure](https://stackoverflow.com/questions/21573405/how-to-prepare-a-unity-project-for-git) - How to prepare a Unity project for git? 

## Tutorials to be used 
* [NGUI Drag Drop] (https://www.youtube.com/watch?v=UK3aMHRfgcw) How to setup DragDrop for NGUI


## Project Structure for DragDrop
*   -> ScrollView (UI Panel, UI Scroll View) {Depth - 1}
		-> Container (UI Widget, BoxCol[Auto-adjust], UI DragScrollView) {Depth - 2}
		-> Grid
			-> Sprite  (UI Sprite, UI DragDropItem, BoxCol[Auto-adjust])
	-> Panel (UI DragDropRoot, UI Panel) {Depth - 2}
	-> Sprite (UI Sprite, BoxCol[AutoAdjust], UI DragDopContainer) {Depth - 5}
		-> Grid (UI Grid) 