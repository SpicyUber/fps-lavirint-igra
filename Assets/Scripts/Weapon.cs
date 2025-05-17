using UnityEngine;

public interface Weapon
{
    public bool UseAttack();
    public float GetAttackRangeForAI();
    public float GetCooldown();
    public void SetCooldown(float cooldown);

    public float GetWindUp();
    public void SetWindUp(float windUp);
}
