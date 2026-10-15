using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    [SerializeField] private CircleCollider2D downStrikeCollider;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        InventoryManager.instance.ReleaseSwordArcane_ConsiderCooldown();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);
                InventoryManager.instance.UseSwordEffect_ConsiderCooldown(_target.transform);
            }
        }
    }

    private void AirLaunchAttackTrigger()
    {
        InventoryManager.instance.ReleaseSwordArcane_ConsiderCooldown();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //set enemy knockback movement to make them get launched into air
                Enemy _enemy = hit.GetComponent<Enemy>();
                Vector2 originalKnockbackMovement = _enemy.knockbackMovement;
                _enemy.knockbackMovement = new Vector2(0, 17);

                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);

                //set enemy knockback movement to original state
                _enemy.knockbackMovement = originalKnockbackMovement;

                InventoryManager.instance.UseSwordEffect_ConsiderCooldown(_target.transform);
            }
        }
    }

    private void DownStrikeColliderOpenTrigger()
    {
        downStrikeCollider.gameObject.SetActive(true);
    }

    private void DownStrikeColliderCloseTrigger()
    {
        downStrikeCollider.gameObject.SetActive(false);
    }

    private void AirLaunchJumpTrigger()
    {
        player.AirLaunchJumpTrigger();
    }

    private void DownStrikeTrigger()
    {
        player.DownStrikeTrigger();
    }

    private void DownStrikeAnimStopTrigger()
    {
        player.DownStrikeAnimStopTrigger();
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
