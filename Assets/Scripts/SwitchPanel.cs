using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchPanel : MonoBehaviour {
    public Button loginButton;
    public Button signUpButton;
    public GameObject loginPanel;
    public GameObject signUpPanel;

    [SerializeField] TMP_Text loginText = null;
    [SerializeField] TMP_Text signinText = null;
    void Start() {
        SwitchLogin();
    }

    public Color SelectedTabTextColor;
    public Color UnselectedTabTextColor;

    public void SwitchLogin() {
        loginButton.interactable = false;
        signUpButton.interactable = true;
        loginText.color = SelectedTabTextColor;
        signinText.color = UnselectedTabTextColor;
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }
    public void SwitchSignUp() {
        loginButton.interactable = true;
        signUpButton.interactable = false;
        loginText.color = UnselectedTabTextColor;
        signinText.color = SelectedTabTextColor;
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
}
