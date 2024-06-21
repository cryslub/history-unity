using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class Scenario
{
    public int id;
    public int year;
}

public class City :  ICloneable
{
    public int id;
    public int yn;
    public String originalName;
    public String labelPosition;
    public int population;
    public int faction;
    public int snapshot;
    public string color;
    public string type;
    public float latitude;
    public float longitude;
    public bool activated = false;
    public string name;
    public float elevationAdj;
    public List<Road> roads = new List<Road>();

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class Snapshot
{
    public int id;
    public int? year;
    public int? population;
    public int faction;
    public string name;
}

public class Faction
{
    public int id;
    public string color;
    public string name;
}

public class Road
{
    public int id;
    public int start;
    public int end;
    public string waypoint;
    public string elevationAdj;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;
    public GameObject citiesObject;
    public GameObject roadsObject;

    public TextAsset scenarioCityFile;
    public TextAsset scenarioRoadFile;

    public TextAsset cityFile;
    public TextAsset snapshotFile;
    public TextAsset factionFile;
    public TextAsset scenarioFile;

    public TextAsset roadFile;


    public static Scenario selectedScenario;

    public static Dictionary<string, List<int>> scenarioCities;
    public static Dictionary<string,List<int>> scenarioRoads;

    public static Dictionary<string, City> cities;
    public static Dictionary<string, City> cityMap = new Dictionary<string, City>();

    public static Dictionary<string, List<Snapshot>> snapshots;
    public static Dictionary<int, Snapshot> snapshotMap = new Dictionary<int, Snapshot>();

    public static List<Faction> factions;
    private List<Scenario> scenarios;
    public static Dictionary<string,Road> roads;


    public static Dictionary<int, Faction> factionMap = new Dictionary<int, Faction>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            ReadJsonFiles();
            Draw();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public void Draw()
    {
        MakeMaps();

        SelectScenario(scenarios[0]);

    }

    void ReadJsonFiles()
    {
        scenarioCities = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(scenarioCityFile.text);
        scenarioRoads = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(scenarioRoadFile.text);

        cities = JsonConvert.DeserializeObject<Dictionary<string, City>>(cityFile.text);
        snapshots = JsonConvert.DeserializeObject<Dictionary<string, List<Snapshot>>>(snapshotFile.text);
        factions = JsonConvert.DeserializeObject<List<Faction>>(factionFile.text);
        scenarios = JsonConvert.DeserializeObject<List<Scenario>>(scenarioFile.text);
        roads = JsonConvert.DeserializeObject<Dictionary<string, Road>>(roadFile.text);

    }

    public  void MakeMaps()
    {
        cityMap = new Dictionary<string, City>();
        foreach (KeyValuePair<string, City> pair in cities)
        {
            if (pair.Value != null)
                cityMap.Add(pair.Key, (City)pair.Value.Clone());
        }


        factionMap = new Dictionary<int, Faction>();
        // factionMap[0] = { "id":0};
        foreach (Faction faction in factions)
        {
            factionMap[faction.id] = faction;
        }

        snapshotMap = new Dictionary<int, Snapshot>();
        foreach (KeyValuePair<string, List<Snapshot>> pair in snapshots)
        {
            foreach(Snapshot snapshot in pair.Value)
            {
                snapshotMap.Add(snapshot.id, snapshot);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public static DataManager Instance
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

    public void SelectScenario(Scenario scenario)
    {
        selectedScenario = scenario;

        List<City> objects = new List<City>();
        //        Debug.Log(((int)scenario.id).ToString());
        foreach (dynamic id in scenarioCities[((int)scenario.id).ToString()])
        {
            string i = ((int)id).ToString();
            City city = cityMap[i];
            city.activated = true;

            // var lastSnapshot;
            foreach (Snapshot snapshot in snapshots[i])
            {

                if (scenario.year >= snapshot.year)
                {
                    if(snapshot.population!=null)
                        city.population = (int)snapshot.population;
                    city.snapshot = snapshot.id;
                    city.faction = snapshot.faction;
                    if(snapshot.name!=null)
                       city.name = snapshot.name;
                    //                    city.civilization = snapshot.civilization
                    int factionId = (int)city.faction;
                 //   Debug.Log(factionId);
                    if (factionId == 0)
                    {
                        city.color = "#ffffff";
                    }
                    else
                    {
                        city.color = factionMap[factionId].color;
                    }
                    //                    city.traits = snapshot.traits == null ? '' : snapshot.traits;
                    //                    city.snapshotSub = snapshotSubs[snapshot.id];
                }
            }

            if (city.population >0 || city.type == "waypoint")
                objects.Add(city);
        }

        addObjects(objects);

        makeRoads(scenario);
    }

    private void makeRoads(Scenario scenario)
    {
        roadsObject.GetComponent<RoadsManager>().ClearRoads();

        foreach (dynamic id in scenarioRoads[scenario.id.ToString()])
        {
            string idStr = ((int)id).ToString();
            if (roads.ContainsKey(idStr))
            {
                Road road = roads[idStr];

                string startKey = road.start.ToString();
                string endKey = road.end.ToString();

                if (cityMap.ContainsKey(startKey) && cityMap.ContainsKey(endKey) )
                {
                    City start = cityMap[startKey];
                    City end = cityMap[endKey];

                    if(start.activated && end.activated)
                    {
                        if(!start.roads.Contains(road))
                            start.roads.Add(road);
                        if (!end.roads.Contains(road))
                            end.roads.Add(road);

                        List<List<float>> coordinates = new List<List<float>>();
                        List<float> elevationAdjs = new List<float>();
                        
                        coordinates.Add(new List<float>(new float[] { start.longitude, start.latitude }));
                        elevationAdjs.Add(start.elevationAdj);
                        if (road.waypoint != "" && road.waypoint != null)
                        {
                            List<List<float>> waypoints = JsonConvert.DeserializeObject<List<List<float>>>(road.waypoint);
                            List<float> elAdjs = new List<float>(); ;
                            if (road.elevationAdj!= null)
                                elAdjs = JsonConvert.DeserializeObject<List<float>>(road.elevationAdj);
                            foreach (List<float> waypoint in waypoints)
                            {
                                coordinates.Add(waypoint);
                                if(elAdjs!=null)
                                    if (elAdjs.Count==0)
                                    {
                                        elevationAdjs.Add(0f);
                                    }
                            }
                            if (elAdjs != null)
                                elevationAdjs.AddRange(elAdjs);

                            //                            coordinates.AddRange(JsonConvert.DeserializeObject<List<List<float>>>(road.waypoint));
                        }
                        coordinates.Add(new List<float>(new float[] { end.longitude, end.latitude }));
                        elevationAdjs.Add(end.elevationAdj);

                        roadsObject.GetComponent<RoadsManager>().DrawRoad(coordinates, elevationAdjs);

                    }

                }
            }
        }

    }

    private  void addObjects(List<City> objects)
    {
        ///        clearObjects();
        citiesObject.GetComponent<CitiesManager>().addObjects(objects);
    }


}