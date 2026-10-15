public class DeathBringerAnimationTrigger : Enemy_AnimationTrigger
{
    private DeathBringer deathBringer => GetComponentInParent<DeathBringer>();

    private void Teleport()
    {
        deathBringer.FindTeleportPosition();
    }

    private void MakeInvisible()
    {
        deathBringer.fx.MakeEntityTransparent(true);
    }

    private void MakeVisible()
    {
        deathBringer.fx.MakeEntityTransparent(false);
    }
}
