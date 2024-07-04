using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour {
    public Image iconMask;
    Image icon;
    [SerializeField] Sprite eyeIcon, legIcon, handIcon;

    // Start is called before the first frame update
    void Awake() {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void ChangeIcon(int mode) {
        if (mode == 1) {
            icon.sprite = eyeIcon;
        }
        if (mode == 2) {
            icon.sprite = legIcon;
        }
        if (mode == 3) {
            icon.sprite = handIcon;
        }
    }
}
