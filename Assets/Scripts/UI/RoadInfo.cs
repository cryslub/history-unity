using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadInfo : PopupWindowContent
{
    [SerializeField]
    VisualTreeAsset roadInfo;


    Road road;
    public RoadInfo(Road road)
    {
        this.road = road;
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

        var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/RoadInfo.uxml");
        visualTreeAsset.CloneTree(editorWindow.rootVisualElement);

        VisualElement root = editorWindow.rootVisualElement;

        Dictionary<string, City> cities = DataManager.cities;

        var start = root.Q<Label>("Start");
        start.text = cities[road.start.ToString()].name;

        var end = root.Q<Label>("End");
        end.text = cities[road.end.ToString()].name;

        root.Q<TextField>("Waypoint").value =road.waypoint;
        root.Q<TextField>("ElevationAdj").value = road.elevationAdj;

        root.Q<Button>("Save").clicked += Save;

    }

    void Redraw(){
        DataManager.Instance.Draw();
    }

    void ExportCity()
    {
    //    Debug.Log(DataManager.cityMap.Count);

        string data = JsonConvert.SerializeObject(DataManager.cities);
       // Debug.Log(data);
        
        System.IO.File.WriteAllText(Application.persistentDataPath + "/city.json", data);

    }

    void ExportRoad()
    {
        //    Debug.Log(DataManager.cityMap.Count);

        string data = JsonConvert.SerializeObject(DataManager.roads);
        // Debug.Log(data);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/road.json", data);

    }

    void Save()
    {
        VisualElement root = editorWindow.rootVisualElement;

        road.waypoint = root.Q<TextField>("Waypoint").value;
        road.elevationAdj = root.Q<TextField>("ElevationAdj").value ;

    }

    public override void OnClose()
    {
    

    }
}
