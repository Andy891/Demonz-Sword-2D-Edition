using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform arrow;
    private int currentPosition;


    private void Awake()
    {
        arrow = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveArrow(1);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveArrow(-1);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }
    }

    private void MoveArrow(int direction)
    {
        int newPosition = currentPosition + direction;
        if (newPosition >= 0 && newPosition < buttons.Length)
        {
            currentPosition = newPosition;
            SoundManager.instance.PlaySound(changeSound);
            arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
        }
    }

    private void Interact()
    {

        SoundManager.instance.PlaySound(interactSound);
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }


}