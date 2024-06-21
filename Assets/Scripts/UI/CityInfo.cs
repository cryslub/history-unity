using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;
public class CityInfo : PopupWindowContent
{
    [SerializeField]
    VisualTreeAsset roadInfo;

    Dictionary<int,TextField> waypointTextFields = new Dictionary<int,TextField>();
    Dictionary<int, TextField> elevationAdjTextFields = new Dictionary<int, TextField>();

    Dictionary<int, int> cityIndexMap= new Dictionary<int, int>();
    Rect buttonRect;

    City city;

    public CityInfo(City city)
    {
        this.city = city;
    }
    // Start is called before the first frame update
    public override Vector2 GetWindowSize()
    {
        return new Vector2(600, 500);
    }

    public override void OnGUI(Rect rect)
    {
        // Intentionally left empty
       
    }

    public override void OnOpen()
    {
        Debug.Log("Popup opened: " + this);

        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/CityInfo.uxml");
        visualTreeAsset.CloneTree(editorWindow.rootVisualElement);

        VisualElement root = editorWindow.rootVisualElement;
        root.Q<Label>("Name").text = city.name;
        root.Q<FloatField>("Latitude").value = city.latitude;
        root.Q<FloatField>("Longitude").value = city.longitude;
        root.Q<FloatField>("ElevationAdj").value = city.elevationAdj;
        root.Q<IntegerField>("Population").value = city.population;


        root.Q<Button>("Save").clicked += Save;
        root.Q<Button>("ExportCity").clicked += ExportCity;
        root.Q<Button>("ExportRoad").clicked += ExportRoad;
        root.Q<Button>("AddRoad").clicked += AddRoad;

        root.Q<Button>("Redraw").clicked += Redraw;

       roadInfo = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/RoadItem.uxml");


        Func<VisualElement> makeItem = () => roadInfo.Instantiate();

        Dictionary<string,City> cities = DataManager.cities;

        List<TextField> list = new List<TextField>();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
//            Debug.Log(i);
            Road road = city.roads[i];
            var start = e.Q<Label>("Start");
            start.text = cities[road.start.ToString()].name;

            var end = e.Q<Label>("End");
            end.text = cities[road.end.ToString()].name;

            Button edit = e.Q<Button>("Edit");
            edit.clicked += () =>
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(edit.worldTransform.GetPosition());
               // Rect menuRect = new Rect(screenPos.x, screenPos.y - 100, 600, 500);
                PopupWindow.Show(buttonRect, new RoadInfo(road));
            };

            Button delete = e.Q<Button>("Delete");
            delete.clicked += () =>
            {
                DataManager.scenarioRoads[DataManager.selectedScenario.id.ToString()].Remove(road.id);
            };
        };

        var listView = root.Q<ListView>();
        listView.makeItem = makeItem;
        listView.bindItem = bindItem;
        listView.itemsSource = city.roads;


        var choices = new List<string>();

        List<City> scenarioCities = new List<City>();
        foreach (int id in DataManager.scenarioCities[DataManager.selectedScenario.id.ToString()])
        {
//            Debug.Log(DataManager.scenarioCities);
            if (cities.ContainsKey(id.ToString()))
            {
                City city = cities[id.ToString()];
                scenarioCities.Add(city);
            }

        }

        scenarioCities.Sort(delegate (City x, City y)
        {
            if (x.name == null && y.name == null) return 0;
            else if (x.name == null) return -1;
            else if (y.name == null) return 1;
            else return x.name.CompareTo(y.name);
        });

        int i = 0;
        foreach (City city in scenarioCities)
        {
            choices.Add(city.name);
            //                Debug.Log(i + "," + city.name);
            cityIndexMap.Add(i, city.id);
            i++;

        }


        // Get a reference to the dropdown field from UXML and assign a value to it.
        DropdownField cityList = root.Q<DropdownField>("CityList");
        
        cityList.choices = choices;
        cityList.index = 0;
    }

    void Redraw(){
        DataManager.Instance.Draw();
    }

    void ExportCity()
    {
    //    Debug.Log(DataManager.cityMap.Count);

        string data = JsonConvert.SerializeObject(DataManager.cities);
        
        System.IO.File.WriteAllText(Application.persistentDataPath + "/city.json", data);
         Debug.Log(Application.persistentDataPath);

        data = JsonConvert.SerializeObject(DataManager.snapshots);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/snapshot.json", data);

    }

    void ExportRoad()
    {
        //    Debug.Log(DataManager.cityMap.Count);

        string data = JsonConvert.SerializeObject(DataManager.roads);
        // Debug.Log(data);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/road.json", data);

        data = JsonConvert.SerializeObject(DataManager.scenarioRoads);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/scenarioRoad.json", data);
        Debug.Log(Application.persistentDataPath);
    }

    void AddRoad()
    {
        //    Debug.Log(DataManager.cityMap.Count);
        Road road = new Road();
        road.start = city.id;

        DropdownField cityList = editorWindow.rootVisualElement.Q<DropdownField>("CityList");
        Debug.Log(cityList.index);
        Debug.Log(cityIndexMap);
        City end = DataManager.cities[cityIndexMap[ cityList.index].ToString()];
        Debug.Log(end.name);

        road.end = end.id;

        int index = Int32.Parse(DataManager.roads.Keys.Last())+1;
        DataManager.roads.Add(index.ToString(),road);

        DataManager.scenarioRoads[DataManager.selectedScenario.id.ToString()].Add(index);
    }

    void Save()
    {
        VisualElement root = editorWindow.rootVisualElement;

        city.latitude = root.Q<FloatField>("Latitude").value;
        city.longitude = root.Q<FloatField>("Longitude").value;
        city.population = root.Q<IntegerField>("Population").value;

        City c = DataManager.cities[city.id.ToString()];
        c.latitude = root.Q<FloatField>("Latitude").value;
        c.longitude = root.Q<FloatField>("Longitude").value;
        c.elevationAdj = root.Q<FloatField>("ElevationAdj").value;
      //  c.population = root.Q<IntegerField>("Population").value;


        for (int i =0;i< city.roads.Count;i++) 
        {
            Road road = city.roads[i];
            if (waypointTextFields.ContainsKey(road.id) )
            {
//                Debug.Log(road.id+","+road.waypointTextField.value);
                road.waypoint = waypointTextFields[road.id].value;
              
            }

            if (elevationAdjTextFields.ContainsKey(road.id))
            {
                road.elevationAdj = elevationAdjTextFields[road.id].value;
            }
        }

        Debug.Log(city.snapshot);
        Snapshot snapshot = DataManager.snapshotMap[city.snapshot];
        snapshot.population = city.population;


     //   foreach(Snapshot s in DataManager.snapshots[city.id.ToString()])
     //   {
     //       Debug.Log(s.id+","+s.population);
      //  }
        
    }

    public override void OnClose()
    {
        waypointTextFields = new Dictionary<int, TextField>();
        elevationAdjTextFields = new Dictionary<int, TextField>();

    }
}
