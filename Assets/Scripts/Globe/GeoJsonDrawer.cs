using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Linq;

public class GeoJsonDrawer : MonoBehaviour
{
     public TextAsset jsonFile;

    public float adj;

    public float basicRadius = 200;

    public float lineWidth = 0f;

    public bool useElevation = true;



    // Start is called before the first frame update
    void Start()
    {
        DrawGeoJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  DrawGeoJson(){

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        List<CombineInstance> combines = new List<CombineInstance>();

        Renderer renderer = GetComponent<Renderer>();

        lineRenderer.material = renderer.material;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 0;

        dynamic root = JsonConvert.DeserializeObject(jsonFile.text);
//        int count = 0;


        IterateJson(root,combines);        


        Mesh mesh = new Mesh();
        mesh.RecalculateNormals();
        mesh.CombineMeshes(combines.ToArray());

//        GetComponent<MeshFilter>().mesh = mesh;
    }

    public virtual void IterateJson(dynamic root,List<CombineInstance> combines){

        foreach(dynamic geometry in root.geometries){
            DrawGeometry(geometry,combines);  
        }
    }


    protected void DrawGeometry(dynamic geometry, List<CombineInstance> combines){
        
        JArray coordinates = geometry.coordinates;
        if(coordinates!=null){

            if(geometry.type == "Polygon"){
                //    Debug.Log(coordinates);
                DrawCoordinates(coordinates,combines);
            }else if(geometry.type == "MultiPolygon"){
                foreach(dynamic sub in coordinates){
                    DrawCoordinates(sub,combines);
                }
            }
            else if (geometry.type == "LineString")
            {

                DrawLine(coordinates.ToObject<List<List<float>>>());
                
            }
            else if (geometry.type == "MultiLineString")
            {
                foreach (dynamic sub in coordinates)
                {
                    DrawLine(sub.ToObject<List<List<float>>>());
                }
            }
        }
    }

    void DrawCoordinates(dynamic coordinates, List<CombineInstance> combines)
    {

        float radius = basicRadius + adj;

        Data data = EarcutLibrary.Flatten(coordinates);


        data.Vertices.RemoveAt(data.Vertices.Count - 1);
        data.Vertices.RemoveAt(data.Vertices.Count - 1);


        List<float> Vertices = data.Vertices;

        List<int> triangles = EarcutLibrary.Earcut(Vertices, data.Holes, data.Dim);
        //   Debug.Log($"[{string.Join(",", triangles)}]");

        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < Vertices.Count; i += 2)
        {
            Vector3 vector = new Vector3(Vertices[i], Vertices[i + 1], 0f);
            vertices.Add(vector);
        }

        divide(vertices, triangles);

        List<Vector3> vs = new List<Vector3>();
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vector = Util.gpsToVector(vertices[i].y,vertices[i].x, radius, useElevation);
            vs.Add(vector);
        }

        Mesh m = new Mesh();

        m.vertices = vs.ToArray();
        // triangles.Reverse();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.RecalculateTangents();

        GameObject child = new GameObject();
        MeshFilter meshFilter = child.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = child.AddComponent<MeshRenderer>();
        Renderer renderer = GetComponent<Renderer>();
        meshRenderer.material = renderer.material;
        meshFilter.mesh = m;
        child.transform.SetParent(transform, false);

 //       CombineInstance combine = new CombineInstance();
//        combine.mesh = m;
 //       combine.transform = transform.localToWorldMatrix;
//        combines.Add(combine);
    }
    
    void DrawLine(List<List<float>> coordinates)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();


        float radius = basicRadius + adj;
        Util.DrawLine(gameObject, coordinates, lineWidth,radius,renderer.material,true);

     }

    bool checkDistance(List<Vector3> vertices,int point1,int point2,int point3,List<int> adds){



        float triangleMax = 0.8f;
        bool ret = false;
        float distance = getDistance(vertices,point1, point2);
        if(distance> triangleMax)
        {
        //    Debug.Log(distance);
            vertices.Add(Vector3.Lerp(vertices[point1],vertices[point2],0.5f));
            adds.Add(point1);
            adds.Add(vertices.Count-1);
            adds.Add(point3);

            adds.Add(vertices.Count-1);
            adds.Add(point2);
            adds.Add(point3);

//                                    Debug.Log($"[{string.Join(",", adds.ToArray())}]");
//                                    Debug.Log($"[{string.Join(",", triangles.ToArray())}]");
            ret = true;

        } 
        return ret;
    }

    float getDistance(List<Vector3> vertices, int point1, int point2)
    {
        return Vector3.Distance(vertices[point1], vertices[point2]);
        
    }

    void divide(List<Vector3> vertices,List<int> triangles){

        List<int> adds = new List<int>();
        List<int> removes = new List<int>();
        for (int i=0;i<triangles.Count;i+=3){

            List<float> list = new List<float>();

            list.Add(getDistance(vertices, triangles[i], triangles[i + 1]));
            list.Add(getDistance(vertices, triangles[i+1], triangles[i + 2]));
            list.Add(getDistance(vertices, triangles[i+2], triangles[i]));

            int index = list.IndexOf(list.Max());
            bool result = false;
            switch (index)
            {
                case 0:
                    result = checkDistance(vertices, triangles[i], triangles[i + 1], triangles[i + 2], adds);
                    break;
                case 1:
                    result = checkDistance(vertices, triangles[i+1], triangles[i + 2], triangles[i], adds);
                    break;
                case 2:
                    result = checkDistance(vertices, triangles[i+2], triangles[i], triangles[i + 1], adds);
                    break;

            }

            if (result)
            {
                removes.Add(i);
            }
        }

        for(int i = removes.Count-1; i >=0; i--)
        {
            triangles.RemoveAt(removes[i]);
            triangles.RemoveAt(removes[i]);
            triangles.RemoveAt(removes[i]);
        }

        if (adds.Count>0){
//            Debug.Log($"[{string.Join(",", triangles.ToArray())}]");
//            Debug.Log($"[{string.Join(",", adds.ToArray())}]");
            
            divide(vertices,adds);            
        }

        triangles.AddRange(adds); 

    }


}
