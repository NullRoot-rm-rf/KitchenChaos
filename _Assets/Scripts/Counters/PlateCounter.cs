using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjSO kitchenObjSO;
    private float counntDownOfPlateSpawning;
    private int plateSpawned;

    private List<GameObject> plateSpawnedList;

    private void Awake()
    {
        plateSpawnedList = new List<GameObject>(); 
    }

    private void Update()
    {
        if (plateSpawned != 4) counntDownOfPlateSpawning += Time.deltaTime;
        if (counntDownOfPlateSpawning > 6f && plateSpawned < 4)
        {
            Transform plateVisual = Instantiate(kitchenObjSO.prefab, GetKitchenObjPlacingPoint());

            plateVisual.transform.localPosition = new Vector3(0, plateSpawned * 0.1f, 0);

            plateSpawnedList.Add(plateVisual.gameObject);

            plateSpawned++;
            counntDownOfPlateSpawning = 0f;
        }
    }

    public override void Interact()
    {
        if (!Player.Instance.HasKitchenObj() && plateSpawned != 0)
        {
            Debug.Log(plateSpawned);
            plateSpawned--;

            KitchenObj.SpawnKitchenObj(kitchenObjSO, Player.Instance);

            GameObject lastPlateSpawned = plateSpawnedList[plateSpawnedList.Count - 1];
            plateSpawnedList.Remove(lastPlateSpawned);
            Destroy(lastPlateSpawned);
        }
    }
}
