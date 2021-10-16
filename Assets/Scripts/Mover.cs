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

    //Bounds CalculateBounds(RectTransform transform)
    //{
    //    Bounds bounds = new Bounds(transform.position, new Vector3(transform.rect.width, transform.rect.height, 0.0f));

    //    Debug.Log(bounds.min);
    //    Debug.Log(bounds.max);
    //    return bounds;
    //}
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
    //IEnumerator PerSec()
    //{
    //    while(true)
    //    {
    //        yield return new WaitForSeconds(1);
    //        list.Add(_payout);
    //        wg.Populatelist(list);
    //    }
       
    //}
    void Update()
    {



          MoveUpdate();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //   x++;
        //    //   y ++ ;
        //    //      x = Mathf.Lerp(x, x++, 0.1f);
         
        //    x++;
        //    y++;
        //}

        //    if (transform.position.x < x )
        //        transform.Translate(new Vector3(transform.position.x + x, 0, 0) * Time.deltaTime);

        //if (transform.position.y < y)
      //                transform.Translate(new Vector3(0, y, 0) * Time.deltaTime);

        

        //payoutText.color = roundOverText.color = _roundOver ? Color.red : Color.red;
        //roundOverText.text = _roundOver ? "Round Over" : "Current Payout";

        //if (!_roundOver)
        //{
        //    _duration += Time.deltaTime;

        //    HandleMovement();
        //    HandlePayout();
        //    HandleUI();

        //    if (transform.position.x > screenBounds.y)
        //    {
        //        _roundOver = true;

        //         OnEnd.Invoke();

        //        Invoke(nameof(Restart), 2.5f);
        //    }
        //}
    }

    //void HandleMovement()
    //{
       
    //   var ratio = Mathf.InverseLerp(screenBounds.x, screenBounds.y, transform.position.x);
      
    //   //_hSpeed = Mathf.Lerp(transform.position.x, transform.position.x + wg.timeStart,ratio);
    //   //_vSpeed = Mathf.Lerp(transform.position.y, transform.position.y + myMul, ratio);
    //    _zRot = Mathf.Lerp(0, maxRotation, ratio);

    //    transform.Translate( new Vector3(wg.counter, myMul, 0) * Time.deltaTime);   //   *  _hSpeed 
    //   // transform.Translate(new Vector3(0, myMul, 0)  * Time.fixedDeltaTime);    //    * _vSpeed
    //    transform.eulerAngles = new Vector3(0, 0, _zRot);
    //  //  Mathf.Clamp(transform.position.x, screenBounds.x, screenBounds.y);
    //}

    //void HandlePayout(float mul)
    //{
    //  //  payoutLayout.gameObject.SetActive(true);
    //    mul += _payoutRate * Time.deltaTime;

    //    //if (Time.time > _nextCoin)
    //    //{
    //    //    _nextCoin = Time.time + Random.Range(coinInterval.x, coinInterval.y);
    //    //    var c = Instantiate(coinPrefab, transform.position, Quaternion.identity);
    //    //    _coins.Add(c);
    //    //}
    //}

    //void HandleUI()
    //{
    //    payoutText.text = $"{_payout.ToString("F2")}x";
        
    //    //if(Time.time > _lastTimeStamp)
    //    //{
    //    //    _lastTimeStamp = Time.time + 1;

    //    //    var ui = Instantiate(timePrefabUI, timeLayout.transform);
    //    //    ui.GetComponentInChildren<TMP_Text>().text = Mathf.RoundToInt(_duration).ToString();
    //    //}
    //}

    void Restart()
    {
        //foreach(var coin in _coins)
        //{
        //    Destroy(coin);
        //}

        //foreach (var ui in timeLayout.GetComponentsInChildren<Transform>())
        //{
        //    if (ui != timeLayout.transform)
        //    {
        //        Destroy(ui.gameObject);
        //    }
        //}

       // transform.position = _startPos;
      //  _coins.Clear();
       
       // wg.timerActive = false;
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
        wg.counter = 0;
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