using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GraphGenerator : MonoBehaviour {
	public float GenScale;
	public float xOffset;
	public int GenPerTime;
	public float[] StepScales;
	public int SmallMarksCount = 5;
	[Header("Prefabs")]
	public MovablePoint movablePointPrefab;
	public GraphNamedMark MarkNamed;
	public GraphNamedMark MarkNamedSmall;
	public GameObject MarkSmall;
	// Kludge
	public GameObject scaleCheckMarkPrefab;

	[Header("Objects")]
	public RocketControl rc;
	public Transform graphMarksFolder;
	public Transform TopRight;

	// vars
	int points = 0;
	int currScaleIndex = 0;
	Transform folder;

	private void Start() {
		var _p = transform.position;
		_p.y = rc.startYGlob;
		transform.position = _p;
		ResetGraph();
	}

	void FixedUpdate() {
		var pos = transform.position;
		pos.x = TopRight.position.x + xOffset;
		transform.position = pos;
	}

	float _mult => rc.MultHeightScale * GenScale;

	Vector2 pos;
	public void GenerateGraph() {
		// scale check mark
		var _cm = Instantiate(scaleCheckMarkPrefab, folder, false);
		pos = Vector2.zero;
		pos.y = _mult * StepScales[currScaleIndex];
		_cm.transform.localPosition = pos;
		var _cm_c = _cm.GetComponent<CircleCollider2D>();
		_cm_c.radius = 0.5f / transform.localScale.y;
		_cm_c.enabled = true;

		for (int i = 0; i < points + GenPerTime; i++) {
			// big mark
			var m_p = Instantiate(movablePointPrefab, folder, false);
			pos.y = (i + points) * _mult * StepScales[currScaleIndex];
			m_p.transform.localPosition = pos;

			var mark = Instantiate(MarkNamed, graphMarksFolder);
			mark.text = ((points + i) * StepScales[currScaleIndex] + 1).ToString("F2") + "x";
			m_p.attachedObject = mark.gameObject;

			// mid mark
			var m_p1 = Instantiate(movablePointPrefab, folder, false);
			pos.y = (i + points + .5f) * _mult * StepScales[currScaleIndex];
			m_p1.transform.localPosition = pos;
			m_p1.transform.SetParent(m_p.transform, true);

			var mark1 = Instantiate(MarkNamedSmall, graphMarksFolder);
			mark1.text = ((points + i + 0.5) * StepScales[currScaleIndex] + 1).ToString("F2") + "x";
			m_p1.attachedObject = mark1.gameObject;
			mark1.transform.SetParent(mark.transform, true);

			// small marks
			for (int j = 0; j < 2; j++) {
				for (int k = 1; k < SmallMarksCount; k++) {

					var p = Instantiate(movablePointPrefab, folder, false);
					pos.y = (i + points + (k * 0.5f / SmallMarksCount) + 0.5f * j) * _mult * StepScales[currScaleIndex];
					p.transform.localPosition = pos;
					p.transform.SetParent(m_p.transform, true);

					var mark2 = Instantiate(MarkSmall, graphMarksFolder);
					p.attachedObject = mark2.gameObject;
					mark2.transform.SetParent(mark.transform, true);
				}
			}
		}
		points += points + GenPerTime;
	}

	public void UpscaleGraph() {
		if (currScaleIndex + 1 >= StepScales.Length) return;
		ResetGraph();
		currScaleIndex++;
		GenerateGraph();
	}

	public void CheckGen() {
		//generating new
		if (folder.childCount > 0) {
			if (folder.GetChild(folder.childCount - 1).position.y <= TopRight.position.y) GenerateGraph();
		} else {
			GenerateGraph();
		}
	}

	public void ResetGraph() {
		points = 0;
		currScaleIndex = 0;

		if (folder != null) Destroy(folder.gameObject);
		folder = new GameObject().transform;
		folder.SetParent(transform, false);
		folder.localPosition = Vector3.zero;
		folder.localScale = Vector3.one;

		//GenerateGraph();
	}
}
