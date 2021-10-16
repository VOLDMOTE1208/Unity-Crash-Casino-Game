using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCase : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public LineRenderer linerenderer;
    public float vertexCount = 0;
    public float Point2Ypositio = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TrailDrawer.instance.first_line)
        {
            vertexCount++;
            Point2.transform.position = new Vector3((Point1.transform.position.x + Point3.transform.position.x), Point2Ypositio, (Point1.transform.position.z + Point3.transform.position.z) / 2);
            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(Point1.position, Point2.position, ratio);
                var tangent2 = Vector3.Lerp(Point2.position, Point3.position, ratio);
                 var curve = Vector3.Lerp(-tangent1, -tangent2*2, ratio);
               

                pointList.Add(curve);
            }

            linerenderer.positionCount = pointList.Count;
            linerenderer.SetPositions(pointList.ToArray());
            TrailDrawer.instance.line.enabled = false;
        }
        
    }
}
