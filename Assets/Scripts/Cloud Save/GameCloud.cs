using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class GameCloud : MonoBehaviour
{
    private static Dictionary<string, string> uploadedData = new Dictionary<string, string>();

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public static async Task SaveData(string KEY, object storedData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>() { { KEY, storedData } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public static async void LoadData<T>(string KEY,  T obj)
    {
        uploadedData = new Dictionary<string, string>();
        await Task.Run(() => LoadData(KEY));
        obj = JsonConvert.DeserializeObject<T>(uploadedData[KEY]);
    }

    private static async Task LoadData(string KEY)
    {
        uploadedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { KEY });
    }
}