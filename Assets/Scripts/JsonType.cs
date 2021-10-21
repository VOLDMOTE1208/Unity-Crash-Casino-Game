using UnityEngine;
using System.Collections;

public class JsonType {
    public string userName;
    public string betAmount;
    public string autoCashAmount;
}

public class ReceiveJsonObject {
    public string userName;
    public string betAmount;
    public int timeCount;
    public float currentAmount;
    public bool running;
    public ReceiveJsonObject() {
    }
    public static ReceiveJsonObject CreateFromJSON(string data) {
        return JsonUtility.FromJson<ReceiveJsonObject>(data);
    }
}