using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    // public GameObject animGO;
    public GameObject planets;
    public GameObject mover;
    Animator anim;
    public GameObject graph;
    public Camera perp;
    
    // Start is called before the first frame update
    void Start()
    {
      //  planets.SetActive(false);
       // graph.SetActive(false);
       // mover.SetActive(false);
       // animGO.SetActive(true);
        anim = GetComponent<Animator>();
      // anim.SetBool("Play",true);
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    public void Trigger()
    {
       // planets.SetActive(true);
       // graph.SetActive(true);
        anim.SetBool("Play", false);
        mover.SetActive(true);
        perp.enabled = false;
      //  gameObject.SetActive(false);
    }
}
