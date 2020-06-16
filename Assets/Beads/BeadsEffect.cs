﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadsEffect : MonoBehaviour
{
    
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        //rigid.AddForce(new Vector3(5f, 1f, 1f), ForceMode.Impulse);

        rigid.AddForce(new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15)), ForceMode.Impulse);
    }
}
