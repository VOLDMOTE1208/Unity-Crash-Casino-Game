using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextDisapear : MonoBehaviour
{
	private TextMeshProUGUI tmPro;
	 float alpthSpeed = 0.3f;
	private float alpth = 1;


	private void OnEnable()
	{
		tmPro = GetComponent<TextMeshProUGUI>();
		DisplaySplath();
		
	}
	


	public void DisplaySplath()
	{
		StartCoroutine(UpdateSplath());
	}

	IEnumerator UpdateSplath()
	{
		alpth = 1;

		while (alpth > 0)
		{
			alpth -= Time.deltaTime * alpthSpeed;

			tmPro.color  = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, alpth);
			if (alpth <= 0)
				gameObject.SetActive(false);

			yield return new WaitForEndOfFrame();
		}
		tmPro.text = "";
	}
}
