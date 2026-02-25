using UnityEngine;

public class InteractionSystem : Singleton<InteractionSystem>
{
    public bool PlayerIsDragging { get; set; } = false;
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.isPerforming)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerCanHover()
    {
        if (PlayerIsDragging)
        {
            return false;
        }
        return true;
    }
}
