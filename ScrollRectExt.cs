using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectExt : MonoBehaviour {

	public ScrollRect scrollRect;
	protected RectTransform scrollRectTransform;

	protected bool horizontal;
	protected bool vertical;

	[Header("Update events")]
	public bool onUpdate;

	void Awake () {
		if (scrollRect == null)
			scrollRect = GetComponent<ScrollRect>();

		if (scrollRect == null) {
			Destroy(this);
			return;
		}

		if (scrollRect != null)
			scrollRectTransform = scrollRect.GetComponent<RectTransform>();
		
		vertical = scrollRect.vertical;
		horizontal = scrollRect.horizontal;
	}

	void OnEnable () {
		UpdateScrollable();
	}

	void Update () {
		if (onUpdate)
			UpdateScrollable();
	}

	void UpdateScrollable () {
		if (vertical) {
			var contentHeight = scrollRect.content.sizeDelta.y;
			var height = scrollRectTransform.sizeDelta.y;
			var scrollable = contentHeight - height > 2;
			scrollRect.vertical = scrollable;
		}

		if (horizontal) {
			var contentWidth = scrollRect.content.sizeDelta.x;
			var width = scrollRectTransform.sizeDelta.x;
			var scrollable = contentWidth - width > 2;
			scrollRect.horizontal = scrollable;
		}
	}
}
