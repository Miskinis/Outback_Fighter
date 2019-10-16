using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Components.Combat
{
    public struct HealthBar : IComponentData
    {
        public Entity playerEntity;
    }
    
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    [RequireComponent(typeof(Image))]
    public class HealthBarComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        //public Entity playerEntity;
        //public Image healthBar;

        /*public void RegisterPlayer(Entity entity)
        {
            playerEntity = entity;
        }*/

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new HealthBar());
            //dstManager.AddComponentObject(entity, this);
        }

        /*public void AdjustHealth(float percentage)
        {
            healthBar.fillAmount = percentage;
        }*/
    }
}