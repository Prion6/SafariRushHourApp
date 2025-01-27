using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformedConsentManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject declarationPanel;
    [SerializeField] private GameObject agreementPanel;
    [SerializeField] private GameObject mainMenu;
    
    [Header("Panels")]
    [SerializeField] private Button next1Button;
    [SerializeField] private Button next2Button;
    [SerializeField] private Button prev2Button;
    [SerializeField] private Button finalAcceptButton;
    [SerializeField] private Button finalNotAcceptButton;
    [SerializeField] private Button finalPrevButton;

    [SerializeField] TMP_InputField player_name;
    [SerializeField] TMP_InputField player_rut;
    [SerializeField] TMP_InputField player_location;

    [Header("Development properties")]
    [SerializeField] private bool logInOnStart = false;

    private void Start()
    {
        if(logInOnStart)
        {
            CloudManager.AuthenticateDedveloper(callbackOnSuccess: () =>
            {
                gameObject.SetActive(false);
                mainMenu.SetActive(true);
                informationPanel.SetActive(false);
                declarationPanel.SetActive(false);
                agreementPanel.SetActive(false);
            });
        }
        else
        {
            GoToInformationPanel();
        }

        next1Button.onClick.AddListener(GoToDeclarationPanel);
        next2Button.onClick.AddListener(GoToAgreementPanel);
        prev2Button.onClick.AddListener(GoToInformationPanel);
        finalAcceptButton.onClick.AddListener(Accept);
        finalNotAcceptButton.onClick.AddListener(NotAccept);
        finalPrevButton.onClick.AddListener(GoToDeclarationPanel);
    }
    public void GoToInformationPanel()
    {
        Debug.Log("Information panel open");
        informationPanel.SetActive(true);
        declarationPanel.SetActive(false);
        agreementPanel.SetActive(false);
    }
    public void GoToDeclarationPanel()
    {
        Debug.Log("Declaration panel open");
        informationPanel.SetActive(false);
        declarationPanel.SetActive(true);
        agreementPanel.SetActive(false);
    }
    public void GoToAgreementPanel()
    {
        Debug.Log("Agreement panel open");
        informationPanel.SetActive(false);
        declarationPanel.SetActive(false);
        agreementPanel.SetActive(true);
    }
    public void Accept()
    {
        Debug.Log("Data saved");
        CloudManager.AuthenticatePlayer(player_name.text, player_rut.text, player_location.text, callbackOnSuccess: () =>
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
            informationPanel.SetActive(false);
            declarationPanel.SetActive(false);
            agreementPanel.SetActive(false);
        }, callbackOnFailure: () =>
        {
            player_name.text = string.Empty;
            player_rut.text = string.Empty;
            player_location.text = string.Empty;
        });
    }
    public void NotAccept()
    {
        Debug.Log("Data not saved");
        Application.Quit();
    }
    public void UpdateButtonState()
    {
        if(player_name.text != string.Empty && player_rut.text != string.Empty && player_location.text != string.Empty)
        {
            finalAcceptButton.interactable = true;
        }
        else
        {
            finalAcceptButton.interactable = false;
        }
    }
}
