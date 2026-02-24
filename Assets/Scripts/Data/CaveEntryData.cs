using UnityEngine;

[CreateAssetMenu(menuName = "Data/CaveEntry")]
public class CaveEntryData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public CaveEntryType EntryType { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}
