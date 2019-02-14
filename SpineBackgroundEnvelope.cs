using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

[ExecuteInEditMode]
public class SpineBackgroundEnvelope : MonoBehaviour 
{
	[SerializeField]
	Vector2 spineSize;
	[SerializeField]
	float scaleFactor;

	const float UNIT_TO_PIXEL = 100.0f;

	void OnEnable()
	{
		AdjustScaleToFitScreen();
	}

	#if UNITY_EDITOR
	int lastWidth;
	int lastHeight;
	void Update()
	{
		if (lastWidth != Camera.main.pixelWidth || lastHeight != Camera.main.pixelHeight)
		{
			lastWidth = Camera.main.pixelWidth;
			lastHeight = Camera.main.pixelHeight;
			AdjustScaleToFitScreen();
		}
	}
	#endif

	void AdjustScaleToFitScreen()
	{
		float screenRatio = (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight;
        float realScreenWidth = screenRatio * Camera.main.orthographicSize * 2 * UNIT_TO_PIXEL;
        float realScreenHeight = Camera.main.orthographicSize * 2 * UNIT_TO_PIXEL;
        float scaleX = realScreenWidth / spineSize.x;
        float scaleY = realScreenHeight / spineSize.y;
        float preferScale = Mathf.Max(scaleX, scaleY) * scaleFactor;
        transform.localScale = new Vector3(preferScale, preferScale, 1.0f);
	}
}
