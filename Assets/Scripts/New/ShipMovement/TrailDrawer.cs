using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrailDrawer : MonoBehaviour {
    public static TrailDrawer instance;
    public Transform rocketPoint;
    public Transform TrailPointsFolder;
    public float PointAddPeriod = 0.5f;
    public bool AutogeneratePoints;
    public LineRenderer line;
    int lastPointIndex;
    public bool first_line = false;
    List<Transform> points = new List<Transform>();
    Transform lastPoint => points[points.Count - 1];
    public float Point2Ypositio = 2;

    Vector3 pos => rocketPoint.localPosition;
    Coroutine _cor;
    //public Transform point2;

    void Awake() {
        if (rocketPoint == null) return;
        line = GetComponent<LineRenderer>();
        ResetPoints();
        EnableTrailDrawing();
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        first_line = false;
    }
    public Vector2 AddPoint() {
        lastPointIndex++;
        line.positionCount++;
        var _i = new GameObject($"point {lastPointIndex}");
        _i.transform.SetParent(TrailPointsFolder, false);
        points.Add(_i.transform);
        lastPoint.localPosition = pos;
        line.SetPosition(lastPointIndex - 1, lastPoint.position - transform.position);
        

        return lastPoint.localPosition;
        
    }
    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount++;
        lastPointIndex++;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < line.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
            t += (1 / (float)line.positionCount);
        }
        //float u = 1 - t;
        //float tt = t * t;
        //float uu = u * u;
        //Vector3 p = uu * point0;
        //p += 2 * u * t * point1;
        //p += tt * point2;
        //return p;
    }



    IEnumerator addPontVertices() {
        //Debug.Log("3");
        while (enabled) {
            AddPoint();
            yield return new WaitForSeconds(PointAddPeriod);
        }
    }

    Action _upd = delegate { };
    void FixedUpdate() {
        if (points.Count > 50)
        {
            first_line = true;
        }
        _upd();
    }

    void updateVertices() {
        lastPoint.localPosition = pos;
        for (int i = 0; i < points.Count; i++)
        {
            var _pos = points[i].position - transform.position;


            line.SetPosition(i, _pos);
        }
    }

    public void DisableTrailDrawing() {
        if (_cor != null)
            StopCoroutine(_cor);
        _upd -= updateVertices;
        lastPoint.localPosition = pos + Vector3.right * 0.05f;
        updateVertices();
    }

    public void ResetPoints() {
        foreach (Transform point in TrailPointsFolder) {
            Destroy(point.gameObject);
        }
        points.Clear();
        line.positionCount = 0;
        lastPointIndex = 0;
    }

    public void EnableTrailDrawing() {
        AddPoint();
        _upd += updateVertices;
        if (AutogeneratePoints)
            _cor = StartCoroutine(addPontVertices());
        else
            AddPoint();
        
    }
}