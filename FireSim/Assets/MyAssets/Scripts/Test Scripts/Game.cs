using UnityEngine;
using Valve.VR.InteractionSystem;

public class Game : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform xForm;
    public Player player;

    // Awake is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        if(!player)
        {
            Instantiate(playerPrefab, xForm.position, xForm.rotation);
            player = FindObjectOfType<Player>();
        }

        else
        {
            Destroy(player); 
            Instantiate(playerPrefab, xForm.position, xForm.rotation);
            player = FindObjectOfType<Player>();
        }
    }
}
