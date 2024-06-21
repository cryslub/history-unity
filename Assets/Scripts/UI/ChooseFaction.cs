using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class ChooseFaction : ZoomUI
{

    private UIDocument _doc;  // �� ��ũ��Ʈ�� ���� ���� ������Ʈ�� �ִ� UI Document ������Ʈ �Ҵ��
    private Button _backButton;
    private Button _startButton;

    private Label _factionNameLabel;


    public GameObject stateManagerObject;
    public GameObject gameManagerObject;

    private StateManager stateManager;
    private GameManager gameManager;

    private VisualElement _topElement;
    private VisualElement _bottomElement;
    private VisualElement _facitonColorElement;

    private Faction selectedFaction;

    private void Awake()
    {

        base.Init();

        _doc = GetComponent<UIDocument>();

        // �� ��ư�� ������
        _backButton = _doc.rootVisualElement.Q<Button>("BackButton");
        _startButton = _doc.rootVisualElement.Q<Button>("StartButton");


        _topElement = _doc.rootVisualElement.Q<VisualElement>("TopElement");
        _bottomElement = _doc.rootVisualElement.Q<VisualElement>("BottomElement");
        _facitonColorElement = _doc.rootVisualElement.Q<VisualElement>("FactionColorElement");

        _factionNameLabel = _doc.rootVisualElement.Q<Label>("FactionNameLabel");


        _bottomElement.style.display = DisplayStyle.None;

        _backButton.clicked += BackButtonClicked;
        _startButton.clicked += StartButtonClicked;

        this.stateManager = stateManagerObject.GetComponent<StateManager>();
        this.gameManager = gameManagerObject.GetComponent<GameManager>();

 //       Debug.Log(gameManager);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void cityClicked(City city)
    {
        if (gameState.state == State.CHOOSE_FACTION)
            SetSelectedFaction(city);

    }

    private void SetSelectedFaction(City city)
    {
        _bottomElement.style.display = DisplayStyle.Flex;
        Faction faction = DataManager.factionMap[city.faction];

        if (city.faction == 0)
        {
            LocalizedString localizeString = new LocalizedString() { TableReference = "UI", TableEntryReference = "No Faction" };

            _factionNameLabel.text = localizeString.GetLocalizedString();
            _facitonColorElement.style.backgroundColor = Color.white;
            _startButton.style.display = DisplayStyle.None;
        }
        else
        {

            LocalizedString localizeString = new LocalizedString() { TableReference = "City Name", TableEntryReference = faction.name };

            _factionNameLabel.text = localizeString.GetLocalizedString();

            UnityEngine.Color customColor;
            UnityEngine.ColorUtility.TryParseHtmlString(faction.color, out customColor);

            _facitonColorElement.style.backgroundColor = customColor;
            _startButton.style.display = DisplayStyle.Flex;

            selectedFaction = faction;

        }
    }

   
    private void BackButtonClicked()
    {
        Debug.Log("BackButtonClicked");

        stateManager.Back();

    }

    private void StartButtonClicked()
    {
        Debug.Log("StartButtonClicked");

        _topElement.style.display = DisplayStyle.None;
        _bottomElement.style.display = DisplayStyle.None;

        stateManager.SetState(State.IN_GAME);

        Debug.Log(gameManager);
        this.gameManager.StartGame(selectedFaction);
      
    }
}
