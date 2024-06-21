using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public static class Util
{
    public static Vector3 gpsToVector(float lat, float lng, float radius, bool useElevation)
    {
        var phi = (90 - lat) * Mathf.PI / 180;
        var theta = (180 - lng) * Mathf.PI / 180;


        float elevation = 0;

        if (useElevation) elevation = Elevation.getElevation(lat, lng); ;
        float scale = 1200f;

        float sphereSize = radius + Mathf.Floor(elevation /scale)*scale/ 2500;
        //if(radius !== undefined) sphereSize = radius;

        return new Vector3(
                sphereSize * Mathf.Sin(phi) * Mathf.Sin(theta),
                sphereSize * Mathf.Cos(phi),
                sphereSize * Mathf.Sin(phi) * Mathf.Cos(theta)

        );
    }

    public static void DrawLine(GameObject gameObject, List<List<float>> coordinates, float lineWidth, float radius, Material material, bool isNeedToSmooth)
    {
        List<float> elevationAdjs = new List<float>();
        DrawLine(gameObject, coordinates, lineWidth, radius, material, isNeedToSmooth,elevationAdjs);
    }


    public static void DrawLine(GameObject gameObject, List<List<float>> coordinates,float lineWidth,float radius,Material material,bool isNeedToSmooth, List<float> elevationAdjs)
    {


        GameObject child = new GameObject();
        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.positionCount = coordinates.Count;
        //        lineRenderer.startWidth = 0f;
        //        lineRenderer.endWidth = lineWidth;
        lineRenderer.widthMultiplier = lineWidth;
  //      lineRenderer.generateLightingData = true;
        lineRenderer.numCapVertices = 5;
        //        lineRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
       

//        lineRenderer.useWorldSpace = false;
// lineRenderer.alignment = LineAlignment.TransformZ;

        child.transform.SetParent(gameObject.transform, false);

        int i = 0;
  //      CurvedLineRenderer curvedLineRenderer = lineRenderer.AddComponent<CurvedLineRenderer>();

        List<Vector3> list = new List<Vector3>();
        foreach (List<float> coordinate in coordinates)
        {
            //  Debug.Log(coordinate);
            ///      if (i > lineRenderer.positionCount) lineRenderer.positionCount++;
            var elevationAdj = 0f;
            if (i<elevationAdjs.Count)
            {
                elevationAdj = elevationAdjs[i];
            }

            list.Add(Util.gpsToVector((float)coordinate[1], (float)coordinate[0], radius+ elevationAdj, true));
            //            curvedLineRenderer.AddGizmo(Util.gpsToVector((float)coordinate[1], (float)coordinate[0], radius, true));
            //lineRenderer.SetPosition(i++, Util.gpsToVector((float)coordinate[1], (float)coordinate[0], radius, true));
            i++;
        }

        Vector3[] linePositions = list.ToArray();
        Vector3[] smoothedPoints = linePositions;

        if (isNeedToSmooth)
            smoothedPoints = LineSmoother.SmoothLine(linePositions, 0.15f);

        lineRenderer.positionCount = smoothedPoints.Length;
        lineRenderer.SetPositions(smoothedPoints);

        //        curvedLineRenderer.Draw();

    }

}
