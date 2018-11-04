using UnityEngine;

public class Duplicatable : MonoBehaviour
{
  public event System.Action<GameObject> Duplicated;

  [SerializeField]
  private PrefabAsset _duplicatePrefab = null;

  public GameObject CreateDuplicate()
  {
    CritterController critter = GetComponent<CritterController>();
    GameObject dupe;
    if (critter != null)
    {
      dupe = CritterSpawner.Instance.SpawnCritter(critter.GetDNA(), null);
    }
    else
    {
      dupe = Instantiate(_duplicatePrefab.Prefab);
      dupe.transform.SetParent(transform.parent);
    }

    Duplicated?.Invoke(dupe);
    return dupe;
  }
}