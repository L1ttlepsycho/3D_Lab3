# Unity"简单"实现flash小游戏 "Priests and Devils"
## 游戏分析
### 游戏提及事物
Priest X 3；

Devil X 3；

一条河；

船；

项目 | 条件 | 动作
---|---|---
下船|船已经靠岸并且船上有对象|对象下船
上船|船已经靠岸并且岸上有对象，船有空间|对象上船
开船|船靠岸状态下并且船上至少有一个人|船带着对象一起走
结束状态|任意一边Priests比Devils少，或者全部对象过河|游戏结束

## 预制游戏对象
使用了```Capsule```作为Priest和Devil，二者通过颜色加以区分；

使用```Cube```构建小船、河两岸。

预制了用于UI构建的```Canvas```以及```Text```。

![Prefabs](https://github.com/L1ttlepsycho/3D_Lab3/blob/master/Priest%20and%20Devil.sln)

## 代码结构
实现了```MainSceneController```,通过```MainSceneController```对游戏对象接口```CreatureBehavior```,```BoatBehavior```,```IPlayerAction```进行调用,与玩家通过```UserInterface```中实现的UI进行交互。

使用脚本```GenGameObj```进行游戏对象的动态生成。
### ```GameComponent```实现思路
对游戏中重要的类与接口进行实现。 (隐去了大部分具体实现代码，具体代码详见项目文件,下同)
```
namespace GameComponent 
{ 
    public class LOCATIONS 
    {
        
    }

    public interface IPlayerAction
    {
        void boatMove();
        void priestOn();
        void devilOn();
        void priestOff();
        void devilOff();
    }

    public interface IGameCondition
    {
        void status_PriestOn(bool isLeftshore);
        void status_PriestOff(bool isLeftshore);
        void status_DevilOn(bool isLeftshore);
        void status_DevilOff(bool isLeftshore);
        bool isOver();
    }

    public class MainSceneController: System.Object,IPlayerAction,IGameCondition
    {
        private static MainSceneController instance;
        private GenGameObj gameObj;

        private int  lShoreNumDevil, rShoreNumDevil,rShoreNumPriest,lShoreNumPriest,boatPriest,boatDevil;  

        public static MainSceneController getInstance()
        {
            if (instance == null)
                instance = new MainSceneController();
            return instance;
        }

        internal void setMainSceneController(GenGameObj _gameObj)
        {
            gameObj = _gameObj;
            lShoreNumDevil = 3;
            rShoreNumDevil = 0;
            lShoreNumPriest = 3;
            rShoreNumPriest = 0;
            boatDevil = 0;
            boatPriest = 0;
        }

        //IUserAction interfaces
        public void boatMove()
        {
            gameObj.boatMove();
        }
        public void priestOn()
        {
            gameObj.priestGetOn();
        }
        public void devilOn()
        {
            gameObj.devilGetOn();
        }
        public void priestOff()
        {
            gameObj.priestGetOff();
        }
        public void devilOff()
        {
            gameObj.devilGetOff();
        }

        //GameCondition interfaces
        public void status_PriestOn(bool isLeftshore)
        {

        }

        public void status_PriestOff(bool isLeftshore)
        {

        }
        public void status_DevilOn(bool isLeftshore)
        {

        }

        public void status_DevilOff(bool isLeftshore)
        {

        }

        public bool isOver()
        {
           
        }

        public void showGameText(string s)
        {
            
        }
    }
```

### ```CreatureBehavior```实现思路
Priest与Devil都需要实现上下船操作。
```
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

    public void getOnBoat(bool onable,Vector3 emptyPos,bool pos)
    {   
        
    }

    public bool getOffBoat(bool onable,Vector3 emptyPos)
    {
        
    }

    public Vector3 getOriginPos()
    {
        return originPos;
    }
}
```

### ```BoatBehavior```实现思路
这艘船承担着比较重要的角色，既要上下Priests和Devils，还要进行移动。为了让玩家有反悔的余地，我们在船的行驶过程中再去判断游戏状态XD
```
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
```

### UI实现
```
public class UserInterface : MonoBehaviour
{
    IPlayerAction myActions;
    float btnWidth = (float)Screen.width / 6.0f;
    float btnHeight = (float)Screen.height / 6.0f;

    void Start()
    {
        myActions = MainSceneController.getInstance() as IPlayerAction;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(5, 250, btnWidth, btnHeight), "Priest GetOn"))
        {
            myActions.priestOn();
        }
        if (GUI.Button(new Rect(155, 250, btnWidth, btnHeight), "Priest GetOff"))
        {
            myActions.priestOff();
        }
        if (GUI.Button(new Rect(325, 250, btnWidth, btnHeight), "Boat Go!"))
        {
            myActions.boatMove();
        }
        if (GUI.Button(new Rect(505, 250, btnWidth, btnHeight), "Devil GetOn"))
        {
            myActions.devilOn();
        }
        if (GUI.Button(new Rect(655, 250, btnWidth, btnHeight), "Devil GetOff"))
        {
            myActions.devilOff();
        }

    }
}
```
### ```GenGameObj```实现思路
由于```GenGameObj```动态生成各个游戏对象，对象间的交互逻辑全部在此处实现，代码屎山警告⚠
```
public class GenGameObj : MonoBehaviour
{
    public  List<GameObject> priests = new List<GameObject>();
    public List<GameObject> devils = new List<GameObject>()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ;
    public GameObject priest,devil,rightShore, leftShore,boat;

    protected LOCATIONS locs = new LOCATIONS();

    private BoatBehavior boatBehavior;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject p = Instantiate(priest);
            p.name = "Priest " + (i + 1);
            p.tag = "Priest";
            p.transform.position = locs.leftShore_spaces[i];
            p.AddComponent<CreatureBehavior>();
            priests.Add(p);
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject d = Instantiate(devil);
            d.name = "Devil " + (i + 1);
            d.tag = "Devil";
            d.transform.position = locs.leftShore_spaces[i+3];
            d.AddComponent<CreatureBehavior>();
            devils.Add(d);
        }
        rightShore = Instantiate(rightShore);
        rightShore.name = "RightShore";
        rightShore.transform.position = locs.rightShore_loc;

        leftShore = Instantiate(leftShore);
        leftShore.name = "LeftShore";
        leftShore.transform.position = locs.leftShore_loc;

        boat= Instantiate(boat);
        boat.name = "Boat";
        boat.AddComponent<BoatBehavior>();
        boatBehavior = boat.GetComponent<BoatBehavior>();
        boat.transform.position = locs.boat_dst_l;

        MainSceneController.getInstance().setMainSceneController(this);
    }

    public void priestGetOn()
    {
        if (boatBehavior.isMoving) return;
        if (boatBehavior.isBoatAtLeftSide())
        {
            
        }
        else
        { 
            
        }
    }

    public void devilGetOn()
    {
        if (boatBehavior.isMoving) return;
        if (boatBehavior.isBoatAtLeftSide())
        {
            
        }
        else
        {
            
        }
    }

    public void priestGetOff()
    {
        if (boatBehavior.isMoving) return;
        if (boatBehavior.onLeftShore)
        {
            
        }
        else
        {
            
        }

    }

    public void devilGetOff()
    {
        if (boatBehavior.isMoving) return;
        if (boatBehavior.onLeftShore)
        {
            
        }
        else
        {
            
        }

    }

    public void boatMove()
    {   
        boatBehavior.boatMove();
    }

}
```

### 脚本挂载与漫长Debug
```GenGameObj```挂载在```MainCamera```上（毕竟在这款游戏里兼具场记与导演）

```UserInterface```挂载在了空游戏对象上，其他脚本均为自动挂载。

Debug这块最折磨的就是各个游戏对象之间的交互逻辑了，各种千奇百怪的Bug都被我制造出来过，譬如乱飞的牧师、停不下来的船、船在右边也能上去的左岸恶魔……

真的好折磨QAQ
## 演示视频
见项目文件

https://github.com/L1ttlepsycho/3D_Lab3/blob/master/Priest%20and%20Devil%20-%20SampleScene%20-%20Windows%2C%20Mac%2C%20Linux%20-%20Unity%202021.3.8f1c1%20Personal%20_DX11_%202022-10-24%2003-23-47_Trim.mp4
