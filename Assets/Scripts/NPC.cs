using UnityEngine;
using TMPro;

using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    public InputActionAsset inputActionsAsset;

    public GameObject dialogPanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogText;
    public TextMeshProUGUI playerOptionsText;

    private List<string> dialogOptionsList = new List<string>();

    private InputAction dialogSelect;
    private InputAction dialogNext;
    private InputAction dialogPrev;

    private bool dialogSelectPressed;
    private bool dialogNextPressed;
    private bool dialogPrevPressed;

    private bool playerInArea = false;
    private int playerSelectedOption = 0;
    private int playerAvailableOptions = 0;

    private int questStage = 0;

    void Awake()
    {
        dialogSelect = inputActionsAsset.FindActionMap("Player").FindAction("Dialog Select");
        dialogNext = inputActionsAsset.FindActionMap("Player").FindAction("Dialog Next");
        dialogPrev = inputActionsAsset.FindActionMap("Player").FindAction("Dialog Previous");
    }

    void OnEnable()
    {
        dialogSelect.Enable();
        dialogNext.Enable();
        dialogPrev.Enable();
    }
    void OnDisable()
    {
        dialogSelect.Disable();
        dialogNext.Disable();
        dialogPrev.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        dialogSelectPressed = dialogSelect.triggered;
        dialogNextPressed = dialogNext.triggered;
        dialogPrevPressed = dialogPrev.triggered;

        if(playerInArea)
        {
            if(dialogNextPressed)
            {
                playerSelectedOption=(playerSelectedOption+1)%playerAvailableOptions;
                updateOptionsList();
            }
            if(dialogPrevPressed)
            {
                playerSelectedOption=(playerSelectedOption-1)%playerAvailableOptions;
                updateOptionsList();
            }
            if(dialogSelectPressed)
            {
                if(questStage==0)
                {
                    if(playerSelectedOption==0)
                    {
                        npcDialogText.SetText("Nice, find me some carrots and wheat :D");

                        dialogOptionsList.Clear();
                        dialogOptionsList.Add("Where to find it ?");
                        dialogOptionsList.Add("I will search everywhere");

                        playerSelectedOption = 0;
                        playerAvailableOptions = 2;
                        updateOptionsList();
                        questStage++;
                    }
                    else
                    {
                        npcDialogText.SetText("Get lost >:(");
                        playerOptionsText.SetText(":(");
                    }
                }
                else if(questStage==1)
                {
                    if(playerSelectedOption==0)
                    {
                        npcDialogText.SetText("Go to that island on my left");

                        dialogOptionsList.Clear();
                        dialogOptionsList.Add("Okay");
                        dialogOptionsList.Add("Is it safe ?");

                        playerSelectedOption = 0;
                        playerAvailableOptions = 2;
                        updateOptionsList();

                        questStage++;
                    }
                    else
                    {
                        npcDialogText.SetText("I like your attitude");
                        playerOptionsText.SetText("*smiles*");
                    }
                }
                else if(questStage==2)
                {
                    if(playerSelectedOption==0)
                    {
                        npcDialogText.SetText("Have fun !!");

                        playerOptionsText.SetText("*smiles*");
                    }
                    else
                    {
                        npcDialogText.SetText("Ya, totally");

                        playerOptionsText.SetText("*smiles*");
                    }
                }
            }
            //Debug.Log($"Options is {playerSelectedOption}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Player came :D");
        dialogPanel.SetActive(true);
        playerInArea = true;

        npcNameText.SetText("Dude");

        if(questStage==0)
        {
            npcDialogText.SetText("Ay bro, can you help ?");
            dialogOptionsList.Clear();
            dialogOptionsList.Add("Yes");
            dialogOptionsList.Add("No");

            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Player gone :(");
        dialogPanel.SetActive(false);
        playerInArea = false;
    }

    void updateOptionsList()
    {
        int i = 0;
        string options ="> ";
        foreach (string dialogString in dialogOptionsList)
        {
            if(playerSelectedOption==i)
            {
                options += $"<{dialogString}> ";
            }
            else
            {
                options += $"{dialogString} ";
            }
            i++;
        }
        playerOptionsText.SetText(options);
    }
}
