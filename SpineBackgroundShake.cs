using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpineBackgroundShake : MonoBehaviour 
{
	[SerializeField]
	Vector2 shakeVariant;

	[SerializeField]
	float shakeDuration;

	[SerializeField]
	float shakePerSecond;

	[SerializeField, Range(1, 100)]
	float smoothness;

	float _shakeDuration;
	float _timeToNextShake;
	float _timePerShake;
	float _lastUpdateTime;
	float _timePerUpdate;
	Vector3 _oldPosition;
	Vector3 _lastShakePosition;
	Vector3 _nextShakePosition;

	[ContextMenu("Shake")]
	public void Shake()
	{
		_oldPosition = transform.localPosition;
		_shakeDuration = shakeDuration;
		_timePerShake = 1.0f / shakePerSecond;
		_timeToNextShake = _timePerShake;
		_lastUpdateTime = _timeToNextShake;
		_timePerUpdate = _timePerShake / smoothness;
		_lastShakePosition = _oldPosition;
		RandomNextShakePosition();
	}


    // s = 0 => update at _timePerShake
    // s = 1 => update every frame
    // s = 0.5 => update at _timePerShake / 2 and _timePerShake

    void Update()
	{
		if (_shakeDuration > 0)
		{
			_shakeDuration -= Time.deltaTime;
			_timeToNextShake -= Time.deltaTime;

			if (_lastUpdateTime - _timeToNextShake >= _timePerUpdate)
			{
				_lastUpdateTime = _timeToNextShake;

				float lerpT = Mathf.Min(1, (_timePerShake - _timeToNextShake) / _timePerShake);
                Vector3 newPosition = new Vector3();
                newPosition.x = Mathf.Lerp(_lastShakePosition.x, _nextShakePosition.x, lerpT);
                newPosition.y = Mathf.Lerp(_lastShakePosition.y, _nextShakePosition.y, lerpT);
                transform.localPosition = newPosition;
			}
			

			if (_timeToNextShake <= 0)
			{
				_timeToNextShake = _timePerShake;
				_lastUpdateTime = _timeToNextShake;
				_lastShakePosition = _nextShakePosition;
				RandomNextShakePosition();
			}

			// Shake duration has ended
			if (_shakeDuration <= 0)
			{
				transform.localPosition = _oldPosition;
			}
		}
	}

	void RandomNextShakePosition()
	{
		// Last shake
		if (_shakeDuration <= _timePerShake)
		{
			_nextShakePosition = _oldPosition;
		}
		else
		{
			_nextShakePosition.x = Random.Range(-shakeVariant.x, shakeVariant.x) + _oldPosition.x;
            _nextShakePosition.y = Random.Range(-shakeVariant.y, shakeVariant.y) + _oldPosition.y;
            _nextShakePosition.z = _oldPosition.z;	
		}
	}
}
