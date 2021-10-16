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
    public ReceiveJsonObject() {
    }
    public static ReceiveJsonObject CreateFromJSON(string data) {
        return JsonUtility.FromJson<ReceiveJsonObject>(data);
    }
}