using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketPoint : MonoBehaviour {
    public Vector2 Speed;
    public Vector2 AccelerationRange;
    float verticalAcceleration;
    Vector3 _speed;

    // Start is called before the first frame update
    void Start() {
        ResetValues();
        startMoving();
    }

    Action _upd = delegate { };
    // Update is called once per frame
    void Update() {
        _upd();
    }
    void moveSelf() {
        transform.localPosition += _speed * Time.deltaTime;
        _speed.y *= 1 + verticalAcceleration * Time.deltaTime;
    }

    void startMoving() {
        _upd += moveSelf;
	}
    void stopMoving() {
        _upd -= moveSelf;
    }

    public void ResetValues() {
        _speed = Speed;
        verticalAcceleration = Random.Range(AccelerationRange.x, AccelerationRange.y);
    }
}
