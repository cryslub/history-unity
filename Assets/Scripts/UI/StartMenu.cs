using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _resumeButton;
    private Button _newGameButton;
    private Button _continueButton;
    private Button _loadButton;
    private Button _saveButton;


    public GameObject stateManagerObject;

    private StateManager stateManager;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();

        _resumeButton = _doc.rootVisualElement.Q<Button>("ResumeButton");
        _newGameButton = _doc.rootVisualElement.Q<Button>("NewGameButton");
        _continueButton = _doc.rootVisualElement.Q<Button>("ContinueButton");
        _loadButton = _doc.rootVisualElement.Q<Button>("LoadButton");
        _saveButton = _doc.rootVisualElement.Q<Button>("SaveButton");

        _resumeButton.clicked += ResumeButtonClicked;
        _newGameButton.clicked += NewGameButtonClicked;
        _continueButton.clicked += ContinueButtonClicked;
        _loadButton.clicked += LoadButtonClicked;
        _saveButton.clicked += SaveButtonClicked;

        _resumeButton.visible = false;
        _saveButton.visible = false;

        this.stateManager = stateManagerObject.GetComponent<StateManager>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResumeButtonClicked()
    {
        Debug.Log("ResumeButtonClicked");
      
    }

    private void NewGameButtonClicked()
    {
        Debug.Log("NewGameButtonClicked");
        stateManager.SetState(State.CHOOSE_FACTION);
    }

    private void ContinueButtonClicked()
    {
        Debug.Log("ContinueButtonClicked");

    }

    private void LoadButtonClicked()
    {
        Debug.Log("LoadButtonClicked");

    }

    private void SaveButtonClicked()
    {
        Debug.Log("SaveButtonClicked");

    }

}
