using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Drawing;

public class BarrenDesertDrawer : BiomeDrawer
{


    public override void Draw(dynamic feature, List<CombineInstance> combines)
    {
        dynamic properties = feature.properties;

        bool draw = false;

        if (properties.GBL_STAT == 1)
        {
            if (properties.G200_STAT == 2)
            {
                draw = true;
            }
        }

        if (properties.GBL_STAT == 3)
        {
            draw = true;
        }


        if (draw) DrawGeometry(feature.geometry, combines);
    }


}
