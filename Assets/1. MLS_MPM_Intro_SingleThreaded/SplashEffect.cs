﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : MonoBehaviour
{
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15)), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
