using UnityEngine;

public class OpponentHandScript: MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    public int activeCardCount = 5;
    public void UpdateOpponentHand(int count)
    {
        print("Изменение количества карт оппонента "+count);
        activeCardCount = (activeCardCount + count > 5 ? 5 : activeCardCount + count);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i<activeCardCount);
        }
        
    }
}
