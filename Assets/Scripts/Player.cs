using System;
using System.Collections;

using KinematicCharacterController.Examples;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Transform CurrentPlayer;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    [SerializeField] float m_CurrentHealth, m_MaxHealth;
    [SerializeField] GameObject m_HealthBar;
    [SerializeField] GameObject ResetButton;
    [SerializeField] ParticleSystem bloodParticle;
    [SerializeField] AudioClipPreset hurtAP, deathAP;
    public bool isFirstPerson;
    internal void DealDamage(float Damage)
    {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - Damage, 0, m_MaxHealth);
        m_HealthBar.transform.localScale = new((float)m_CurrentHealth / (float)m_MaxHealth, 1, 1);
        bloodParticle.Play();
        hurtAP.play();
        if (m_CurrentHealth <= 0)
        {
            if (currentVehicle != null)
                currentVehicle.UnDrive();
            IsAlive = false;
            m_Animator.Play("death");
            deathAP.play();
            ResetButton.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    internal void dealDamagerPerHour(float hours)
    {
        float dealDamageInSeconds = hours * 3600f;
        float factor = Time.deltaTime / dealDamageInSeconds;
        float damageFactor = Mathf.Lerp(0, m_MaxHealth, factor);
        DealDamage(damageFactor);
    }

    public static Player instance;
    public static bool IsAlive = true;
    public static Vehical currentVehicle;
    [SerializeField] internal ExampleCharacterController m_ExampleCharacterController;
    [SerializeField] ExampleCharacterCamera CharacterCamera;
    [SerializeField] GameObject m_model_character;
    [SerializeField] float sprintSpeed;
    [SerializeField] Animator m_Animator;
    [SerializeField] Transform m_player;
    [SerializeField] LayerMask cycleLandMask;
    new Camera camera;
    float normalSpeed, YPos;
    bool IsOnCycle = false;
    PhotonView view;

    private void Awake()
    {
        normalSpeed = m_ExampleCharacterController.MaxStableMoveSpeed;
        lastCharacterPos = m_player.position;
        camera = Camera.main;
        CurrentPlayer = m_ExampleCharacterController.transform;
        currentVehicle = null;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _startTime = Time.time;
        m_Animator.SetFloat("Y", 0);
        m_HealthBar = GameObject.Find("HealthBar");
        PlayerPrefs.DeleteAll();
    }

    Vector3 lastCharacterPos;
    void Update()
    {
        //if (view.IsMine)
        //{
        bool shiftPressed = false;
        if (Input.GetKey(KeyCode.LeftShift) || IsOnCycle)
        {
            m_ExampleCharacterController.MaxStableMoveSpeed = sprintSpeed;
            shiftPressed = true;
        }
        else
        {
            m_ExampleCharacterController.MaxStableMoveSpeed = normalSpeed;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isFirstPerson)
            {
                CharacterCamera.FollowPointFraming = new Vector3(0.09f, 0.31f, 0.76f);
                CharacterCamera.MaxDistance = 0;
                isFirstPerson = true;
            }
            else
            {
                isFirstPerson = false;
                CharacterCamera.FollowPointFraming = new Vector3(-0.33f, 0.95f, -2.97f);
                CharacterCamera.MaxDistance = 10;
                CharacterCamera.DefaultDistance = 6;
            }

        }

        Vector3 newPos = m_player.position - lastCharacterPos;
        lastCharacterPos = m_player.position;

        if (newPos.magnitude <= 0.0001f)
        {
            m_Animator.SetFloat("X", 0);
            YPos = 0;
        }
        else
        {
            if (shiftPressed)
            {
                m_Animator.SetFloat("X", 0);
                YPos = 1;
            }
            else
            {
                m_Animator.SetFloat("X", 0);
                YPos = 0.5f;
            }
        }

        m_Animator.SetFloat("Y", Mathf.Lerp(m_Animator.GetFloat("Y"), YPos, Time.unscaledDeltaTime * 30));
        //}

    }

    [SerializeField] AudioClipPreset jumpStartAP, jumpEndAP;
    public void SetJumpStart()
    {
        if (landCheck_Instance != null)
        {
            StopCoroutine(landCheck_Instance);
        }
        m_Animator.SetTrigger("Jump");
        jumpStartAP.play();
    }
    public void SetJumpEnd()
    {
        if (landCheck_Instance != null)
        {
            StopCoroutine(landCheck_Instance);
        }
        landCheck_Instance = landCheck();
        StartCoroutine(landCheck());
        jumpEndAP.play();
    }

    float _startTime = 0;
    IEnumerator landCheck_Instance;
    IEnumerator landCheck()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (Physics.Linecast(m_ExampleCharacterController.transform.position + new Vector3(0, 0.3f, 0), m_ExampleCharacterController.transform.position - new Vector3(0, 0.3f, 0), out RaycastHit hit, cycleLandMask))
            {
                if ((Time.time - _startTime) > 0.5f)
                {
                    Debug.Log("here you go", hit.collider.gameObject);
                    m_Animator.SetTrigger("Land");
                }
                yield break;
            }
        }
    }

    public void Ride()
    {
        CharacterCamera.gameObject.SetActive(false);
        m_ExampleCharacterController.gameObject.SetActive(false);
    }

    public void UnRide()
    {
        CharacterCamera.gameObject.SetActive(true);
        m_ExampleCharacterController.gameObject.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
