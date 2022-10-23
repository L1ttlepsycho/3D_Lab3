using GameComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{   

    public bool isMoving;
    public bool onLeftShore;
    public bool l_empty,r_empty;

    private IGameCondition gameCon;
    private LOCATIONS loc=new LOCATIONS();
    private Vector3 direction=new Vector3(0.1f,0,0);
    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        onLeftShore = true;
        l_empty = true;
        r_empty = true;

        gameCon = MainSceneController.getInstance() as IGameCondition;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private bool isMovingToEdge()
    {
        if (direction.x < 0 && this.transform.position.x <= loc.boat_dst_l.x)
        {
            gameCon.isOver();
            isMoving = false;
            onLeftShore = true;
            direction = new Vector3(-direction.x, 0, 0);
            return true;
        }
        //right
        else if (direction.x > 0 && this.transform.position.x >= loc.boat_dst_r.x)
        {
            gameCon.isOver();
            isMoving = false;
            onLeftShore = false;
            direction = new Vector3(-direction.x, 0, 0);
            return true;
        }
        //on the move
        else
        {
            return false;
        }
    }


    private void move()
    {
        if (isMoving)
        {
            if (!isMovingToEdge())
            {
                this.transform.Translate(direction);
            }
        }
    }

    public void boatMove()
    {
        if (!isMoving && (!l_empty || !r_empty))
        {
            isMoving = true;
        }
    }
    public bool isBoatAtLeftSide()
    {
        return onLeftShore;
    }


    public bool isLeftSeatEmpty()
    {
        return l_empty;
    }
    public bool isRightSeatEmpty()
    {
        return r_empty;
    }


    public void sitOnPos(bool isLeft)
    {
        if (isLeft)
            l_empty = false;
        else
            r_empty = false;
    }
    public void getOffPos(bool isLeft)
    {
        if (isLeft)
            l_empty = true;
        else
            r_empty = true;
    }

}
