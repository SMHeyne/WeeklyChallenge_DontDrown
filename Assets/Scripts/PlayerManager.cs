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

    public string toolInSpotlight;
    bool isWateringCanEnlightened = false;
    public GameObject lightOnWateringCan;
    public GameObject lightOnPickaxe;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Rigidbody = GetComponent<Rigidbody>();
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

            lightOnWateringCan.SetActive(isWateringCanEnlightened);
                        
        }
        
        if (toolInSpotlight == "WateringCan" && showingToolUI)
        {
            isWateringCanEnlightened = true;
        }   
        else
        {
            isWateringCanEnlightened = false;
        }


        
        if(showingToolUI && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key E was pressed!");
            //ChangePlayerAppearance();
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

        if(collision.gameObject.name.Equals("WateringCan")){
            Debug.Log("touched wateringCan");
            SetToolUIVisibility(toolUiIsVisible: true);

                toolInSpotlight = "WateringCan";
                Debug.Log("Name for the tool touched: " + toolInSpotlight);
        }

        if(collision.gameObject.name.Equals("Pickaxe")){
            Debug.Log("touched Pickaxe");
            SetToolUIVisibility(toolUiIsVisible: true);

                toolInSpotlight = "Pickaxe";
                Debug.Log("Name for the tool touched: " + toolInSpotlight);
        }
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
