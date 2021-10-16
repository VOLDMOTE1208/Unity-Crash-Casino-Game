using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global {

    public static string DOMAIN = "localhost";//"31.220.108.168";//
    public static int PORT = 3000;
    public static bool SSL_ENALBLED = false;
    /***************************************/

    /*********** Android *********************/
    // public static string DOMAIN = "31.220.108.168";
    // public static int PORT = 3002;
    // public static bool SSL_ENALBLED = false;
    /***************************************/

    public static string currentDomain = "";

    public static string betAmount = "0";

    public static float balance = 0;
    public static int minAmount = 10;

    public static bool socketConnected;
    public static bool mainPlayer;
    public static bool isMultiplayer;
    public static int myTurn;

    public static User m_user;
    public static SaveData savedData = new SaveData();
    public static bool isLoading = false;
    public static bool nextLoad;
    public static string myname = "";
    public static List<string> othernames = new List<string>();

    public static string GetDomain() {
        currentDomain = DOMAIN;        
        currentDomain = "http://" + currentDomain+":"+ PORT;
        return currentDomain;
}

    [Serializable]
    public class User {
        public long id;
        public string name;
        public long score;

        public string address;

        public User(long id = -1, string name = "", long score = 0, string address = "") {
            this.id = id;
            this.name = name;
            this.score = score;
            this.address = address;
        }
    }

    [Serializable]
    public class SaveData {
        public int cntPlayers;
        public int turn;
        public bool initial;
        public List<int> positions;

        public static SaveData CreateFromJSON(string data) {
            return JsonUtility.FromJson<SaveData>(data);
        }

        public SaveData() {
            cntPlayers = -1;
            turn = -1;
            initial = false;
            positions = new List<int>();
        }
    }

    [Serializable]
    public class BetInfo {
        public long id;
        public float amount;

        public BetInfo() {
            if (Global.m_user != null) {
                id = Global.m_user.id;
            }
            amount = float.Parse(Global.betAmount);
        }

        public static BetInfo CreateFromJSON(string data) {
            return JsonUtility.FromJson<BetInfo>(data);
        }
    }
}
