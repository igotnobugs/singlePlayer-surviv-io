using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenUnder : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer spriteToHide = null;
    [SerializeField] private float hideAlpha = 0.3f;

    private Color defaultColor;
    private Color hideColor;

    private void Start() {
        defaultColor = spriteToHide.color;
        hideAlpha = Mathf.Clamp(hideAlpha, 0.0f, 1.0f);
        hideColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, hideAlpha);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            spriteToHide.color = hideColor;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            spriteToHide.color = defaultColor;
        }
    }
}
