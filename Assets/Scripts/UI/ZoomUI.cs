using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public abstract class ZoomUI : MonoBehaviour
{
    public GameState gameState;


    private UIDocument _doc;  // �� ��ũ��Ʈ�� ���� ���� ������Ʈ�� �ִ� UI Document ������Ʈ �Ҵ��
    private Button _zoomInButton;
    private Button _zoomOutButton;

    public GameObject MainCamera;


    public void Init()
    {
        _doc = GetComponent<UIDocument>();

        // �� ��ư�� ������
        _zoomInButton = _doc.rootVisualElement.Q<Button>("ZoomInButton");
        _zoomOutButton = _doc.rootVisualElement.Q<Button>("ZoomOutButton");

        // �ݹ� ����
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
