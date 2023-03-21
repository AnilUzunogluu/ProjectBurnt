using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
   [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

   public event Action OnPlateSpawned;
   public event Action OnPlateRemoved;
   
   private float plateSpawnTimer;
   private const float PLATE_SPAWN_TIMER_MAX = 4F;

   private int platesSpawned;
   private const int PLATES_SPAWNED_MAX = 4;

   private void Update()
   {
      plateSpawnTimer += Time.deltaTime;

      if (plateSpawnTimer >= PLATE_SPAWN_TIMER_MAX)
      {
         plateSpawnTimer = 0f;
         
         if (platesSpawned < PLATES_SPAWNED_MAX)
         {
            OnPlateSpawned?.Invoke();
            platesSpawned++;
         }
      }
   }

   public override void Interact(Player player)
   {
      if (!player.HasKitchenObject() && platesSpawned > 0)
      {
         KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
         platesSpawned--;
         OnPlateRemoved?.Invoke();
      }
   }
}
