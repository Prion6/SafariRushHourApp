using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public static Action playerAuthenticated;
    public static Action playerAuthenticationFailed;
    private async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        await UnityServices.InitializeAsync();
    }
    public static async void AuthenticatePlayer(string playerName, string playerRut, string playerLocation, Action callbackOnSuccess = null, Action callbackOnFailure = null)
    {
        try
        {
            string sanitizedPlayerName = RemoveSpecialCharacters(playerName);
            //await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(sanitizedPlayerName + "S", "Paassw0rd!");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            playerAuthenticated?.Invoke();
            callbackOnSuccess?.Invoke();
        }
        catch (RequestFailedException ex)
        {
            playerAuthenticationFailed?.Invoke();
            callbackOnFailure?.Invoke();
            Debug.LogException(ex);
        }

        var data = new Dictionary<string, object> { { "playerName", playerName }, { "playerRut", playerRut }, { "playerLocation", playerLocation } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        //var data = new Dictionary<string, object> { { "playerName", playerName }, { "playerRut", playerRut }, { "playerLocation", playerLocation } };
        //await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        //await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public static async void AuthenticateDedveloper(Action callbackOnSuccess = null, Action callbackOnFailure = null)
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            playerAuthenticated?.Invoke();
            callbackOnSuccess?.Invoke();
        }
        catch (RequestFailedException ex)
        {
            playerAuthenticationFailed?.Invoke();
            callbackOnFailure?.Invoke();
            Debug.LogException(ex);
        }
    }

    private static string RemoveSpecialCharacters(string input)
    {
        return Regex.Replace(input, "[^a-zA-Z]", "");
    }

}