# UnityMathWorkingRules

## Git handling of Unity Projects
* [SOF: Git Unity Procedure](https://stackoverflow.com/questions/21573405/how-to-prepare-a-unity-project-for-git) - How to prepare a Unity project for git? 

## Tutorials to be used 
* [NGUI Drag Drop](https://www.youtube.com/watch?v=UK3aMHRfgcw) How to setup DragDrop for NGUI


## Project Structure for DragDrop
*   ScrollView (UI Panel, UI Scroll View) {Depth - 1}
		Container (UI Widget, BoxCol[Auto-adjust], UI DragScrollView) {Depth - 2}
		Grid
			Sprite  (UI Sprite, UI DragDropItem, BoxCol[Auto-adjust])
	Panel (UI DragDropRoot, UI Panel) {Depth - 2}
	Sprite (UI Sprite, BoxCol[AutoAdjust], UI DragDopContainer) {Depth - 5}
		Grid (UI Grid) 


## Layout Structure
* Horizontal Layout 
	Left|Default|Right
* Vertical LAyout
	Top|Default|Bottom

## Adding Monoscript in runtime. 

```
gameObject.AddComponent<Monoscript>();
```
Remember to use Awake() method to initialize instead of Start() in Monoscript.

## .NET Version 

For Unity 5.6, .NET verision is 3.5

## Testing in Unity
[Unity Test Runner][https://docs.unity3d.com/Manual/testing-editortestsrunner.html]
[Unity Tests Runner Wiki][https://bitbucket.org/Unity-Technologies/unitytesttools/wiki/]
[Unity Test Runner Test command Line][https://bitbucket.org/Unity-Technologies/unitytesttools/wiki/UnitTestsRunner]
[Unity Comand Line Arguments][https://docs.unity3d.com/Manual/CommandLineArguments.html]
[Unity Command Line Issue][https://forum.unity3d.com/threads/editor-test-runner-does-nothing-from-the-command-line.381750/]
[Unity Log files][https://docs.unity3d.com/Manual/LogFiles.html]
[Unity Test Tools SOF][https://stackoverflow.com/questions/34659654/how-to-set-up-unity-test-tools-on-unity-5]