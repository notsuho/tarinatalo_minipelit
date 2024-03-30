using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowControl : MonoBehaviour {
    private float glowSpeed = 3f;
    private float glowStartTime;
    private bool isGlowing = false;
    private List<Color> correctGlowRange = new List<Color> {
        new Color(1.5f, 1.5f, 1f, 0.1f),
        new Color(2f, 2f, 1f, 0.1f)
    };
    private List<Color> wrongGlowRange = new List<Color> {
        new Color(1f, 1f, 1f, 1.0f),
        new Color(2f, 1f, 1f, 0.1f)
    };
    private List<Color> currentGlowRange;

    private Color defaultColor = new Color(1f, 1f, 1f, 1.0f);
    private Renderer rendererComponent;

    void Start() {
        this.rendererComponent = this.gameObject.GetComponent<Renderer>();
    }

    void Update() {
        if (!this.isGlowing) {
            return;
        }

        // https://discussions.unity.com/t/arc-movement-lerp-mathf-sin-and-mathf-pingpong-question/126838
        // calculate current color using Mathf builtin functions.
        // subtract glowStartTime from Time.time so the glowing cycle always starts at the same position
        float lerp = Mathf.PingPong(Time.time - this.glowStartTime, this.glowSpeed) / this.glowSpeed;
        Color lerpColor = Color.Lerp(
            this.currentGlowRange[0],
            this.currentGlowRange[1],
            Mathf.Sin(lerp * Mathf.PI)
        );
        this.rendererComponent.material.SetColor("_Color", lerpColor);
    }

    public void MakeBookGlow() {
        this.glowStartTime = Time.time;
        this.isGlowing = true;
        this.currentGlowRange = this.correctGlowRange;
    }

    public void MakeBookRed() {
        this.glowStartTime = Time.time;
        this.isGlowing = true;
        this.currentGlowRange = this.wrongGlowRange;

        // use glowSpeed value as time after when to restore the book color
        // so the glow finishes one cycle
        Invoke("RestoreBookColor", this.glowSpeed);
    }

    void RestoreBookColor() {
        this.isGlowing = false;
        this.rendererComponent.material.SetColor("_Color", this.defaultColor);
    }
}
