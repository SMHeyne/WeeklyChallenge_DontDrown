using System.Transactions;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool playerOnGround = true;
    private CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody Rigidbody;

    private bool showingToolUI = false;
    public float timeRemainingOnCountdown = 3;
    public GameObject useToolUI;

    public GameObject lightOnWateringCan;
    public GameObject lightOnPickaxe;
    private GameObject activeLight;

    public Dictionary<string, GameObject> lightOnTool;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Rigidbody = GetComponent<Rigidbody>();

        lightOnTool = new Dictionary<string, GameObject>()
        {
            { Tools.WateringCan, lightOnWateringCan },
            { Tools.Pickaxe, lightOnPickaxe },
        };
        
    }

    void FixedUpdate()
    {
        
        Vector3 moveDirection = new Vector3(
            x: Input.GetAxis("Horizontal"), 
            y: 0, 
            z: Input.GetAxis("Vertical"));

        Rigidbody.AddForce(moveDirection * moveSpeed);

    }

    private void Update() {
        
        if (showingToolUI)
        {
            StartCountdownToDisableToolUI();   

            if (Input.GetKeyDown(KeyCode.E))       
            {
                Debug.Log("Key E was pressed!");
                //ChangePlayerAppearance();
            }
        }
        else if (activeLight is not null)
        {
            SetActiveLight(null);    
            SetToolUIVisibility(toolUiIsVisible: false);
        }
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Ground") {
            playerOnGround = true;
        }
        else
        {
            playerOnGround = false;
            Debug.Log("Player not on ground");
        }

        if (lightOnTool.TryGetValue(collision.gameObject.name, out GameObject light))
        {
            Debug.Log($"touched {collision.gameObject.name}");
            SetActiveLight(newActiveLight: light);
            SetToolUIVisibility(toolUiIsVisible: true);
        }
    }

    private void SetActiveLight(GameObject newActiveLight)
    {
        var previousActiveLight = activeLight;
        activeLight = newActiveLight;

        previousActiveLight?.SetActive(false);
        activeLight?.SetActive(true);
        
        /* Short form for:
            
            if (previousActiveLight is not null)
            {
                previousActiveLight.SetActive(false);
            }

            if (activeLight is not null)
            {
                activeLight.SetActive(false);
            }
        */
    }

    private void SetToolUIVisibility(bool toolUiIsVisible) 
    {
        showingToolUI = toolUiIsVisible;
        useToolUI.gameObject.SetActive(toolUiIsVisible);
        timeRemainingOnCountdown = 3;
    } 

    private void StartCountdownToDisableToolUI()
    {
        timeRemainingOnCountdown -= Time.deltaTime;

        if (timeRemainingOnCountdown <= 0)
        {
            SetToolUIVisibility(toolUiIsVisible: false);
            timeRemainingOnCountdown = 3;
        }
    }

    /*
    private void ChangePlayerAppearance()
    {

    }
    */
}
