using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float dragSpeed = 2;
    public Rect boundary;
    private Vector3 dragOrigin;

    private Vector3 rotation = new Vector3(0f,0f,0f);

    private Vector3 target = new Vector3(0f, 0f,0f);
    private Vector3 targetDown = new Vector3(0f,0f,0f);

    //    private Vector3 target =  new Vector3((float)Mathf.PI*3/2, Mathf.PI / 6f ,0f);

    public float distance = 300f;
    
    private float distanceTarget = 300f;

    public float zoomMin = 203f;
    public float zoomMax = 250f;

    // Start is called before the first frame update
    void Start()
    {
        MoveCameraTo(32f,41f);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") != 0f ) // forward
        {
          //  Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
//        minimap.orthographicSize += Input.GetAxis("Mouse ScrollWheel");
        }

         if (Input.GetMouseButtonDown(0))
        {
            
            dragOrigin = Input.mousePosition;
       //     Debug.Log(dragOrigin);
            targetDown.x = target.x;
            targetDown.y = target.y;
            return;
        }
 
        if (!Input.GetMouseButton(0)){
            render();
            return;
        }


 
            float zoomDamp = this.distance*this.distance/300000f;
//        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition c);
//        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
            target.x = targetDown.x + (Input.mousePosition.x-dragOrigin.x)*0.005f * zoomDamp;
            target.y = targetDown.y + (dragOrigin.y-Input.mousePosition.y)*0.005f * zoomDamp;
            render();

         //   Debug.Log(target);

    }

    public void Zoom(float delta){
       

        distanceTarget -= delta*(distanceTarget- zoomMin+10);

   //     Debug.Log(distanceTarget);

        distanceTarget = distanceTarget > zoomMax ? zoomMax : distanceTarget;
        distanceTarget = distanceTarget < zoomMin  ? zoomMin : distanceTarget;
    }

    void render(){
        rotation.x += (target.x - rotation.x) * 0.1f;
        rotation.y += (target.y - rotation.y) * 0.1f;

      //  Debug.Log(this.rotation.x + "," + this.rotation.y);
        if (this.rotation.x > boundary.x+ boundary.width) this.rotation.x = boundary.x + boundary.width;
        if (this.rotation.x < boundary.x) this.rotation.x = boundary.x;
        if (this.rotation.y < boundary.y) this.rotation.y = boundary.y;
        if (this.rotation.y > boundary.y+boundary.height) this.rotation.y = boundary.y + boundary.height;


        distance += (distanceTarget - distance) * 0.3f;

        float x = distance * Mathf.Sin(rotation.x) * Mathf.Cos(rotation.y);
        float y = distance * Mathf.Sin(rotation.y);
        float z = distance * Mathf.Cos(rotation.x) * Mathf.Cos(rotation.y);

//           Vector3 move = new Vector3(x,y,z);

//            Debug.Log(move);
        transform.position  =new Vector3(x,y,z);

        float tiltStart = 250f;
        if (distance < tiltStart)
        {
            float mult = (320f - y) / 80;
            transform.LookAt(new Vector3(0f, ((tiltStart-zoomMin) -(distance-205f))*mult, 0f));
           // transform.LookAt(Vector3.zero);
            // Debug.Log(transform.rotation);
        }
        else
        {
            transform.LookAt(Vector3.zero);
        }
    }

    void MoveCameraTo(float lat, float lng)
    {
        //	  var point = globePoint(lat,long,distance);
        target.x = (90f + lng) * Mathf.PI / 180f;
        target.y = lat / 180f * Mathf.PI;

      //  Debug.Log(target);
        //distanceTarget = distance;
    }

    void MoveCameraTo(float lat, float lng, float distance)
    {
        MoveCameraTo(lat, lng);
        this.distanceTarget = distance;
    }
}
