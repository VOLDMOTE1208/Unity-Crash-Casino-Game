using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePoint : MonoBehaviour {
    public GameObject attachedObject;

    void Update() {
        if (attachedObject != null) {
            var pos = attachedObject.transform.position;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            attachedObject.transform.position = pos;
        }
    }

    public void Destroy() {
        Destroy(attachedObject);
        Destroy(gameObject);
	}
	private void OnDestroy() {
        Destroy(attachedObject);
    }
}
