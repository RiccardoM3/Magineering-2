using UnityEngine;

[CreateAssetMenu(fileName = "New Aspect", menuName = "Inventory/Aspect")]
public class Aspect : ScriptableObject
{
    public string aspectName = "Aspect Name";
    public Sprite icon;
}
