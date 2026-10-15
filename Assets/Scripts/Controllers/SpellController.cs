using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats enemyStats;

    public void SetupSpell(CharacterStats _enemyStats)
    {
        enemyStats = _enemyStats;
    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //GameManager.instance.consule_UI.AddDebug("玩家被触手击败了");
                enemyStats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
