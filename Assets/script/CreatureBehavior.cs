using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameComponent;

public class CreatureBehavior : MonoBehaviour
{
    private Vector3 originPos;
    private IGameCondition gameCon;
    public bool onLeftShore, onBoat,posOnBoat;

    void Start()
    {
        originPos = this.transform.position;
        onBoat = false;
        onLeftShore = true;
        gameCon = MainSceneController.getInstance() as IGameCondition;
        posOnBoat = false;
    }

    void Update()
    {
        
    }

    public void getOnBoat(bool onable,Vector3 emptyPos,bool pos)
    {   
        if (onable)
        {
            this.transform.position = emptyPos;
            
            if (this.tag == "Priest")
            {
                gameCon.status_PriestOn(onLeftShore);
            }
            else
            {
                gameCon.status_DevilOn(onLeftShore);
            }
            onBoat = true;
            posOnBoat = pos;
        }
    }

    public bool getOffBoat(bool onable,Vector3 emptyPos)
    {
        if (onable)
        {
            this.transform.position = emptyPos;

            if (this.tag == "Priest")
            {
                gameCon.status_PriestOff(onLeftShore);
            }
            else
            {
                gameCon.status_DevilOff(onLeftShore);
            }
            onBoat = false;
            originPos = emptyPos;
        }
        return posOnBoat;
    }

    public Vector3 getOriginPos()
    {
        return originPos;
    }
}
