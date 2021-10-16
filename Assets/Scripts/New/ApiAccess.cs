using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
// change to new Unity HTTP requests only if you know how to handle them right
public class ApiAccess {
	//static readonly HttpClient client = new HttpClient();
	static readonly string baseUrl = "https://vub.gla.mybluehost.me/api/users";
	static string reqUrl(string requestSubUrl) => $"{baseUrl}/{requestSubUrl}";

	public static string auth_token = "";

	//public static Action<UnityWebRequest> onResponseGet = delegate { };
	public static Action<WWW> onResponseGetWWW = delegate { };

	public static IEnumerator Register(string name, string email, string password, int securityCode = 123456) {
		var json = $"{{\n    \"name\":\"{name}\",\n    \"email\":\"{email}\",\n    \"password\":\"{password}\",\n    \"securityCode\":\"{securityCode}\"\n}}";
		/*
		Debug.Log(json);
		var request = UnityWebRequest.Post(reqUrl("register"), json);
		//request.SetRequestHeader("Content-Type", "application/json");
		Debug.Log(request.GetRequestHeader("Content-Type"));
		yield return request.SendWebRequest();
		onResponseGet(request);
		*/
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
		var data = Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(reqUrl("register"), data, headers);
		yield return www;
		onResponseGetWWW(www);
	}

	public static IEnumerator Login(string email, string password) {
		var json = $"{{\n    \"email\":\"{email}\",\n    \"password\":\"{password}\"\n}}";
		/*
		var request = UnityWebRequest.Post(reqUrl("login"), json);
		//request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		onResponseGet(request);
		*/
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
		var data = Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(reqUrl("login"), data, headers);
		yield return www;
		onResponseGetWWW(www);
	}
}
#pragma warning restore 0618

