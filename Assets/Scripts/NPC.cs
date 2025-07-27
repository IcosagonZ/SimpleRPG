using UnityEngine;
using TMPro;

using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

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
    private bool playerHasItems = false;

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

        if (playerInArea)
        {
            if (dialogNextPressed)
            {
                playerSelectedOption = (playerSelectedOption + 1) % playerAvailableOptions;
                updateOptionsList();
            }
            if (dialogPrevPressed)
            {
                playerSelectedOption = (playerSelectedOption - 1) % playerAvailableOptions;
                updateOptionsList();
            }
            if (dialogSelectPressed)
            {
                if (questStage == 0)
                {
                    if (playerSelectedOption == 0)
                    {
                        questStage++;
                        updateQuestText(); //1
                    }
                    else
                    {
                        npcDialogText.SetText("Get lost >:(");
                        playerOptionsText.SetText(">*sad*");
                    }
                }
                else if (questStage == 1)
                {
                    questStage++;
                    if (playerSelectedOption == 0)
                    {
                        updateQuestText(); //2
                    }
                    else
                    {
                        npcDialogText.SetText("Nice");
                        playerOptionsText.SetText(">*smiles*");
                    }
                }
                else if (questStage == 2)
                {
                    questStage++;
                    if (playerSelectedOption == 0)
                    {
                        npcDialogText.SetText("Have fun !!");
                        playerOptionsText.SetText(">*smiles*");
                    }
                    else
                    {
                        npcDialogText.SetText("Ya, totally");
                        playerOptionsText.SetText(">*worried*");
                    }
                }
                else if (questStage == 3)
                {
                    questStage++;
                    if (playerSelectedOption == 0)
                    {
                        npcDialogText.SetText("Have fun !!");

                        playerOptionsText.SetText(">*smiles*");
                    }
                    else
                    {
                        npcDialogText.SetText("Ya, totally");

                        playerOptionsText.SetText(">*worried*");
                    }
                }
                else if (questStage == 4)
                {
                    if (playerSelectedOption == 0)
                    {
                        checkPlayerInventory();
                        if (playerHasItems)
                        {
                            npcDialogText.SetText("Thanks a lot");
                            playerOptionsText.SetText(">*happy*");
                            questStage++;
                        }
                        else
                        {
                            npcDialogText.SetText("Stop lying");
                            playerOptionsText.SetText(">*sad*");
                        }
                    }
                    else
                    {
                        npcDialogText.SetText("Okay");

                        playerOptionsText.SetText(">Yeah");
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
        updateQuestText();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Player gone :(");
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
        playerInArea = false;
    }

    void updateQuestText()
    {
        if (questStage == 0)
        {
            npcDialogText.SetText("Hey bro, can you help ?");
            dialogOptionsList.Clear();
            dialogOptionsList.Add("Yes");
            dialogOptionsList.Add("No");

            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
        else if (questStage == 1)
        {
            npcDialogText.SetText("Nice, find me some carrots and wheat :D");

            dialogOptionsList.Clear();
            dialogOptionsList.Add("Where to find it ?");
            dialogOptionsList.Add("I will search everywhere");

            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
        else if (questStage == 2)
        {
            npcDialogText.SetText("Go to that island on my left");

            dialogOptionsList.Clear();
            dialogOptionsList.Add("Okay");
            dialogOptionsList.Add("Is it safe ?");

            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
        else if (questStage == 3)
        {
            npcDialogText.SetText("You have doubts ?");

            dialogOptionsList.Clear();
            dialogOptionsList.Add("No");
            dialogOptionsList.Add("Is it safe ?");

            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
        else if (questStage == 4)
        {
            npcDialogText.SetText("Have you found what I asked ?");
            dialogOptionsList.Clear();
            dialogOptionsList.Add("Yes");
            dialogOptionsList.Add("No");
            playerSelectedOption = 0;
            playerAvailableOptions = 2;
            updateOptionsList();
        }
        else if (questStage == 5)
        {
            npcDialogText.SetText("Thanks for finding me the ingredients");
            playerOptionsText.SetText(">Sure");
        }
    }

    public void updateOptionsList()
    {
        int i = 0;
        string options = "> ";
        foreach (string dialogString in dialogOptionsList)
        {
            if (playerSelectedOption == i)
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

    public void checkPlayerInventory()
    {
        int carrots = 0;
        int wheats = 0;

        Player player;
        GameObject playerObject = GameObject.Find("Player");
        List<string> playerInventory = new List<string>();

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                playerInventory = player.inventory;
                foreach (String item in playerInventory)
                {
                    if (item == "Carrot")
                    {
                        carrots++;
                    }
                    if (item == "Wheat")
                    {
                        wheats++;
                    }
                }
            }
        }

        if (carrots>1 && wheats>1)
        {
            playerHasItems = true;
        }
    }
}
