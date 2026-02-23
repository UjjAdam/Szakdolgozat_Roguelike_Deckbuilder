using UnityEngine;

public class HeroView : CombatantView
{
    private HeroData heroData;

    public void Setup(HeroData heroData)
    {
        this.heroData = heroData;
        SetupBase(heroData.Health, heroData.Image);
    }

    public void SetRuinedSprite()
    {
        if (heroData != null && heroData.DeathImage != null)
            SetSprite(heroData.DeathImage);
    }

    private void OnDestroy()
    {
        // avoid HeroSystem keeping a dangling reference to a destroyed scene object
        if (HeroSystem.Instance != null)
            HeroSystem.Instance.UnregisterHeroView(this);
    }
}
