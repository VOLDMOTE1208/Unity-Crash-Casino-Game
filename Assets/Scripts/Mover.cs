using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// unused legacy from prev developer

//Use an auto scroll view instead for the UI. Preconfigure all the UI in edit mode and scroll at runtime
public class Mover : MonoBehaviour
{
#pragma warning disable 0649

    [Header("Movement")]
   // public Vector2 horizontalSpeed = new Vector2(2, 0.5f);
   // public Vector2 verticalSpeed = new Vector2(0, 1.5f);
    public Vector2 coinInterval = new Vector2(0.5f, 1);
    public Vector2 screenBounds; // new Vector2(-7, 9);
    public float maxRotation = 30;

    [Header("Payout")]
    public Vector2 payoutRateRange = new Vector2(0.1f, 0.3f);
    public TMP_Text payoutText;
    public TMP_Text roundOverText;
    public GameObject coinPrefab;

    [Header("Graph")]
   // public HorizontalOrVerticalLayoutGroup timeLayout;
   // public HorizontalOrVerticalLayoutGroup payoutLayout;
    //public GameObject timePrefabUI;
   // public GameObject payoutPrefabUI;

    [Header("Events")]
    public UnityEvent OnEnd, OnRestart;

    List<GameObject> _coins = new List<GameObject>();
    Vector3 _startPos;
    bool _roundOver;
    float _hSpeed;
    float _vSpeed;
    float _payout;
    float _nextCoin;
    float _payoutRate;
    float _zRot;
    float _duration;
    float _lastTimeStamp;

    public Window_Graph wg;
    [SerializeField]
    List<float> listY;   

    public  bool myEnded;
    float myMul;
    //float x = 0;
    //float y = 0;

    private Rigidbody2D rb;
    private Vector2 endPosition;//= new Vector3(-6, -2.5f);

   // Vector2 lastPos;
    // public RectTransform graphBound;
    void Start()
    {
       // lastPos = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        float x =  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)).x;
        screenBounds = new Vector2(x, -x);
      //  CalculateBounds(graphBound);
      
        //payoutLayout.gameObject.SetActive(false);
        _startPos = transform.position;
      //  payoutLayout.gameObject.SetActive(true);
        ResetValues();

    //    StartCoroutine(PerSec());
    }
    public void Move(float mul)
    {
        myMul = mul;
        //lastPos = new Vector2(rb.position.x + wg.timeStart, rb.position.y + myMul);


        endPosition = new Vector2(rb.position.x + wg.counter, rb.position.y + myMul);
    }
    public void Populate()
    {
        listY.Add(myMul);
       // listX.Add((int)wg.timeStart);
        wg.Populatelist(listY);
    }

 
    void MoveUpdate()
    {

        if (!myEnded)
        {

            var ratio = Mathf.InverseLerp(screenBounds.x, screenBounds.y, transform.position.x);
            _zRot = Mathf.Lerp(0, maxRotation, ratio);
            transform.eulerAngles = new Vector3(0, 0, _zRot);

            // HandleMovement();
            if (rb.position != endPosition)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, 3 * Time.deltaTime);
                rb.MovePosition(newPosition);
            }

                payoutText.color = Color.white;
            roundOverText.color = Color.white;
            payoutText.text = $"{myMul.ToString("F2")}x";
            roundOverText.text = "Current Payout";
        }
        else
        {
            payoutText.color = Color.red;
            roundOverText.color = Color.red;
            roundOverText.text = "Round Over" ;
           
        }
     
    }
    void Update() {
        MoveUpdate();
    }

    void Restart()
    {
        ResetValues();      
    }
    public void SetPos()
    {
        transform.position = _startPos;
    }
   public void ResetValues()
    {
       // _roundOver = false;
       // OnRestart.Invoke();
        myMul = 0;
        wg.counter = 10;
       // transform.position = _startPos;
        wg.timeStart = 0;
       // wg.timeStart = 0;
        if (listY.Count > 0)
        listY.Clear();

        if (wg.listX.Count > 0)
            wg.listX.Clear();


        //_duration = 0;
        //_payout = _lastTimeStamp = 1;
        //_nextCoin = Random.Range(coinInterval.x, coinInterval.y);
        // _payoutRate = Random.Range(payoutRateRange.x, payoutRateRange.y);
    }
}

#pragma warning restore 0649