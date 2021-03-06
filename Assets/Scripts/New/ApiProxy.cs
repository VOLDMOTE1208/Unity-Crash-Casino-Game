using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ApiProxy : MonoBehaviour {
	public FcmData ApiManager;

	public float minTime;
	public float maxTime;
	private void Start() {
        //StartCoroutine(play());
        //try
        //{
        //    StartCoroutine(play());
        //}
        //catch (Exception)
        //{
        //    StartCoroutine(play());
        //    throw;
        //}
	}

	IEnumerator play() {
		yield return new WaitForEndOfFrame();
		StartCoroutine(RoomData(0));
	}

	IEnumerator RoomData(float delay = 0) {
		yield return new WaitForSeconds(0.5f);
		Random.InitState(DateTime.Now.Second * DateTime.Now.Millisecond);
		if (delay == 0) {
			ApiManager._startGame();
		}
		JSONNode joined = new JSONObject();
		joined["body"] = "Game start";

		ApiManager.room_joined(joined.ToString());

		float startTime = Time.time - delay;
		float dur = Random.Range(minTime, maxTime);
		Random.InitState((int)(DateTime.Now.Second * DateTime.Now.Millisecond * 1.2f));
		float mul = 1 - delay * 0.5f;

		JSONNode _data = new JSONObject();
		JSONNode _d0 = new JSONObject();
		JSONNode users = new JSONArray();
		_d0["room_info"]["start_time"] = (DateTime.Now + TimeSpan.FromSeconds(delay)).ToString("MM-dd-yyyy hh:mm:ss.fff");

		while (startTime + dur > Time.time) {

			var curTime = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss.fff");

			_d0["room_info"]["is_ended"] = "0";
			_d0["room_info"]["id"] = "1";
			_d0["room_info"]["multiplier_per"] = mul.ToString();
			_d0["room_info"]["current_time"] = curTime;

			if (Random.value >= 0.8f) {
				JSONNode user = new JSONObject();
				user["amount"] = Random.Range(10f, 100f);
				user["multiplier_level"] = mul.ToString();
				user["leave_time"] = curTime;
				users.Add(user);
			}

			_d0["room_users_info"] = users;

			_data["values"] = _d0.ToString();
			var str = _data.ToString();
			ApiManager.room_info(str);

			var dt = (2 - (Time.time - startTime) / (Time.time + Time.deltaTime));
			mul += Random.Range(0.006f * dt, 0.0085f * dt);
			yield return new WaitForSeconds(.1f);
		}

        {
			_d0["room_info"]["is_ended"] = "1";
			_d0["room_info"]["id"] = "1";
			_d0["room_info"]["multiplier_per"] = mul.ToString();

			_d0["room_users_info"] = users;

			_data["values"] = _d0.ToString();
			var str = _data.ToString();
			ApiManager.room_info(str);
		}

		JSONNode _lost = new JSONObject();
		_lost["body"] = "GG";
		ApiManager.bet_lost(_lost.ToString());

		yield return new WaitForSeconds(5f);
		StartCoroutine(RoomData());
	}
}
