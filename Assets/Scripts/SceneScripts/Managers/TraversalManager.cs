using UnityEngine;

public class TraversalManager : MonoBehaviour
{
    [SerializeField] private CaveEntryUI clickedCaveEntry;
    [SerializeField] private FloorNumberUI floorNumberUI;

    public void SelectAndTravel(CaveEntryUI caveEntryUI)
    {
        clickedCaveEntry = caveEntryUI;
        ClickTravel();
    }

    public void ClickTravel()
    {
        floorNumberUI.IncreaseFloorWhenLeaving();

        if (clickedCaveEntry == null) return;

        if (clickedCaveEntry.CaveEntry.type == CaveEntryType.COMBAT)
        {
            SwitchToCombat();
        }
        else if(clickedCaveEntry.CaveEntry.type == CaveEntryType.HEALSITE)
        {
            SwitchToHeal();
        }
        else if (clickedCaveEntry.CaveEntry.type == CaveEntryType.CHEST)
        {
            SwitchToChest();
        }
        else if (clickedCaveEntry.CaveEntry.type == CaveEntryType.EXIT)
        {
            SwitchToExit();
        }
    }
    public void SwitchToCombat()
    { 
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Combat, setActive: true)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .Perform();
    }

    public void SwitchToHeal()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Heal, setActive: true)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .Perform();
    }

    public void SwitchToChest()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Chest, setActive: true)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithLoading()
            .Perform();
    }

    public void SwitchToExit()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.Exit, setActive: true)
            .UnLoad(SceneDatabase.Slots.SessionContent)
            .WithClearUnusedAssets()
            .WithLoading()
            .Perform();
    }


}
