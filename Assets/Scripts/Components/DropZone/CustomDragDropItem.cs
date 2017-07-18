using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDragDropItem : UIDragDropItem {
	public GameObject DragScrollView;
	public GameObject itemClone;
	public override void StartDragging ()
	{

		Debug.Log ("StartDragging called");
		if (!interactable) return;

		if (!mDragging)
		{
			if (cloneOnDrag)
			{
				mPressed = false;
				GameObject clone = transform.parent.gameObject.AddChild(gameObject);
				clone.transform.localPosition = transform.localPosition;
				clone.transform.localRotation = transform.localRotation;
				clone.transform.localScale = transform.localScale;
				itemClone = clone;
				UIButtonColor bc = clone.GetComponent<UIButtonColor>();
				if (bc != null) bc.defaultColor = GetComponent<UIButtonColor>().defaultColor;

				if (mTouch != null && mTouch.pressed == gameObject)
				{
					mTouch.current = clone;
					mTouch.pressed = clone;
					mTouch.dragged = clone;
					mTouch.last = clone;
				}
				//Change#1: Change Component name to current class.
//				UIDragDropItem item = clone.GetComponent<UIDragDropItem>();
				CustomDragDropItem item = clone.GetComponent<CustomDragDropItem>();
				item.mTouch = mTouch;
				item.mPressed = true;
				item.mDragging = true;
				item.Start();
				item.OnClone(gameObject);
				item.OnDragDropStart();

				if (UICamera.currentTouch == null)
					UICamera.currentTouch = mTouch;

				mTouch = null;

				UICamera.Notify(gameObject, "OnPress", false);
				UICamera.Notify(gameObject, "OnHover", false);
			}
			else
			{
				mDragging = true;
				OnDragDropStart();
			}
		}
	}

	protected override void OnDragStart ()
	{
		if (!interactable) return;
		if (!enabled || mTouch != UICamera.currentTouch) return;

		// If we have a restriction, check to see if its condition has been met first
		if (restriction != Restriction.None)
		{
			if (restriction == Restriction.Horizontal)
			{
				Vector2 delta = mTouch.totalDelta;
				if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)) return;
			}
			else if (restriction == Restriction.Vertical)
			{
				Vector2 delta = mTouch.totalDelta;
				if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y)) {
					DragScrollView.GetComponent<CustomDragScrollView> ().childDrag (delta);
					return;
				}
			}
			else if (restriction == Restriction.PressAndHold)
			{
				// Checked in Update instead
				return;
			}
		}
		StartDragging();
	}

	/// <summary>
	/// Drop the dragged item.
	/// </summary>

	override public void StopDragging (GameObject go = null)
	{
		if (go != null){
			Debug.Log ("GameObject name " + go.name);
			if (mDragging) {
				mDragging = false;
				OnDragDropRelease (go);
			}
		}
	}
	protected override void OnDragDropRelease (GameObject surface)
	{

		//Change#2: Allow for clone also.
//		if (!cloneOnDrag)
		if (true)
		{
			// Clear the reference to the scroll view since it might be in another scroll view now
			var drags = GetComponentsInChildren<UIDragScrollView>();
			foreach (var d in drags) d.scrollView = null;

			// Re-enable the collider
			if (mButton != null) mButton.isEnabled = true;
			else if (mCollider != null) mCollider.enabled = true;
			else if (mCollider2D != null) mCollider2D.enabled = true;

			// Is there a droppable container?
			UIDragDropContainer container = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;

			if (container != null)
			{
				// Container found -- parent this object to the container
				mTrans.parent = (container.reparentTarget != null) ? container.reparentTarget : container.transform;

				Vector3 pos = mTrans.localPosition;
				pos.z = 0f;
				mTrans.localPosition = pos;
			}
			else
			{
				// No valid container under the mouse -- revert the item's parent
				mTrans.parent = mParent;
				//Change: Delete the item
				NGUITools.Destroy(gameObject);
			}

			// Update the grid and table references
			mParent = mTrans.parent;
			mGrid = NGUITools.FindInParents<UIGrid>(mParent);
			mTable = NGUITools.FindInParents<UITable>(mParent);

			// Re-enable the drag scroll view script
			if (mDragScrollView != null)
				Invoke("EnableDragScrollView", 0.001f);

			// Notify the widgets that the parent has changed
			NGUITools.MarkParentAsChanged(gameObject);

			if (mTable != null) mTable.repositionNow = true;
			if (mGrid != null) mGrid.repositionNow = true;
		}
		else NGUITools.Destroy(gameObject);
		if (notifySurface (surface)) {
			Debug.Log ("Correct Answer");
			surface.GetComponent<UIDragDropContainer> ().enabled = false;
		} else {
			NGUITools.Destroy (gameObject);
			Debug.Log ("Incorrect Answer");
		}
		// We're now done
		OnDragDropEnd();
	}
	public bool notifySurface(GameObject surface){
		Debug.Log ("Name of surface"+surface.name);
		DropZoneItemChecker itemChecker = surface.GetComponent<DropZoneItemChecker> ();
		if (itemChecker != null) {
			Debug.Log ("notifySurface text "+this.gameObject.GetComponentInChildren<UILabel> ().text);
			Debug.Log ("surface name "+surface.name);
			if (itemChecker.checkDropZoneItem (this.gameObject.GetComponentInChildren<UILabel> ().text)) {
				return true;
			}
		}
		return false;
	}
}
