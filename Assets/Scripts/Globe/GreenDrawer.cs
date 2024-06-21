using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Drawing;

public class GreenDrawer : BiomeDrawer
{


    public override void Draw(dynamic feature, List<CombineInstance> combines)
    {
        dynamic properties = feature.properties;

        bool draw = true;

        if (feature.properties.GBL_STAT == 1)
        {
            //console.log(feature.properties);
            if (feature.properties.G200_STAT == 1)
            {
                draw = false;
            }

        }
        if (feature.properties.GBL_STAT == 2)
        {
            draw = false;

        }
        if (feature.properties.GBL_STAT == 3)
        {
            draw = false;

        }



        if (draw) DrawGeometry(feature.geometry, combines);
    }


}
