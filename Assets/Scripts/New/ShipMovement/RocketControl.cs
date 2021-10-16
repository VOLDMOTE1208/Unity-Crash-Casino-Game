using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControl : MonoBehaviour {
    public float horisontalSpeed;
    public float ChangeTime;
    public float MultHeightScale;

    Vector3 startPos;
    Vector3 startPosGlob;
	private void Awake() {
        startPos = transform.localPosition;
        startPosGlob = transform.position;
    }

    public float startY => startPos.y;
    public float startYGlob => startPosGlob.y;

    IEnumerator changeHeight(float val) {
        float dy = val / ChangeTime;
		float t = Time.time;
        while (t + ChangeTime > Time.time) {
            transform.localPosition += (Vector3)Vector2.up * dy * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition += (Vector3)Vector2.up * val * Time.fixedDeltaTime;
    }

    public void MoveByVal(float val) {
        float dy = val * MultHeightScale;
        //Debug.Log($"{transform.localPosition.y}, {val * MultHeightScale}, {dy}");
        StartCoroutine(changeHeight(dy));
    }

    public void MoveWithDtInstant(float dy) {
        Vector2 add = new Vector2(horisontalSpeed, dy);
        transform.localPosition += (Vector3)add;
	}

    bool isMoving;
    void FixedUpdate() {
		if (isMoving) {
            moveSelf();
		}
    }
    void moveSelf() {
        transform.localPosition += (Vector3)Vector2.right * horisontalSpeed * Time.fixedDeltaTime;
    }

    public void startMoving() {
        isMoving = true;
    }
    public void stopMoving() {
        isMoving = false;
        StopAllCoroutines();
    }
    
    public void ResetValues() {
        StopAllCoroutines();
        transform.localPosition = startPos;
	}
}
