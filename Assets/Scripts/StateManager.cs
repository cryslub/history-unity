using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class StateManager : MonoBehaviour
{

    [SerializeField]
    public GameState gameState;

    [SerializeField]
    public GameObject startMenuObject;

    [SerializeField]
    public GameObject chooseFactionObject;

    [SerializeField]
    public GameObject mainUIObject;


    public GameObject popupMenuObject;


    private StartMenu startMenu;
    private ChooseFaction chooseFaction;
    private MainUI mainUI;

    private PoupMenu popupMenu;


    private UIDocument startMenuDocument;
    private UIDocument chooseFactionDocument;
    private UIDocument mainUIDocument;
    private UIDocument popupMenuDocument;


    private List<UIDocument> documentList = new List<UIDocument>();

    Stack<State> stateStack = new Stack<State>();

    // Start is called before the first frame update
    void Start()
    {

        //  mainUIObject.SetActive(false);
        startMenu = chooseFactionObject.GetComponent<StartMenu>();
        chooseFaction = chooseFactionObject.GetComponent<ChooseFaction>();
        mainUI = mainUIObject.GetComponent<MainUI>();
        popupMenu = popupMenuObject.GetComponent<PoupMenu>();


        startMenuDocument = startMenuObject.GetComponent<UIDocument>();
        chooseFactionDocument = chooseFactionObject.GetComponent<UIDocument>();
        mainUIDocument = mainUIObject.GetComponent<UIDocument>();
        popupMenuDocument = popupMenuObject.GetComponent<UIDocument>();

        documentList.Add(startMenuDocument);
        documentList.Add(chooseFactionDocument);
        documentList.Add(mainUIDocument);
        documentList.Add(popupMenuDocument);


        //        SetVisible(chooseFactionDocument, false);

        SetState(State.START);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back()
    {
        stateStack.Pop();
        State state = stateStack.Pop();
        Debug.Log(state.ToString());
        SetState(state);
    }

    public void SetState(State state,string type="")
    {

        stateStack.Push(state);
        gameState.state = state;

        hideAllDocument();

        switch (state){
            case State.START:
                HandleStart();
                break;
            case State.CHOOSE_FACTION:
                HandleChooseFaction();
                break;
            case State.IN_GAME:
                HandleInGame();

                break;
            case State.MAIN_MENU:
                HandleMainMenu(type);
                break;
            case State.POPUP_MENU:
                HandlePopupMenu(type);
                break;
                
        }
    }

    private void hideAllDocument()
    {
        foreach (UIDocument document in documentList)
        {
            SetVisible(document, false);
        }
    }

    private void HandleChooseFaction()
    {

      

        SetVisible(chooseFactionDocument, true);


    }

    private void HandleStart()
    {

        SetVisible(startMenuDocument, true);


    }
    private void HandleInGame()
    {

        SetVisible(mainUIDocument, true);
       
    }

    private void HandleMainMenu(string type)
    {

        SetVisible(startMenuDocument, true);

    }


    private void HandlePopupMenu(string type)
    {

        SetVisible(popupMenuDocument, true);

    }

    

    private void SetVisible(UIDocument document,bool state)
    {
        if (state)
        {
            document.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            document.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
