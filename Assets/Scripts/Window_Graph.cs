/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
   // private RectTransform dashTemplateX;
   // private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;



    public float timeStart = 0;

    public int counter = 0;
    public List<int> listX;
    //public Text textBox;
    // public Text startBtnText;

    // public bool timerActive = false;

    private void Start()
    {
       
    }

    private void Update()
    {
      //  if (timerActive)
      //  {
            timeStart += Time.time;
          //  textBox.text = timeStart.ToString("F2");
       // }else
       // {
       //     timeStart = 0;
     //   }
    }


    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
      //  dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
      //  dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

       // List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 50, 30, 60, 50, 40, 20 };//, 5, 20, 10, 50, 30, 20, 11 };
       // ShowGraph(valueList, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));


    }



    public void Populatelist(List<float> myListY)
    {
        counter++;
        listX.Add(counter);
        List<int> valueListX = listX;
        List<float> valueListY = myListY;
        ShowGraph(valueListY,valueListX, -1, -1,(int _i) => (_i).ToString("F0") + "s", (float _f) =>  (_f).ToString("F2") + "x");
    }
  


    private void ShowGraph(List<float> valueListY,List<int> valueListX, int maxVisibleValueAmountY = -1, int maxVisibleValueAmountX = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmountY <= 0) {
            maxVisibleValueAmountY = valueListY.Count;
        }
        if (maxVisibleValueAmountX <= 0)
        {
            maxVisibleValueAmountX = valueListX.Count;
        }

        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueListY[0];
        float yMinimum = valueListY[0];

        float xMaximum = valueListX[0];
        float xMinimum = valueListX[0];

        for (int i = Mathf.Max(valueListY.Count - maxVisibleValueAmountY, 0); i < valueListY.Count; i++) {
            float value = valueListY[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }
        for (int i = Mathf.Max(valueListX.Count - maxVisibleValueAmountX, 0); i < valueListX.Count; i++)
        {
            float value = valueListX[i];
            if (value > xMaximum)
            {
                xMaximum = value;
            }
            if (value < xMinimum)
            {
                xMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        float xDifference = xMaximum - xMinimum;


        if (yDifference <= 0) {
            yDifference = 5f;
        }
        if (xDifference <= 0)
        {
            xDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        xMaximum = xMaximum + (xDifference * 0.2f);
        xMinimum = xMinimum - (xDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero
        xMinimum = 0f;

        float xSize = graphWidth / (maxVisibleValueAmountY + 1);

       // int xIndex = 0;

     //   GameObject lastCircleGameObject = null;
        int separatorCount = 10;
        //  for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) { /////
        for (int i = 0; i <= separatorCount-2; i++)
        {
            //  float xPosition = xSize + xIndex * xSize;
            // float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            //  GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            //    gameObjectList.Add(circleGameObject);
            //   if (lastCircleGameObject != null) {
            //        GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            //       gameObjectList.Add(dotConnectionGameObject);
            //    }
            //    lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            float normalizedValue = i * 1f / (separatorCount-2); ////added
            labelX.anchoredPosition = new Vector2(normalizedValue * graphWidth, -7f); /////
            labelX.GetComponent<Text>().text = getAxisLabelX((int)(xMaximum + (normalizedValue * (xMaximum - xMinimum)))); /////
            //labelX.anchoredPosition = new Vector2(xPosition, -7f); /////
            //  labelX.GetComponent<Text>().text = getAxisLabelX(i);//////
            gameObjectList.Add(labelX.gameObject);

         //   RectTransform dashX = Instantiate(dashTemplateX);
         //   dashX.SetParent(graphContainer, false);
         //   dashX.gameObject.SetActive(true);
        //    dashX.anchoredPosition = new Vector2(xPosition, -3f);
          //  gameObjectList.Add(dashX.gameObject);

          //  xIndex++;
        }

        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

        //    RectTransform dashY = Instantiate(dashTemplateY);
         //   dashY.SetParent(graphContainer, false);
         //   dashY.gameObject.SetActive(true);
         //   dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
         //   gameObjectList.Add(dashY.gameObject);
        }
    }

    //private GameObject CreateCircle(Vector2 anchoredPosition) {
    //    GameObject gameObject = new GameObject("circle", typeof(Image));
    //    gameObject.transform.SetParent(graphContainer, false);
    //    gameObject.GetComponent<Image>().sprite = circleSprite;
    //    RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
    //    rectTransform.anchoredPosition = anchoredPosition;
    //    rectTransform.sizeDelta = new Vector2(11, 11);
    //    rectTransform.anchorMin = new Vector2(0, 0);
    //    rectTransform.anchorMax = new Vector2(0, 0);
    //    return gameObject;
    //}

    //private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
    //    GameObject gameObject = new GameObject("dotConnection", typeof(Image));
    //    gameObject.transform.SetParent(graphContainer, false);
    //    gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
    //    RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
    //    Vector2 dir = (dotPositionB - dotPositionA).normalized;
    //    float distance = Vector2.Distance(dotPositionA, dotPositionB);
    //    rectTransform.anchorMin = new Vector2(0, 0);
    //    rectTransform.anchorMax = new Vector2(0, 0);
    //    rectTransform.sizeDelta = new Vector2(distance, 3f);
    //    rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
    //    rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    //    return gameObject;
    //}
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
