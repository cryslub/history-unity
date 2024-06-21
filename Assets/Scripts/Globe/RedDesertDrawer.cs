using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Drawing;

public class RedDesertDrawer : BiomeDrawer
{


    public override void Draw(dynamic feature, List<CombineInstance> combines)
    {
        dynamic properties = feature.properties;

        bool draw = false;

        if (properties.GBL_STAT == 1)
        {
            draw = true;
            if (properties.G200_STAT == 2)
            {
                draw = false;
            }
        }
        if (properties.GBL_STAT == 2)
        {
            if (properties.G200_STAT == 3)
            {
                draw = true;
            }
        }

        if (feature.properties.ECO_NUM == 1)
        {
            draw = false;

        }

        if (draw) DrawGeometry(feature.geometry, combines);
    }


}
