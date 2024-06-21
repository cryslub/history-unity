using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using UnityEditor;
using PopupWindow = UnityEditor.PopupWindow;

public class MainUI : ZoomUI
{

    public GameObject stateManagerObject;

    private UIDocument _doc;  // 요 스크립트와 같은 게임 오브젝트에 있는 UI Document 컴포넌트 할당용

    private VisualElement _detailElement;

    private Button _mainMenuButton;

    private Button _closeDetailButton;
    private Label _cityNameLabel;
    private VisualElement _facitonColorElement;


    private Button _tempUnitButton;

    private StateManager stateManager;

    private void Awake()
    {

        base.Init();

        _doc = GetComponent<UIDocument>();

        _mainMenuButton = _doc.rootVisualElement.Q<Button>("MainMenuButton");

        _mainMenuButton.clicked += MainMenuButtonClicked;


        _detailElement = _doc.rootVisualElement.Q<VisualElement>("DetailElement");
        _facitonColorElement = _doc.rootVisualElement.Q<VisualElement>("FactionColorElement");

        _cityNameLabel = _doc.rootVisualElement.Q<Label>("CityNameLabel");
        _closeDetailButton = _doc.rootVisualElement.Q<Button>("CloseDetailButton");

        _closeDetailButton.clicked += CloseDetailButtonClicked;

        _tempUnitButton = _doc.rootVisualElement.Q<Button>("TempUnitButton");
        _tempUnitButton.clicked += TempUnitButtonClicked;


        _detailElement.style.display = DisplayStyle.None;

        this.stateManager = stateManagerObject.GetComponent<StateManager>();
    }


    public override void cityClicked(City city)
    {
        if (gameState.state == State.IN_GAME)
        {
            _detailElement.style.display = DisplayStyle.Flex;

            LocalizedString localizeString = new LocalizedString() { TableReference = "City Name", TableEntryReference = city.name };

            _cityNameLabel.text = localizeString.GetLocalizedString();


            Faction faction = DataManager.factionMap[city.faction];

            UnityEngine.Color customColor;
            UnityEngine.ColorUtility.TryParseHtmlString(faction.color, out customColor);

            _facitonColorElement.style.backgroundColor = customColor;
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

    private void CloseDetailButtonClicked()
    {
        _detailElement.style.display = DisplayStyle.None;
    }

    
    private void TempUnitButtonClicked()
    {
        
        stateManager.SetState(State.POPUP_MENU);

    }

    private void MainMenuButtonClicked()
    {
        Debug.Log("MainMenuButtonClicked");
        stateManager.SetState(State.MAIN_MENU);
    }

    

}
