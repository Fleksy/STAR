using Unity.Netcode;
using System;

public class SceneLoader : NetworkBehaviour
{
    public static SceneLoader Instatnce { get; private set; }

    private void Awake()
    {
        Instatnce = this;
    }

    public void LoadScene(object sender, EventArgs e)
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
