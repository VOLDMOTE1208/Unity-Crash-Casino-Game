using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
    [Header("Objects")]
    public static GameControl instance;
    public GameObject objectToScale;
    public float ScaleMultiplier;
    public Transform TopRightCorner;
    public RocketControl rc;
    Transform rcTf => rc.transform;
    public RocketObject rocket;
    public TrailDrawer trail;

    [Header("Containers")]
    public Transform PointsFolder;
    public Transform ObjectFolder;

    [Header("Bet cacheout prefabs")]
    public MovablePoint movablePoint;
    public CacheoutMark cacheoutMark;

    [Header("Side Graphs")]
    public GraphGenerator SideGraph;
    public TimeGraph timeGraph;

    Vector2 tr_pos => TopRightCorner.position;
    Vector2 rc_pos => rcTf.position;

    Vector2 tr_loc => TopRightCorner.localPosition;
    Vector2 rc_loc => rcTf.localPosition;

    // room data
   struct PointInfo {
        public float time;
        public Vector3 pos;
    }

    private bool hasStarted = false;
    private DateTime roomStartTime;
    List<RoomUserData> leavedUsers;
    RoomData prevData;
     List<PointInfo> linePositions = new List<PointInfo>();

    private void Awake() {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        leavedUsers = new List<RoomUserData>();
        if (instance == null)
            instance = this;
    }

    Action _upd = delegate { };
    void FixedUpdate() {
        _upd();
    }
    void _Scale() {
        var sc = Vector3.one * ScaleMultiplier;

        if (rc_pos.y >= tr_pos.y) {
			sc.y = tr_loc.y / rc_loc.y;
		} else {
			sc.y = objectToScale.transform.localScale.y;
		}

		//var _l = Mathf.InverseLerp(rc.startYGlob, TopRightCorner.position.y, rc.transform.position.y);
		//var _s = tr_loc.y / rc_loc.y / ScaleMultiplier;
		//sc.y = Mathf.Lerp(ScaleMultiplier, _s, _l);

		//sc.y = tr_loc.y / rc_loc.y;

		SideGraph.transform.localScale = sc;
        SideGraph.CheckGen();

		if (rc_pos.x >= tr_pos.x) {
            sc.x = tr_loc.x / rc_loc.x;
        } else {
            sc.x = objectToScale.transform.localScale.x;
        }
        objectToScale.transform.localScale = sc;
    }

    public void ResetObjects() {
        objectToScale.transform.localScale = Vector3.one * ScaleMultiplier;
        rc.ResetValues();
        trail.ResetPoints();
        rocket.Restore();
        timeGraph.ResetGraph();
        foreach(Transform t in PointsFolder) {
            Destroy(t.gameObject);
        }
        leavedUsers.Clear();
        linePositions.Clear();
    }

    public void StartGame() {
        ResetObjects();
        SideGraph.ResetGraph();
        StartWithoutReset();
    }

    void StartWithoutReset() {
        trail.EnableTrailDrawing();
        _upd += _Scale;
        rc.startMoving();
        hasStarted = true;

        SideGraph.GenerateGraph();

        StopAllCoroutines();
        StartCoroutine(GenTimeGraph());
    }

    public void EndGame() {
        rocket.Crash();
        _upd -= _Scale;
        rc.stopMoving();
        trail.DisableTrailDrawing();
        prevData = null;
        hasStarted = false;

        StopAllCoroutines();
	}

    public void ProcessRoomData(RoomData data) {
        roomStartTime = DateTime.Parse(data.startTimeString);
        var currTime = DateTime.Parse(data.currentTime);
        var timeFromStart = (float)(currTime - roomStartTime).TotalSeconds;

        // todo: fix started room showing
        if (prevData != null) {
            var dy = data.multipliyer - prevData.multipliyer;
            rc.MoveByVal(dy);
        } else {
            if (!hasStarted) {
                CreateTrailForTime(timeFromStart, data.multipliyer);
                StartWithoutReset();
            }
		}

        AddPoint(timeFromStart);
        // todo: side graph scaling (called there)
        //SideGraph.ScaleToCurrVal(data.multipliyer);
        prevData = data;

        foreach (var user in data.users) {
			if (!string.IsNullOrEmpty(user.LeaveTime)) {
                AddUser(user);
            }
		}
    }

    void AddPoint(float timeFromStrat) {
        PointInfo p = new PointInfo();
        p.time = timeFromStrat;
        p.pos = trail.AddPoint();
        linePositions.Add(p);
    }

    void CreateTrailForTime(float time, float mult) {
        rocket.Restore();
        Debug.Log($"creating trail for {time} seconds");
        time = Mathf.Abs(time);
        float dt = time / Time.deltaTime;
        float t = 0;
        mult--;
        while (t < time) {
            rc.MoveWithDtInstant(mult / dt);
            AddPoint(t);
            t += Time.deltaTime;
        }
        Debug.Log(t);
    }

    void AddUser(RoomUserData user) {
        if (leavedUsers.Contains(user)) return;
        leavedUsers.Add(user);
        var leaveTime = DateTime.Parse(user.LeaveTime);
        var diff = leaveTime - roomStartTime;
		float time = (float)diff.TotalSeconds;
        float mlt = float.Parse(user.multiplier_level);
        float totalAmn = float.Parse(user.Amount) * mlt;
        CreateMark(time, totalAmn.ToString("F2"), mlt.ToString("F2"));
    }

    Vector2 TimeToPos(float time) {
        int i = 0;
        do {
            i++;
            if (linePositions.Count <= i) {
                //Debug.Log($"out of bounds at: {time}");
                return rc.transform.localPosition;
            }
        } while (linePositions[i].time < time);
        PointInfo p1 = linePositions[i - 1];
        PointInfo p2 = linePositions[i];
        var _l = Mathf.InverseLerp(p1.time, p2.time, time);
        return Vector2.Lerp(p1.pos, p2.pos, _l);
	}

    void CreateMark(float time, string amn, string mult) {
        var point = Instantiate(movablePoint, PointsFolder, false);
        point.transform.localPosition = TimeToPos(time);
        var mark = Instantiate(cacheoutMark, ObjectFolder, false);
        mark.amount = '$' + amn;
        mark.amountValue = float.Parse(amn);
        mark.mult = mult;
        point.attachedObject = mark.gameObject;
    }

    IEnumerator GenTimeGraph() {
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(0.5f);
        yield return new WaitForSeconds(2);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(1);
        yield return new WaitForSeconds(2);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(2);
        yield return new WaitForSeconds(6);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(5);
        yield return new WaitForSeconds(10);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(10);
        yield return new WaitForSeconds(30);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(30);
        yield return new WaitForSeconds(90);
        timeGraph.ResetGraph();
        timeGraph.GenerateGraph(30);
    }
}

public class RoomData {
    public int isEnded;
    public float multipliyer;
    public string id;
    public string startTimeString;
    public string currentTime;
    public List<RoomUserData> users;
}

public struct RoomUserData {
    public string multiplier_level;
    //public string Name;
    public string Amount;
    public string LeaveTime;
}