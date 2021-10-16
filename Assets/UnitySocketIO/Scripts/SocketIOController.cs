using UnityEngine;
using System;
using System.Collections;
using UnitySocketIO.SocketIO;
using UnitySocketIO.Events;

namespace UnitySocketIO
{
    public class SocketIOController : MonoBehaviour
    {

        public SocketIOSettings settings;
        public static string domain = "localhost";
        public BaseSocketIO socketIO;
        public static SocketIOController instance;
        public string SocketID { get { return socketIO.SocketID; } }

        void Awake()
        {

            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            DontDestroyOnLoad(gameObject);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                socketIO = gameObject.AddComponent<WebGLSocketIO>();
            }
            else
            {
                socketIO = gameObject.AddComponent<NativeSocketIO>();
            }

            settings.sslEnabled = true;
            settings.url = "localhost";
            settings.port = 3000;

            Debug.Log("Port : " + settings.port);
            Debug.Log("URL : " + settings.url);

            socketIO.Init(settings);
        }

        private void Start()
        {          

            On("connected", Connected);

            Connect();

            StartCoroutine(iReconnect());
        }

        IEnumerator iReconnect()
        {

            yield return new WaitForSeconds(2f);
            

            Debug.Log("*******Socket Connecting...");
            Connect();
            //StartCoroutine(iReconnect());
        }
        private void Connected(SocketIOEvent obj)
        {
            Debug.Log("Socket Connected.");
        }


        public void Connect()
        {
            socketIO.Connect();
        }

        public void Close()
        {
            socketIO.Close();
        }

        public void Emit(string e)
        {
            socketIO.Emit(e);
        }
        public void Emit(string e, Action<string> action)
        {
            socketIO.Emit(e, action);
        }
        public void Emit(string e, string data)
        {
            socketIO.Emit(e, data);
        }
        public void Emit(string e, string data, Action<string> action)
        {
            socketIO.Emit(e, data, action);
        }

        public void On(string e, Action<SocketIOEvent> callback)
        {
            socketIO.On(e, callback);
        }
        public void Off(string e, Action<SocketIOEvent> callback)
        {
            socketIO.Off(e, callback);
        }



    }
}