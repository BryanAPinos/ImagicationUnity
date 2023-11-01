using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

class InitWithEnvironment : MonoBehaviour
{
    async void Awake()
    {
        var options = new InitializationOptions();

        options.SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}