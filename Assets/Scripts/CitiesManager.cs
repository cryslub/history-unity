using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using UnityEngine.ResourceManagement.AsyncOperations;
using PopupWindow = UnityEditor.PopupWindow;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using System.Drawing;
using Color = UnityEngine.Color;
using UnityEngine.UI;

public class CitiesManager : MonoBehaviour
{
    public float adj;

    public float basicRadius = 200;

    public TMP_FontAsset fontAsset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
      //  Debug.Log("fixed");
        if (Input.GetMouseButtonDown(0))
        {
    //        Debug.Log("clicked");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100)) {
  //              Debug.Log(hit);

                CityObject cityObject = hit.transform.GetComponent<CityObject>();
//                Debug.Log(cityObject);
                City city = cityObject.GetCity();
                if (city != null) {
               //     Debug.Log(city.name);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.position);
                    Rect menuRect = new Rect(screenPos.x, screenPos.y - 100, 600, 500);
                //    PopupWindow.Show(menuRect, new CityInfo(city));
                };

            //    Debug.DrawLine(ray.origin, hit.point);
            }

        }
    }
    public void addObjects(List<City> cities)
    {


        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }

        float radius = basicRadius + adj;

        List<CombineInstance> combines = new List<CombineInstance>();



        foreach (City city in cities)
        {
            Transform tr;
            GameObject child;
            int population = (int)city.population;
            float adj2 =0.04f;
            if (population == 0)
            {
                child = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                tr = child.transform;
                tr.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                tr.LookAt(Vector3.zero);
                adj2 = -0.02f;                
            }
            else
            {
                float scale = Mathf.Sqrt(Mathf.Sqrt(population))/100;
                scale = Mathf.Max(scale, 0.1f);
                scale = Mathf.Min(scale, 0.3f);

                child = GameObject.CreatePrimitive(PrimitiveType.Cube);
                child.name = city.name;


                tr = child.transform;
                var cubeRenderer = child.GetComponent<Renderer>();

                UnityEngine.Color customColor;
                ColorUtility.TryParseHtmlString(DataManager.factionMap[city.faction].color, out customColor);

                // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
                cubeRenderer.material.SetColor("_Color", customColor);
               

                child.transform.localScale = new Vector3(scale,  0.05f, scale);

                

             //   GameObject text = new GameObject();
               // text.transform.SetParent(transform, false);
                
               // TextMeshPro textMeshPro = text.AddComponent<TextMeshPro>();
              //  LocalizedString localizeString = new LocalizedString() { TableReference = "City Name", TableEntryReference = city.name };
              //  textMeshPro.font = fontAsset;
             //   textMeshPro.text = localizeString.GetLocalizedString();

             //   textMeshPro.outlineWidth = 0.1f;
            //    textMeshPro.outlineColor = new Color32(0, 0, 0, 255);
          //      textMeshPro.autoSizeTextContainer = true;
//                Debug.Log(textMeshPro.rectTransform.rect.height);

             //   text.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                
           //     text.transform.position = Util.gpsToVector((float)city.latitude+(scale/4)*0.3f+0.01f* textMeshPro.rectTransform.rect.height, (float)city.longitude, radius + 0.1f+city.elevationAdj, true);
         //       text.transform.LookAt(Vector3.zero);

                //                text.transform.position = new Vector3(0.1f, -0.1f, 0);
                //                text.transform.Rotate(-90,0,0);

                //     text.transform.LookAt(Vector3.zero);
                AddCityInfo(child,city, radius);
            }

            CityObject cityObject = child.AddComponent<CityObject>();
            cityObject.SetCity(city);

            child.transform.SetParent(transform, false);
            child.transform.position = Util.gpsToVector((float)city.latitude, (float)city.longitude,radius+ adj2+city.elevationAdj, true);
            child.transform.LookAt(Vector3.zero);
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            meshFilter.mesh.RecalculateBounds();

            tr.Rotate(tr.rotation.x+90, tr.rotation.y, tr.rotation.z);

        }


    }


    private void AddCityInfo(GameObject parent, City city, float radius)
    {
        GameObject go;
        GameObject cityName;
        Canvas cityInfoCanvas;
        TextMeshPro text;
        RectTransform canvasRectTransform;

        RectTransform rectTransform;

        // Canvas
        go = new GameObject();
        //        go.name = "TestCanvas";
        go.AddComponent<Canvas>();

        cityInfoCanvas = go.GetComponent<Canvas>();
        cityInfoCanvas.renderMode = RenderMode.WorldSpace;
        //    parent.AddComponent<CanvasScaler>();
        //    parent.AddComponent<GraphicRaycaster>();

        go.transform.position = new Vector3(0, -2, -1);
        go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        go.transform.SetParent(parent.transform, false);
        go.transform.rotation = Quaternion.Euler(90, 0, 0);

      

        //        go.transform.siz = new Vector2(10, 5);
        //        go.transform.rotation = Quaternion.Euler(90, 0, 0);
        //        go.transform.parent = parent.transform;

        //        go.transform.position = Util.gpsToVector((float)city.latitude + (0.2f / 4) * 0.3f + 0.01f , (float)city.longitude, radius + 0.1f + city.elevationAdj, true);
        //        go.transform.LookAt(Vector3.zero);
        //        cityInfoCanvas.transform.position = new Vector3(0, -5, 0);
        //       cityInfoCanvas.transform.rotation = Quaternion.Euler(90, 0, 0);
        //        cityInfoCanvas.transform.LookAt(Vector3.zero);
        canvasRectTransform = go.GetComponent<RectTransform>();
        canvasRectTransform.sizeDelta = new Vector2(20, 5);
        //      canvasRectTransform.LookAt(Vector3.zero);

        // Text
        cityName = new GameObject();
        //        cityName.transform.parent = go.transform;
        //cityName.name = "wibble";
        cityName.transform.rotation = Quaternion.Euler(180, 0, 0);

        text = cityName.AddComponent<TextMeshPro>();
        LocalizedString localizeString = new LocalizedString() { TableReference = "City Name", TableEntryReference = city.name };
        text.font = fontAsset;
        text.text = localizeString.GetLocalizedString();
        text.outlineWidth = 0.1f;


        // Text position
        rectTransform = text.GetComponent<RectTransform>();
//        rectTransform.localPosition = new Vector3(0, -5, 0);
     //   rectTransform.rotation = Quaternion.Euler(180, 0, 0);

        rectTransform.sizeDelta = new Vector2(20, 5);
        rectTransform.localScale = new Vector3(1, 1, 1);

        ContentSizeFitter contentSizeFitter = cityName.AddComponent<ContentSizeFitter>();
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        cityName.transform.SetParent(go.transform, false);

    }
}
