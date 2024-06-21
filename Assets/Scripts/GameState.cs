using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { START, CHOOSE_FACTION,IN_GAME,MAIN_MENU,POPUP_MENU };

[CreateAssetMenu(fileName ="GameState",menuName = "Scriptable Objects/Game State")]
public class GameState : ScriptableObject
{    
    public State state = State.START;
}
