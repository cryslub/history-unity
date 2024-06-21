using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapbox;
using System.Linq;

public class BiomeDrawer : GeoJsonDrawer
{
     public int[] BIOME;
     public int GBL_STAT;

     public int ECO_NUM;

    public int G200_STAT;

 
    public override void IterateJson(dynamic root,List<CombineInstance> combines){

        foreach(dynamic feature in root.features){
            dynamic properties = feature.properties;
            if(feature.geometry!=null && properties != null){

                if (properties.BIOME == null)
                {
                    Draw(feature, combines);
                }
                else
                {
                    if (ArrayUtility.Contains(BIOME, (int)properties.BIOME) )
                    {
                        if (GBL_STAT != 0 && properties.GBL_STAT != GBL_STAT) continue;
                        if (ECO_NUM != 0 && properties.ECO_NUM != ECO_NUM) continue;
                        if (G200_STAT != 0 && properties.G200_STAT != G200_STAT) continue;

                        Draw(feature, combines);
                    }
                }
            }

        }
    }

    public virtual void Draw(dynamic feature, List<CombineInstance> combines)
    {
        DrawGeometry(feature.geometry, combines);
    }


}
