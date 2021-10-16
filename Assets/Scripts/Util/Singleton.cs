using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            Debug.LogWarning($"Multiple instances of {this} were found! All duplicates have been deleted.");
        }
    }
}
