using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphNamedMark : MonoBehaviour {
	[SerializeField] TMP_Text _textObj = null;
	public string text { get => _textObj.text; set => _textObj.text = value; }
}
