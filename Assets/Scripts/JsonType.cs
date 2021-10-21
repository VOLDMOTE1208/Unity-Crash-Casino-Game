using UnityEngine;
using System.Collections;

public class JsonType {
    public string userName;
    public float betAmount;
    public float autoCashAmount;
    public bool IsCashOut;
}

public class ReceiveJsonObject {
    public string userName;
    public string betAmount;
    public int timeCount;
    public float currentAmount;
    public bool running;
    public float leftAmount;
    public ReceiveJsonObject() {
    }
    public static ReceiveJsonObject CreateFromJSON(string data) {
        return JsonUtility.FromJson<ReceiveJsonObject>(data);
    }
}