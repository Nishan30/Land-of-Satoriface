using KinematicCharacterController.Examples;

using TMPro;

using UnityEngine;

public class Vehical : MonoBehaviour
{
    Transform m_PlayerTransform;
    [SerializeField] Transform m_Hook, m_PlayerExitHook;
    [SerializeField] float m_Distance, speed = 1, drag = 0.05f, cameraSpeed = 8;
    [SerializeField] KeyCode m_KeyCode;
    [SerializeField] bool RotateOnlyInMotion = false;
    [SerializeField] float DistanceNeededForRotation = 1;
    [SerializeField] VehicleCamera m_VehicleCamera;
    [SerializeField] bool Jump = false, UseGround = true, SnapToGroundAfterUnDrive;
    [SerializeField] float jumpForce = 1;
    [SerializeField] Vector3 onGroundStart, onGroundEnd;
    [SerializeField] GameObject ObjectToEnableWhileRiding;

    [Header("Extra Stuff")]
    public GameObject carCanvas;
    [SerializeField] TextMeshProUGUI MPH_Meter;
    [SerializeField] Transform Gas_Meter;
    [SerializeField] float maxGas, gasPerMile;

    float RotationYOffset, Idle_Y_Offset, currentGas;
    Rigidbody m_rigidbody;
    GameObject m_Indicator;
    Player m_Player;
    bool IsRiding = false;
    new Camera camera;
    Vector3 lastPos;

    private void Start()
    {
        currentGas = maxGas;
        gameObject.tag = "Vehicle";
        GameObject l_fpsHook = new GameObject();
        camera = Camera.main;
        m_Player = Player.Instance;
        m_PlayerTransform = m_Player.m_ExampleCharacterController.transform;
        m_Indicator = IndicatorManager.AddIndicator(m_Hook, m_KeyCode.ToString());
        m_VehicleCamera.gameObject.SetActive(false);
        RotationYOffset = m_VehicleCamera.transform.rotation.y;
        if (ObjectToEnableWhileRiding != null)
            ObjectToEnableWhileRiding.SetActive(false);
        Idle_Y_Offset = transform.position.y;
    }
    void Update()
    {
        if (Player.IsAlive)
        {
            if (Input.GetKeyDown(m_KeyCode))
            {
                if (IsRiding)
                {
                    UnDrive();
                    carCanvas.SetActive(false);
                }
                else if (Vector3.Distance(transform.position, m_PlayerTransform.transform.position) <= m_Distance && Player.currentVehicle == null && m_Indicator.activeSelf)
                {
                    Drive();
                    carCanvas.SetActive(true);
                }
            }

            if (Vector3.Distance(transform.position, m_PlayerTransform.transform.position) <= m_Distance && IsRiding == false)
            {
                m_Indicator.SetActive(true);
            }
            else
            {
                m_Indicator.SetActive(false);
            }

            if (IsRiding && m_rigidbody != null)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    m_rigidbody.AddForce(transform.forward * speed);
                    moveFrontAP.play();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    m_rigidbody.AddForce(-transform.forward * speed);
                    moveBackAP.play();
                }
                if (Jump && Input.GetKeyDown(KeyCode.Space) && (UseGround == false || isOnGround()))
                {
                    m_rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    moveFrontAP.Stop();
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    moveBackAP.Stop();
                }
                // if (Input.GetKey(KeyCode.A))
                // {
                //     m_rigidbody.AddForce(-transform.right * speed);
                // }
                // if (Input.GetKey(KeyCode.D))
                // {
                //     m_rigidbody.AddForce(transform.right * speed);
                // }
                var rotation = m_VehicleCamera.player.transform.rotation;
                rotation.x = transform.rotation.x;
                rotation.z = transform.rotation.z;
                rotation.y -= RotationYOffset;
                float currentSpeed = Vector3.Distance(transform.position, lastPos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * cameraSpeed * ((RotateOnlyInMotion) ? currentSpeed * DistanceNeededForRotation : 1));
            }
            lastPos = transform.position;
        }
        else
        {
            m_Indicator.SetActive(false);
        }


        // 3.6f to convert in kilometers
        // ** The speed must be clamped by the car controller **
        if (m_rigidbody)
        {
            float _speed = m_rigidbody.velocity.magnitude * 3.6f;

            if (MPH_Meter != null)
                MPH_Meter.text = ((int)_speed) + " MPH";

            if (Gas_Meter != null)
            {
                float factor = _speed * gasPerMile * (Time.deltaTime / 3600);
                currentGas -= factor;
                Gas_Meter.localScale = new Vector3(currentGas / maxGas, 1, 1);
            }
        }
    }

    [SerializeField] AudioClipPreset DriveAP, moveFrontAP, moveBackAP, IdleAP;
    private void Drive()
    {
        Player.currentVehicle = this;
        gameObject.tag = "Player";
        Player.CurrentPlayer = transform;
        m_VehicleCamera.gameObject.SetActive(true);
        m_Player.Ride();
        IsRiding = true;
        m_rigidbody = gameObject.AddComponent<Rigidbody>();
        setupRigidbody();
        if (ObjectToEnableWhileRiding != null)
            ObjectToEnableWhileRiding.SetActive(true);
        DriveAP.play();
        IdleAP.play();
    }
    internal void UnDrive()
    {
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

        if (SnapToGroundAfterUnDrive)
        {
            if (Physics.Linecast(transform.position - new Vector3(0, 0.1f, 0), transform.position - new Vector3(0, 10, 0), out RaycastHit hit1))
            {
                transform.position = hit1.point + new Vector3(0, Idle_Y_Offset, 0);
            }
        }


        m_Player.UnRide();
        m_VehicleCamera.gameObject.SetActive(false);
        IsRiding = false;
        if (m_rigidbody != null)
            Destroy(m_rigidbody);
        if (ObjectToEnableWhileRiding != null)
            ObjectToEnableWhileRiding.SetActive(false);
        IdleAP.Stop();
        moveFrontAP.Stop();
        moveBackAP.Stop();
        DriveAP.Stop();
    }

    void setupRigidbody()
    {
        m_rigidbody.freezeRotation = true;
        m_rigidbody.drag = drag;
    }

    bool isOnGround()
    {
        return Physics.Linecast(transform.TransformPoint(onGroundStart), transform.TransformPoint(onGroundEnd));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.TransformPoint(onGroundStart), transform.TransformPoint(onGroundEnd));
    }
}
