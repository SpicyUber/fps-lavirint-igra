using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Renderer shieldRenderer;      // Mesh renderer objekta (materijal mora imati transparentnost)
    public Collider shieldCollider;      // Collider koji detektuje udarce
    public float visibleDuration = 2f;   // Koliko dugo ostaje vidljiv nakon pogotka
    public Color visibleColor = new Color(0f, 1f, 0f, 0.3f);  // Zelena, 30% providna

    private Material shieldMaterial;
    private Color invisibleColor;
    private Coroutine visibilityCoroutine;

    void Start()
    {
        if (shieldRenderer != null)
        {
            Debug.Log("Shield renderer je postavljen.");
            shieldMaterial = shieldRenderer.material;
            invisibleColor = new Color(0f, 1f, 0f, 0f); // 100% providna
            shieldMaterial.color = invisibleColor;
        }

       //ShieldUp(); // Štit je neaktivan na početku
       // ShowTemporarily();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bullet")) // Pretpostavljamo da metak ima tag "Bullet"
        {
        Debug.Log("Shield triggered by: " + other.name);
        ShowTemporarily();
        }
    }

    private void ShowTemporarily()
    {
        if (visibilityCoroutine != null)
            StopCoroutine(visibilityCoroutine);

        visibilityCoroutine = StartCoroutine(TemporaryVisibility());
    }

    private IEnumerator TemporaryVisibility()
    {
        SetShieldColor(visibleColor);
        yield return new WaitForSeconds(visibleDuration);
        SetShieldColor(invisibleColor);
    }

    private void SetShieldColor(Color color)
    {
        if (shieldMaterial != null)
            shieldMaterial.color = color;
    }
    [ContextMenu("Activate Shield")]
    public void ShieldUp()
    {
        if (shieldRenderer != null)
            shieldRenderer.enabled = true;

        if (shieldCollider != null)
            shieldCollider.enabled = true;
    }
    [ContextMenu("Deactivate Shield")]
    public void ShieldDown()
    {
        if (shieldRenderer != null)
            shieldRenderer.enabled = false;

        if (shieldCollider != null)
            shieldCollider.enabled = false;
    }
}
