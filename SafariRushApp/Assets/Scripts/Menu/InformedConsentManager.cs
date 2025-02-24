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
    [SerializeField] private Button finalJustPlayButton;

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
        finalJustPlayButton.onClick.AddListener(JustPlay);

        player_name.onValueChanged.AddListener(UpdateButtonState);
        player_rut.onValueChanged.AddListener(UpdateButtonState);
        player_location.onValueChanged.AddListener(UpdateButtonState);
    }
    private void OnDestroy()
    {
        next1Button.onClick.RemoveListener(GoToDeclarationPanel);
        next2Button.onClick.RemoveListener(GoToAgreementPanel);
        prev2Button.onClick.RemoveListener(GoToInformationPanel);
        finalAcceptButton.onClick.RemoveListener(Accept);
        finalNotAcceptButton.onClick.RemoveListener(NotAccept);
        finalPrevButton.onClick.RemoveListener(GoToDeclarationPanel);

        player_name.onValueChanged.RemoveListener(UpdateButtonState);
        player_rut.onValueChanged.RemoveListener(UpdateButtonState);
        player_location.onValueChanged.RemoveListener(UpdateButtonState);
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
    public void UpdateButtonState(string _)
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
    public void JustPlay()
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
    public void PlayLevel()
    {
        GameManager.LoadPuzzleScene(0);
    }
}
