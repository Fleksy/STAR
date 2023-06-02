using TMPro;
using Unity.Netcode;
using UnityEngine;
public class NetworkScript : NetworkBehaviour
{
    private TMP_Text log;
    
    
    private void Awake()
    {
        Inst();
    }

    private void Inst()
    {
        log = GameObject.Find("Log").GetComponent<TMP_Text>();
    }

    public void Start()
    {
        if (!IsOwner) return;
       
        
    }
}