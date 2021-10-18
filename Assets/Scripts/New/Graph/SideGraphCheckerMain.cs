using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideGraphCheckerMain : MonoBehaviour {
	public GraphGenerator graph;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "CheckMark") {
			graph.UpscaleGraph();
		} 
	}
}
