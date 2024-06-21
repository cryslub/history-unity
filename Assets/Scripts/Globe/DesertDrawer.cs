using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Drawing;

public class DesertDrawer : BiomeDrawer
{


    public override void Draw(dynamic feature, List<CombineInstance> combines)
    {
        dynamic properties = feature.properties;

        bool draw = true;

        if (feature.properties.GBL_STAT == 1)
        {
            draw = false;
        }
        if (feature.properties.GBL_STAT == 2)
        {
            if (feature.properties.G200_STAT == 3)
            {
                draw = false;
            }
        }
        if (feature.properties.GBL_STAT == 3)
        {
            draw = false;

        }

        if (feature.properties.ECO_NUM == 1)
        {
            draw = false;

        }
        if (feature.properties.ECO_NUM == 3)
        {
            draw = true;

        }



        if (draw) DrawGeometry(feature.geometry, combines);
    }


}
