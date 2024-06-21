using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public abstract class ZoomUI : MonoBehaviour
{
    public GameState gameState;


    private UIDocument _doc;  // 요 스크립트와 같은 게임 오브젝트에 있는 UI Document 컴포넌트 할당용
    private Button _zoomInButton;
    private Button _zoomOutButton;

    public GameObject MainCamera;


    public void Init()
    {
        _doc = GetComponent<UIDocument>();

        // 각 버튼의 가져옴
        _zoomInButton = _doc.rootVisualElement.Q<Button>("ZoomInButton");
        _zoomOutButton = _doc.rootVisualElement.Q<Button>("ZoomOutButton");

        // 콜백 연결
        _zoomInButton.clicked += ZoomInButtonClicked;
        _zoomOutButton.clicked += ZoomOutButtonClicked;

    }

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

            if (Physics.Raycast(ray, out hit, 100))
            {
                //              Debug.Log(hit);

                CityObject cityObject = hit.transform.GetComponent<CityObject>();
                //                Debug.Log(cityObject);
                City city = cityObject.GetCity();
                if (city != null)
                {
                    Debug.Log(city.name);

                    cityClicked(city);

                };

                //    Debug.DrawLine(ray.origin, hit.point);
            }

        }
    }

    public abstract void cityClicked(City city);


    private void ZoomInButtonClicked()
    {
        Debug.Log("ZoomInButtonClicked");
        CameraHandler cameraHandler = MainCamera.GetComponent<CameraHandler>();
        cameraHandler.Zoom(0.2f);
    }

    private void ZoomOutButtonClicked()
    {
        Debug.Log("ZoomOutButtonClicked");
        CameraHandler cameraHandler = MainCamera.GetComponent<CameraHandler>();
        cameraHandler.Zoom(-0.3f);

    }
    
}
