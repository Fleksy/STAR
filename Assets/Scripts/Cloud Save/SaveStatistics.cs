using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEditor;
using UnityEngine;

public class SaveStatistics : MonoBehaviour
{
    [SerializeField] public static TextMeshProUGUI StatisticsText = GameObject.Find("StatisticsText").GetComponent<TextMeshProUGUI>();

    public static async void GetStatisticsAsync()
    {
        var query = await Call(CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { AuthenticationService.Instance.PlayerId + "win" }));
        int.TryParse(query.TryGetValue(AuthenticationService.Instance.PlayerId, out var value) ? Deserialize<string>(value) : default, out Statistics.win);
        query = await Call(CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { AuthenticationService.Instance.PlayerId + "lose" }));
        int.TryParse(query.TryGetValue(AuthenticationService.Instance.PlayerId, out value) ? Deserialize<string>(value) : default, out Statistics.win);
    }
    private static async Task Call(Task action)
    {
        try
        {
            await action;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
    private static async Task<T> Call<T>(Task<T> action)
    {
        try
        {
            return await action;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return default;
    }
    private static T Deserialize<T>(string input)
    {
        if (typeof(T) == typeof(string)) return (T)(object)input;
        return JsonConvert.DeserializeObject<T>(input);
    }

    public static async void Save(string key, object value)
    {
        var data = new Dictionary<string, object> { { key, value } };
        await Call(CloudSaveService.Instance.Data.ForceSaveAsync(data));
    }

    public static void UpdateInfo()
    {
        StatisticsText.text = $"W:{Statistics.win} L:{Statistics.lose}";
    }
}
