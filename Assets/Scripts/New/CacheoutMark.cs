using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class CacheoutMark : MonoBehaviour {
	[SerializeField] TMP_Text Amount = null;
	[SerializeField] TMP_Text Multiplier = null;

	public string amount { get => Amount.text; set => Amount.text = value; }
	public string mult { get => Multiplier.text; set => Multiplier.text = value; }
	public float amountValue;

	[SerializeField] Collider2D trigger = null;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.TryGetComponent(out CacheoutMark mark)) {
			if (amountValue - mark.amountValue < 0) {
				HideText();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		ShowText();
		trigger.enabled = false;
		trigger.enabled = true;
	}

	public void HideText() {
		Amount.enabled = false;
		Multiplier.enabled = false;
	}

	public void ShowText() {
		Amount.enabled = true;
		Multiplier.enabled = true;
	}
}