using UnityEngine;
[CreateAssetMenu(fileName = "剑气-火", menuName = "数据/物品效果/剑气-火")]
public class Sword_FlameEffect : ItemEffect
{
    [SerializeField] private GameObject FireFlame;
    public override void ReleaseSwordArcane()
    {
        Player player = PlayerManager.instance.player;
        GameObject fireflame = Instantiate(FireFlame, player.transform.position, Quaternion.identity);
        if(player.facingDirection == -1){
            fireflame.transform.Rotate(0, 180, 0);
        }
        fireflame.GetComponentInChildren<Animator>().SetInteger("ComboAttack", player.primaryAttackState.comboCounter);
        Destroy(fireflame, 0.7f);
    }
}
