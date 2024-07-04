using UnityEngine;
using UnityEngine.UI;

public class LifespanUI : MonoBehaviour
{
    public Text lifespanText; // Reference to the Text component
    public PlayerController playerController; // Reference to the PlayerController

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    void Update()
    {
        if (lifespanText != null && playerController != null)
        {
            lifespanText.text = playerController.lifespan.ToString();
        }
    }
}