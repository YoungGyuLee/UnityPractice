using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadsEffect : MonoBehaviour
{
    
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(new Vector3(0f, 0f, Random.Range(0,1)), ForceMode.Impulse);

        //rigid.AddForce(new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15)), ForceMode.Impulse);
    }
}
