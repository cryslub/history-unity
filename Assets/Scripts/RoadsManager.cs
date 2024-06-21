using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadsManager : MonoBehaviour
{
    public float adj;

    public float basicRadius = 200;

    public float lineWidth = 0f;
    public float edgeWidth = 0.02f;

    public Material edge;
    public Material middle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearRoads()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }


    }

    public void DrawRoad(List<List<float>> coordinates, List<float> elevationAdjs)
    {

        

        float radius = basicRadius + adj;
        Util.DrawLine(gameObject, coordinates, lineWidth- edgeWidth, radius+0.005f, middle,true, elevationAdjs);
        Util.DrawLine(gameObject, coordinates, lineWidth, radius, edge, true, elevationAdjs);

    }
}
