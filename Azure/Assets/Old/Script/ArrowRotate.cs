using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowRotate : MonoBehaviour
{

    public Transform target;    // 玩家物件
    //public float speed = 50f; // 旋轉速度
    public Vector3 axis = Vector3.up; // 旋轉軸向

    public MoveX movex;
    public GameObject Player;


    [SerializeField] private Vector2 arrowInputValue;
    private PlayerInputActions controls;

    public float angle;
    public float Pangle;

    public bool isFilp;



    void Awake()
    {
        controls = new PlayerInputActions();


    }

    void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    void OnArrow()
    {
        arrowInputValue = controls.GamePlay.Arrow.ReadValue<Vector2>();

    }


    // Start is called before the first frame update
    void Start()
    {
        isFilp = false;

    }

    // Update is called once per frame
    void Update()
    {
        

        arrowInputValue = controls.GamePlay.Arrow.ReadValue<Vector2>();


        angle = Vector2.SignedAngle(arrowInputValue, Vector2.right);
        if (movex.isFacingRight)
            Pangle = Vector2.SignedAngle(this.transform.forward, Vector2.right);
        else
            Pangle = Vector2.SignedAngle(this.transform.forward, Vector2.left);
        // 計算旋轉中心點，這裡以玩家物件的位置為中心點
        Vector3 center = target.position;
        // 使用 Transform.RotateAround 方法實現旋轉
        if ((Pangle - angle > 0.1f || Pangle - angle < 0.1f) && angle!=0)
        {
            
                transform.RotateAround(center, axis, Pangle - angle);

        }

      
    }
}
