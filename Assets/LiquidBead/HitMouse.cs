using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HitMouse : MonoBehaviour
{
    public GameObject splashEffect;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider.transform.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //Instantiate(splashEffect, hit.collider.transform.position, hit.collider.transform.rotation);
                    //Instantiate(splashEffect, hit.collider.transform.position, Quaternion.identity);
                    Debug.Log(hit.collider.transform.rotation);
                    //Instantiate(splashEffect, hit.collider.transform.position, h);
                    Quaternion q1 = Quaternion.Euler(new Vector3(1.0f, 0.0f, 0.0f));

                    Quaternion q2 = hit.collider.transform.rotation;

                    Quaternion myQ = q2 * q1;
                    Debug.Log("1 : " + q1);
                    Debug.Log("2 : " + q2);
                    Debug.Log("3 : " + myQ);



                    Instantiate(splashEffect, hit.collider.transform.position, myQ);
                }
            }
        }
    }
}
