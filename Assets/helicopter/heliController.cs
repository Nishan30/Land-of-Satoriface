using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heliController : MonoBehaviour
{
    [SerializeField] Transform m_Hook, m_PlayerExitHook;
    [SerializeField] KeyCode m_KeyCode;
    private Rigidbody rb;
    public int speed = 10;
    public int rotateSpeed = 1;
    public GameObject copter;
    Player m_Player;
    public Camera camera;
    Vector3 lastPos;
    public GameObject camera1;
    public GameObject camera2;
    GameObject m_Indicator;
    float RotationYOffset, Idle_Y_Offset, currentGas;
    bool isRiding = false;
    Transform m_PlayerTransform;
    [SerializeField]
    float m_Distance;
    public Tradingmanager tradingManager;
    float reductionRate = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_Indicator = IndicatorManager.AddIndicator(m_Hook, m_KeyCode.ToString());
        //camera = Camera.main;
        m_Player = Player.Instance;
        Idle_Y_Offset = transform.position.y;
        m_PlayerTransform = m_Player.m_ExampleCharacterController.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_PlayerTransform.transform.position) <= m_Distance && isRiding == false && tradingManager.heliOn)
        {
            m_Indicator.SetActive(true);
        }
        else
        {
            m_Indicator.SetActive(false);
        }
        if (Player.IsAlive)
        {
            if (Input.GetKeyDown(m_KeyCode))
            {
                if (!isRiding && m_Indicator.activeInHierarchy)
                {
                    isRiding = true;
                    rb.isKinematic = false;
                    camera1.SetActive(true);

                }
                else
                {
                    UnDrive();
                }
            }
            if (isRiding)
            {
                m_Player.Ride();
                camera.gameObject.SetActive(false);
                isRiding = true;
                Vector3 rotationRight = new Vector3(0, -30, 0);
                Vector3 rotationLeft = new Vector3(0, 30, 0);
                if (rb.velocity == Vector3.zero)
                {
                    copter.GetComponent<Animator>().enabled = false;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    copter.GetComponent<Animator>().enabled = true;
                    //rb.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
                    rb.AddForce(transform.up * speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    copter.GetComponent<Animator>().enabled = true;
                    //rb.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
                    rb.AddForce(transform.forward * speed*1/4 * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    copter.GetComponent<Animator>().enabled = true;
                    //rb.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
                    rb.AddForce(-transform.forward * speed *1/4* Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.X))
                {
                    rb.velocity *= 1 - reductionRate * Time.deltaTime;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    camera2.SetActive(!camera2.activeInHierarchy);
                    camera1.SetActive(!camera1.activeInHierarchy);
                }

                //if (Input.GetKey(KeyCode.DownArrow))
                //{
                //    rb.MovePosition(transform.position - transform.up * speed * Time.deltaTime);
                //}

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    copter.GetComponent<Animator>().enabled = true;
                    Quaternion deltaRotationRight = Quaternion.Euler(rotationRight * Time.deltaTime);
                    rb.MoveRotation(rb.rotation * deltaRotationRight);
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    copter.GetComponent<Animator>().enabled = true;
                    Quaternion deltaRotationLeft = Quaternion.Euler(rotationLeft * Time.deltaTime);
                    rb.MoveRotation(rb.rotation * deltaRotationLeft);
                }
            }
        }
        
    }
    internal void UnDrive()
    {
        m_Player.UnRide();
        Player.currentVehicle = null;
        gameObject.tag = "Vehicle";
        Player.CurrentPlayer = Player.instance.m_ExampleCharacterController.transform;
        if (Physics.Linecast(m_PlayerExitHook.position, m_PlayerExitHook.position - new Vector3(0, 10, 0), out RaycastHit hit))
        {
            ExampleCharacterController cc = m_Player.m_ExampleCharacterController;
            if (cc)
            {
                cc.Motor.SetPositionAndRotation(hit.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
        }
        else
        {
            ExampleCharacterController cc = m_Player.m_ExampleCharacterController;
            if (cc)
            {
                cc.Motor.SetPositionAndRotation(m_PlayerExitHook.position, Quaternion.identity);
            }
        }

            if (Physics.Linecast(transform.position - new Vector3(0, 0.1f, 0), transform.position - new Vector3(0, 10, 0), out RaycastHit hit1))
            {
                transform.position = hit1.point + new Vector3(0, Idle_Y_Offset, 0);
            }

        isRiding = false;
        camera.gameObject.SetActive(true);
        camera1.SetActive(false);
        camera2.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        copter.GetComponent<Animator>().enabled = false;
    }
}
