using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCube : MonoBehaviour
{

    public GameObject splashEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider.transform.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Instantiate(splashEffect, hit.collider.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
