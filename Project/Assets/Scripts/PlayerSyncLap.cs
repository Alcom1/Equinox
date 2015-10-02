using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSyncLap : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerLapSynced")]
    private int lap;                        //Lap

    private int aLap;                       //Lap

    //Displays lap on each update. Pretty inefficient, could add some boolena states to block the repetition.
    void Update()
    {
        DisplayLap();
    }

    //Changes the Opponent's lap display to be the opponent's lap.
    void DisplayLap()
    {
        if (!isLocalPlayer)
        {
            GameObject.Find("TextLapOpponent").GetComponent<Text>().text = "Opponent's Lap: " + lap;
        }
    }

    //Server set new lap
    [Command]
    void CmdSendNewLapToServer(int newLap)
    {
        lap = newLap;
    }

    //Transmit new lap to server
    [Client]
    public void TransmitLap(int newLap)
    {
        if (isLocalPlayer && CheckIfNewLap(aLap, newLap))
        {
            aLap = newLap;
            CmdSendNewLapToServer(newLap);
        }
    }

    //Check if new lap is different from old
    bool CheckIfNewLap(int lap1, int lap2)
    {
        if (lap1 == lap2)
        {
            return false;
        }
        return true;
    }

    //Sync laps
    [Client]
    void OnPlayerLapSynced(int aLap)
    {
        lap = aLap;
    }
}