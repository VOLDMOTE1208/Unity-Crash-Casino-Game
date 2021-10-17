using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;
using UnityEngine.Video;
using UnitySocketIO;
using UnitySocketIO.Events;
using LitJson;
using UnityEngine.SceneManagement;
#pragma warning disable 0618
public class UserData {
    #region ***** DATA *****

    //   public  string device_token;
    public string username;
    public string email_address;
    public string name;
    public string password;
    public string verification_code;
    public string balance;
    public string level;

    #endregion
}

public class FcmData : MonoBehaviour {
    #region Data Fields
    UserData userData = new UserData();
    //  UserLogin loginData;

    // private string url = "http://4zeetraders.com/roobet_backend/";

    //  private string myToken = null;

    //  private string verificationCode = null;

    [Header("Login")]
    public TMP_InputField login_Email_usernameText;
    public TMP_InputField login_passwordText;

    [Header("Signup")]
    public TMP_InputField amountText;
    public TMP_InputField betAmount;
    public TMP_InputField setAutoCashOut;
    public TMP_InputField signup_emailText;
    public TMP_InputField signup_nameText;
    public TMP_InputField signup_passwordText;
    public TMP_InputField signup_confirmPasswordText;
    public TMP_InputField change_passwordText;
    public TMP_InputField change_confirmPasswordText;

    [Header("Forget password")]
    public TMP_InputField forgetPass_usernameText;
    public GameObject forgetPass_panel;

    [Header("Verify Code")]
    public TMP_InputField verifyCode_Text;
    public GameObject verifyCode_panel;

    [Header("Panel")]
    public GameObject registerGO;
    public GameObject welcomePanel;
    public GameObject changePassPanel;
    public SwitchPanel sp_Script;


    [Header("Texts")]
    //public TMP_Text errorText;
    public TMP_Text info_errorText;
    public TMP_Text welcomeText;
    public TMP_Text tot_BidsText;
    public TMP_Text tot_UsersText;
    public UserinfoText userText;
    public TMP_Text fcmText;

    public TMP_Text payoutText;
    public TMP_Text roundOverText;

    [Header("GameObjects")]
    //GameObjects
    public GameControl gameControl;

    public GameObject mover;
    public GameObject video;

    public VideoPlayer videoPlayer;

    public RectTransform contentPanel;

    public GameObject loginPanel;
    public GameObject signUpPanel;

    [Header("Input Field bet and cash")]
    //Input Field bet and cash
    public TMP_InputField betText;
    public TMP_InputField cashOutText;

    //Data Arrays

    //Users Bids
    // [SerializeField]
    // string[] online_Users;
    string[] users_Bids_Username;
    string[] users_Bids_Amount;
    public SocketIOController io;

    //Bool 
    //private bool signedIn = false;
    private bool roomJoined = false;
    #endregion
    public int authMode = 0;
    private void Start() {
        //login_Email_usernameText.text=PlayerPrefs.GetString("_PlayerEmail");
        //login_passwordText.text= PlayerPrefs.GetString("_PlayerPass");
        amountText.text = "100";
        PlayVideo();
		registerGO.SetActive(true);
        verifyCode_panel.SetActive(false);
        forgetPass_panel.SetActive(false);
        welcomePanel.SetActive(false);
        GetOnlineUsersBid();

        io = SocketIOController.instance;
        io.On("connect", (e) => {
            Debug.Log("Game started");
            io.On("user list", (res) => {
                SetResponse(res);
            });
        });
        io.Connect();
    }

    void SetResponse(SocketIOEvent socketIOEvent) {
        var res = ReceiveJsonObject.CreateFromJSON(socketIOEvent.data);
        JSONNode _data = new JSONObject();
        _data["users_req_bids"] = new JSONArray();
        _data["users_bids"] = new JSONArray();
        _data["bids_req_sum"] = "10";
        _data["bids_sum"] = "20";

        _data["users_bids"][0] = new JSONObject();
        _data["users_bids"][0]["username"] = res.userName;
        _data["users_bids"][0]["amount"] = res.betAmount;
        api_get_bids_res(_data.ToString());
    }

    public void SendBetInfo() {
        JsonType JObject = new JsonType();
        float myTotalAmount = float.Parse(amountText.text);
        float betamount = float.Parse(betAmount.text);
        if (betamount < myTotalAmount) {
            JObject.betAmount = betAmount.text;
            JObject.autoCashAmount = setAutoCashOut.text;
            amountText.text = (myTotalAmount - betamount).ToString();
            JObject.userName = PlayerPrefs.GetString("_UserName");
            io.Emit("bet amount", JsonUtility.ToJson(JObject));
        } else
            info_errorText.text = "Not enough Funds";        
    }

    #region ****** EXTRAS *******
    Coroutine vidCor;
    public void PlayVideo() {
        //video.SetActive(true);
        //videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "scene rocket.mp4");

        //IEnumerator _cor() {
        //    yield return new WaitForEndOfFrame();
        //    videoPlayer.Play();
        //}
        //vidCor = StartCoroutine(_cor());
    }

    public void StopVideo() {
        //if (vidCor != null) StopCoroutine(vidCor);
        //videoPlayer.Stop();
        //video.SetActive(false);
    }

    void ChangeText(string msg, bool isRed) {
        info_errorText.gameObject.SetActive(true);
        if (!isRed) {
            info_errorText.color = Color.white;
            info_errorText.text = msg;
        } else {
            info_errorText.color = Color.red;
            info_errorText.text = msg;
        }
    }
    #endregion
    IEnumerator iRequest(UnityWebRequest www) {
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Network Error." + www.error);
            yield break;
        }        
        string resultData = www.downloadHandler.text;

        if (string.IsNullOrEmpty(resultData)) {
            Debug.Log("Result Data Empty");
            info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Failed");
            yield break;
        }
        Debug.Log(resultData);
        info_errorText.text = "";
        JsonData json = JsonMapper.ToObject(resultData);
        string response = json["success"].ToString();

        if (response != "1") {
            if (authMode == 0) {
                if (response == "-1") {
                    info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Sign Up Failed");
                } else if (response == "0") {
                    info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("UserName already exists.");
                }
            } 
            else if (authMode == 1) {
                if (response == "-1") {
                    info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Login Failed");
                } else if (response == "0") {
                    info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Password Wrong.");
                }
            }            
        } 
        else if(response=="1") {
            if (authMode == 0) {                
                signUpPanel.SetActive(false);
                loginPanel.SetActive(true);
                info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Sign Up Success! Please log in.");
            }                
            if (authMode == 1) {
                verifyCode_panel.SetActive(false);
                registerGO.SetActive(false);
                welcomePanel.SetActive(true);

                StopVideo();
                info_errorText.transform.GetComponent<TextMeshProUGUI>().SetText("Login Success.");
            }                
        }
        

    }

    #region*********** Sign UP ***********
    public void SignUp() {
        info_errorText.text = "";
        authMode = 0;
		if (signup_passwordText.text != signup_confirmPasswordText.text) {
            info_errorText.text = "Passwords do not match";
        } else if (signup_nameText.text != "" && signup_emailText.text != "" && signup_passwordText.text != "" && signup_confirmPasswordText.text != "") {            
            info_errorText.text = "";
			userData = new UserData();
            userData.username = signup_nameText.text;
            userData.email_address = signup_emailText.text;
            userData.password = signup_passwordText.text;

            PlayerPrefs.SetString("_UserName", userData.username);
            PlayerPrefs.SetString("_PlayerEmail", userData.email_address);
            PlayerPrefs.SetString("_PlayerPass", userData.password);

            WWWForm formData = new WWWForm();
            formData.AddField("username", userData.username);
            formData.AddField("email_address", userData.email_address);
            formData.AddField("password", userData.password);

            string requestURL = Global.GetDomain() + "/api/signup";

            UnityWebRequest www = UnityWebRequest.Post(requestURL, formData);
            Debug.Log(Global.GetDomain());
            www.SetRequestHeader("Accept", "application/json");
            www.uploadHandler.contentType = "application/json";
            StartCoroutine(iRequest(www));
        }
    }

    public void api_signup_res(string ResponseData) { // unused
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;

        if (code == 1) {
            ChangeText(msg, false);
            sp_Script.SwitchLogin();
            login_Email_usernameText.text = userData.username;
            login_passwordText.text = userData.password;
            //  verifyCode_panel.SetActive(true);
        } else {
            ChangeText(msg, true);
        }
    }
    #endregion


    #region*********** Log IN ***********
    public void user_logged_in(string Response) {
        JSONNode userInfo = JSON.Parse(Response);
        var verify = Int32.Parse(userInfo["user_info"]["is_verified"].Value);
        userData.username = userInfo["user_info"]["username"].Value;
        userData.balance = userInfo["user_info"]["balance"].Value;
        userData.level = userInfo["user_info"]["level"].Value;

        if (verify == 1) {
            registerGO.SetActive(false);
            welcomePanel.SetActive(true);
            welcomeText.text = "Welcome \"" + userData.username + "\" \nBalance: " + userData.balance + "\nLevel: " + userData.level;

            StopVideo();
        }
    }

    // todo: rework login and register api
    public void Login() {

        info_errorText.text = "";
        authMode = 1;
        if (login_Email_usernameText.text != "" && login_passwordText.text != "") {
            info_errorText.text = "";
            userData = new UserData();
            userData.email_address = login_Email_usernameText.text;
            userData.password = login_passwordText.text;

            WWWForm formData = new WWWForm();
            formData.AddField("email_address", userData.email_address);
            formData.AddField("password", userData.password);

            string requestURL = Global.GetDomain() + "/api/login";

            UnityWebRequest www = UnityWebRequest.Post(requestURL, formData);
            Debug.Log(Global.GetDomain());
            www.SetRequestHeader("Accept", "application/json");
            www.uploadHandler.contentType = "application/json";
            StartCoroutine(iRequest(www));

        } else {
            info_errorText.text = "Enter email or password";
        }
    }

    public void api_signin_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo["code"]);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;
        var verify = Int32.Parse(userInfo["user_info"]["is_verified"].Value);
        var userName = userInfo["user_info"]["username"].Value;
        var balance = userInfo["user_info"]["balance"].Value;
        var level = userInfo["user_info"]["level"].Value;

        if (code == 1) {
            ChangeText(msg, false);

            if (verify == 0) {
                verifyCode_panel.SetActive(true);
            } else {
                verifyCode_panel.SetActive(false);
                registerGO.SetActive(false);
                welcomePanel.SetActive(true);
                welcomeText.text = "Welcome \"" + userName + "\" \nBalance: " + balance + "\nLevel: " + level;

                StopVideo();
            }
        } else {
            ChangeText(msg, true);
        }
    }
    #endregion


    #region ******** Log OUT *********
    public void LogOut() {
        Application.ExternalCall("api_logout");
    }

    public void api_logout_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;

        if (code == 1) {
            ChangeText(msg, false);
            registerGO.SetActive(true);
            welcomePanel.SetActive(false);
            //signedIn = false;
        } else {
            ChangeText(msg, true);
        }
    }
    #endregion


    #region*********** Forget/Change Password ***********

    public void ForgetPassword() {

        StartCoroutine(ForgetPasswordApi());
    }

    IEnumerator ForgetPasswordApi() {

        if (forgetPass_usernameText.text != "") {
            userData = new UserData();
            userData.username = forgetPass_usernameText.text;

            Application.ExternalCall("api_forget_password", userData.username);


        } else {
            info_errorText.color = Color.red;
            info_errorText.text = "Kindly Re-enter Correct info";
        }
        yield return null;
    }

    public void api_forget_password_res(string ResponseData) {

        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;

        if (code == 1) {
            ChangeText(msg, false);
            // verifyCode_panel.SetActive(true);
            forgetPass_panel.SetActive(false);
            login_Email_usernameText.text = userData.username;

        } else {
            ChangeText(msg, true);
        }
    }



    public void ChangePassword() {

        StartCoroutine(ChangePasswordApi());
    }

    IEnumerator ChangePasswordApi() {

        if (change_passwordText.text != "" && change_confirmPasswordText.text != "" && change_passwordText.text == change_confirmPasswordText.text) {
            userData = new UserData();
            userData.password = change_passwordText.text;

            Application.ExternalCall("api_change_password", userData.password);


        } else {
            info_errorText.color = Color.red;
            info_errorText.text = "Kindly Re-enter Correct info";
        }
        yield return null;
    }

    public void api_change_password_res(string ResponseData) {

        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;

        if (code == 1) {
            ChangeText(msg, false);
            // verifyCode_panel.SetActive(true);


            ////////////////////////////////////// ADD NEW PASSWORD CODE
            changePassPanel.SetActive(false);
            // login_usernameText.text = userData.username;

        } else {
            ChangeText(msg, true);
        }
    }

    #endregion


    #region **********  CODE VERIFICATION ***********
    public void VerifyCode() {
        if (verifyCode_Text.text != "") {
            userData = new UserData();
            userData.verification_code = verifyCode_Text.text;
            Application.ExternalCall("api_verify_code", userData.verification_code);
        } else {
            info_errorText.color = Color.red;
            info_errorText.text = "Kindly Re-enter Correct info";
        }
    }

    public void api_verify_code_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;

        if (code == 1) {
            ChangeText(msg, false);
            verifyCode_panel.SetActive(false);
            registerGO.SetActive(false);
            welcomePanel.SetActive(true);
            welcomeText.text = "Welcome \"" + userData.username + "\"";
        } else {
            ChangeText(msg, true);
        }
    }

    public void ResendVerifyCode() {
        Application.ExternalCall("api_resend_code");
    }

    public void api_resend_code_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        Debug.Log(userInfo.Value);
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;
        if (code == 1) {
            ChangeText(msg, false);
        } else {
            ChangeText(msg, true);
        }
    }
    #endregion

    #region ******** Get Online Users Bid ********
    public void GetOnlineUsersBid() {
        Application.ExternalCall("api_get_bids");
    }

    public void api_get_bids_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        //Debug.Log(userInfo.Value);
        var length = userInfo["users_req_bids"].Count + userInfo["users_bids"].Count;
        users_Bids_Username = new string[length];
        users_Bids_Amount = new string[length];
        var totalSum = int.Parse(userInfo["bids_req_sum"].Value) + int.Parse(userInfo["bids_sum"].Value);
        tot_BidsText.text = "$" + totalSum.ToString();
        tot_UsersText.text = length + " Players";

        if (contentPanel.childCount != 0) {
            for (int i = 0; i < contentPanel.childCount; i++) {
                Destroy(contentPanel.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < userInfo["users_req_bids"].Count; i++) {
            users_Bids_Username[i] = userInfo["users_req_bids"][i]["username"].Value;
            users_Bids_Amount[i] = userInfo["users_req_bids"][i]["amount"].Value;
            //  var bidreqSum = Int32.Parse(userInfo["bids_req_sum"].Value);
            var text = Instantiate(userText, contentPanel.transform);
            text.username = users_Bids_Username[i];
            text.userBid = users_Bids_Amount[i];
            text.IsEven = i % 2 == 0;
        }

        for (int i = 0; i < userInfo["users_bids"].Count; i++) {
            users_Bids_Username[i] = userInfo["users_bids"][i]["username"].Value;
            users_Bids_Amount[i] = userInfo["users_bids"][i]["amount"].Value;
            //  var bidreqSum = Int32.Parse(userInfo["bids_req_sum"].Value);
            var text = Instantiate(userText, contentPanel.transform);
            text.username = users_Bids_Username[i];
            text.userBid = users_Bids_Amount[i];
            text.IsEven = userInfo["users_req_bids"].Count + i % 2 == 0;
        }

        var cLayout = contentPanel.GetComponent<VerticalLayoutGroup>();
        int height = (int)userText.rectTransform.rect.height * userInfo["users_bids"].Count;
        int minHeight = (int)contentPanel.GetComponentInParent<RectTransform>().rect.height;
        if (height > minHeight) {
            cLayout.padding.bottom = height;
		} else {
            cLayout.padding.bottom = minHeight;
        }
    }
    #endregion


    #region ********* ROOM APIS ********
    public void LeaveRoom() {
        Application.ExternalCall("api_leave_room");
    }

    public void api_leave_room_res(string ResponseData) {
        if (roomJoined) {
            JSONNode userInfo = JSON.Parse((ResponseData).ToString());
            Debug.Log(userInfo.Value);
            var code = Int32.Parse(userInfo["code"].Value);
            var msg = userInfo["msg"].Value;
            if (code == 1) {
                ChangeText(msg, false);
            } else {
                ChangeText(msg, true);
            }
        } else {
            info_errorText.color = Color.red;
            info_errorText.text = "Room Not Joined";
        }
    }

    private void OnApplicationQuit() {
        LeaveRoom();
    }

    public void JoinRoom() {
        if (betText.text != "") {
            Application.ExternalCall("api_room_join_req", betText.text);
        }
    }

    public void api_room_join_req_res(string ResponseData) {
        JSONNode userInfo = JSON.Parse((ResponseData).ToString());
        var code = Int32.Parse(userInfo["code"].Value);
        var msg = userInfo["msg"].Value;
        if (code == 1) {
            ChangeText(msg, false);
        } else {
            ChangeText(msg, true);
        }
    }
    #endregion


    #region ******** < FCM RESPONSES > ********
    public void room_info(string Response) {
        JSONNode half_info = JSON.Parse(Response);
        JSONNode info = JSON.Parse(half_info["values"].Value);

        RoomData data = new RoomData();

		data.isEnded = int.Parse(info["room_info"]["is_ended"].Value);
		data.multipliyer = float.Parse(info["room_info"]["multiplier_per"].Value);
        data.id = info["room_info"]["id"].Value;
        data.startTimeString = info["room_info"]["start_time"].Value;
        data.currentTime = info["room_info"]["current_time"].Value;
        if (data.isEnded == 0) {
            payoutText.color = Color.white;
            roundOverText.color = Color.white;
            roundOverText.text = "Current Payout";
        }

        payoutText.text = $"{data.multipliyer.ToString("F2")}x";

        var length = info["room_users_info"].Count;
        data.users = new List<RoomUserData>(length);

        for (int i = 0; i < length; i++) {
            RoomUserData user = new RoomUserData();
            user.Amount = info["room_users_info"][i]["amount"].Value;
            //user.Name = info["room_users_info"][i]["name"].Value;
            user.LeaveTime = info["room_users_info"][i]["leave_time"].Value;
            user.multiplier_level = info["room_users_info"][i]["multiplier_level"].Value;

            data.users.Add(user);
        }

        gameControl.ProcessRoomData(data);
    }

    public void room_joined(string Response) {
        GetOnlineUsersBid();
        // fcmText.text = Response;
        JSONNode info = JSON.Parse(Response);
        var msg = info["body"].Value;
        ChangeText(msg, false);
        roomJoined = true;
        //mover.SetActive(true);
        gameControl.StartGame();
        StopVideo();
    }

    public void bet_lost(string Response) {
        // GetOnlineUsersBid();
        fcmText.text = Response;
        JSONNode info = JSON.Parse(Response);
        Debug.Log(info);
        var msg = info["body"].Value;
        ChangeText(msg, false);
        gameControl.EndGame();
		StartCoroutine(BetTime());

		payoutText.color = Color.red;
        roundOverText.color = Color.red;
        roundOverText.text = "Round Over";
    }

    IEnumerator BetTime() {
        yield return new WaitForSeconds(2f);
        PlayVideo();
        payoutText.color = Color.white;
        roundOverText.color = Color.white;
        payoutText.text = "Bet time";
        roundOverText.text = "";
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(2f);

        payoutText.color = Color.white;
        roundOverText.color = Color.white;
        roundOverText.text = "Current Payout";
        gameControl.StartGame();
    }
    #endregion


    public void _startGame() {
        gameControl.StartGame();
    }
}

#pragma warning restore 0618