using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoupMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _resumeButton;
    private Button _newGameButton;
    private Button _continueButton;
    private Button _loadButton;
    private Button _closeButton;


    public GameObject stateManagerObject;

    private StateManager stateManager;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();

        _closeButton = _doc.rootVisualElement.Q<Button>("CloseButton");

        _closeButton.clicked += CloseButtonClicked;


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

    private void CloseButtonClicked()
    {
        Debug.Log("CloseButtonClicked");
        stateManager.Back();

    }

}
