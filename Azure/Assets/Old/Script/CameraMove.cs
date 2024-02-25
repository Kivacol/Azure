using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    //public CinemachineVirtualCamera cam;
    //CinemachineComposer camera;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;

    public Move isFacingRight;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //camera = GetComponent<CinemachineVirtualCamera>().GetComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            if(isFacingRight)
            {
                Debug.Log("切換鏡頭右");
                cam1.SetActive(false);
                cam3.SetActive(false);
                cam2.SetActive(true);
            }
            //camera.m_ScreenX = 0;

            else if (!isFacingRight)
            {
                Debug.Log("切換鏡頭左");
                cam1.SetActive(false);
                cam2.SetActive(false);
                cam3.SetActive(true);
            }
                //camera.m_ScreenX = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //camera.m_ScreenX = 0.5f;
            cam1.SetActive(true);
            cam2.SetActive(false);
            cam3.SetActive(false);
        }
    }
}
