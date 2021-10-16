using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserinfoText : MonoBehaviour {
	[SerializeField] TMP_Text UsernameText = null;
	[SerializeField] TMP_Text UserBidText = null;
	[SerializeField] Image img = null;
	public string username { get => UsernameText.text; set => UsernameText.text = value; }
	public string userBid {
		get => UserBidText.text;
		set {
			UserBidText.text = value;
		}
	}
	public RectTransform rectTransform {
		get {
			if (_rect == null) {
				_rect = GetComponent<RectTransform>();
			}
			return _rect;
		}
	}
	RectTransform _rect;

	public Color EvenColor;
	public Color OddColor;

	bool _isEven;
	public bool IsEven {
		get => _isEven;
		set {
			_isEven = value;
			if (value) {
				img.color = EvenColor;
			} else {
				img.color = OddColor;
			}

		}
	}
}
