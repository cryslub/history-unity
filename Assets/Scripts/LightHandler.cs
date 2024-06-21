using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LightHandler : MonoBehaviour
{
    public float speed = 0.002f;
    private Vector3 dragOrigin;


    private Vector3 rotation = new Vector3(0f,0f,0f);

    private Vector3 target = new Vector3(0f, 0f,0f);
    private Vector3 targetDown = new Vector3(0f,0f,0f);

    //    private Vector3 target =  new Vector3((float)Mathf.PI*3/2, Mathf.PI / 6f ,0f);

    public float distance = 400f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

            render();

         //   Debug.Log(target);

    }

    void render(){
        rotation.x += speed;
//        rotation.y += (target.y - rotation.y) * 0.1f;
//        distance += (distanceTarget - distance) * 0.3f;

        float x = distance * Mathf.Sin(rotation.x) * Mathf.Cos(rotation.y);
        float y = distance * Mathf.Sin(rotation.y);
        float z = distance * Mathf.Cos(rotation.x) * Mathf.Cos(rotation.y);

//           Vector3 move = new Vector3(x,y,z);

//            Debug.Log(move);
        transform.position  =new Vector3(x,y,z);

      
        transform.LookAt(Vector3.zero);
      
    }

}
