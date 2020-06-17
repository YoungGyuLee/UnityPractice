using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMouseLiquid : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem liquidParticle1;

    [SerializeField]
    private ParticleSystem liquidParticle2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) { 
 

            if (hit.collider.tag == "liquid")
            {
            
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //Instantiate(splashEffect, hit.collider.transform.position, hit.collider.transform.rotation);
                    //Instantiate(splashEffect, hit.collider.transform.position, Quaternion.identity);
                    //Debug.Log(hit.collider.transform.rotation);
                    //Instantiate(splashEffect, hit.collider.transform.position, h);
                    //Quaternion q1 = Quaternion.Euler(new Vector3(1.0f, 0.0f, 0.0f));

                    //Quaternion q2 = hit.collider.transform.rotation;

                    //Quaternion myQ = q2 * q1;
                    Debug.Log("클릭3");


                    if (liquidParticle1.isPlaying)
                        liquidParticle1.Stop();
                    else
                        liquidParticle1.Play();

                    if (liquidParticle2.isPlaying)
                        liquidParticle2.Stop();
                    else
                        liquidParticle2.Play();
    

                }
            }
            
        }
    }
}
