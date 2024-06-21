using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Elevation : MonoBehaviour
{

    public TextAsset jsonFile;

    public static Elevation instance = null;

    public static int[,] elevation = new int[181, 361];

    static int MAP_HEIGHT = 1024; // magic number refers to resolution of map
    static int MAP_WIDTH = 2048;

    public static float[] heightMap;

    private void Awake()
    {
        if (null == instance)
        {
            DontDestroyOnLoad(this.gameObject);

          //  Debug.Log(jsonFile.text);
            
            Texture2D heightTexture = Resources.Load<Texture2D>("World_elevation_map");

            Color[] colorMap = heightTexture.GetPixels();
            Debug.Log(heightTexture.width + "," + heightTexture.height);
            MAP_HEIGHT = heightTexture.height;
            MAP_WIDTH = heightTexture.width;
            // Initialize height map 
            heightMap = new float[colorMap.Length];
            Debug.Log(colorMap.Length);

            // Loop through each pixel and convert to a height
            for (int y = 0, index = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++, index++)
                {

                    Color pixelColor = colorMap[index];

                    float height = pixelColor.r; // color stored in alpha channel

               //     Debug.Log(pixelColor);


                    heightMap[index] = height; // might need to apply some sort of transform
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
           


    }

    public static Elevation Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    public static float getElevation(float lat,float lng)
    {

     

        float height = heightMap[GetClosestPixelIndex(lat, lng)];

        //        Debug.Log(height);
        float el = ((height - 0.5f) * 10000);
        //Debug.Log(el);

        return el;
    }
    public static int  GetClosestPixelIndex(float lat, float lng)
    {
         int resolution = 180;

        Vector2 percent = new Vector2((lng+180) / (resolution * 2), (lat+90) / (resolution));
        int x = Mathf.FloorToInt(percent.x * MAP_WIDTH);
        int y = Mathf.FloorToInt(percent.y * MAP_HEIGHT);

        int index = x + y * MAP_WIDTH;
    
       // Debug.Log(index);
        return index;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
