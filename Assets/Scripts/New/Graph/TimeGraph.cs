using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGraph : MonoBehaviour {
	[Header("Prefabs")]
	public MovablePoint pointPrefab;
	public GraphNamedMark markPrefab;
	public GraphNamedMark smallMarkPrefab;

	[Header("Scene objects")]
	public Transform marksFolder;
	public RocketControl rc;

	[Header("Vars")]
	public int GenPerTime;
	public float GenScale;

	//
	Transform folder;

	private void Awake() {
		ResetGraph();
	}

	int points = 0;
	public void GenerateGraph(float period = 1f) {
		for (int i = 0; i < GenPerTime; i++) {
			// big mark
			var p = Instantiate(pointPrefab, folder);
			var pos = Vector3.zero;
			pos.x = (points + i) * rc.horisontalSpeed * period * GenScale;
			p.transform.localPosition = pos;

			var m = Instantiate(markPrefab, marksFolder);
			m.text = ((points + i) * period).ToString() + " s";
			p.attachedObject = m.gameObject;

			// small mark
			var p1 = Instantiate(pointPrefab, folder);
			pos.x = (points + i + 0.5f) * rc.horisontalSpeed * period * GenScale;
			p1.transform.localPosition = pos;

			var m1 = Instantiate(smallMarkPrefab, marksFolder);
			m1.text = ((points + i + 0.5f) * period).ToString("G") + " s";
			p1.attachedObject = m1.gameObject;
		}
		points += GenPerTime;
	}

	public void ResetGraph() {
		if (folder != null)
			Destroy(folder.gameObject);
		folder = new GameObject("Folder ").transform;
		folder.SetParent(transform, false);
		folder.localPosition = Vector3.zero;
		folder.localScale = Vector3.one;
		points = 0;
	}
}