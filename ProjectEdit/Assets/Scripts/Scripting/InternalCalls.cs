using UnityEngine;

using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

using MoonSharp.Interpreter;

using ProjectEdit.Entities;
using Xedrial.Graphics;

namespace ProjectEdit.Scripting
{
    [MoonSharpUserData]
    internal class InternalCalls
    {
        private static EntityManager EntityManager => EntitiesManager.EntityManager;

        public static Entity CreateEmptyEntity() => EntitiesManager.CreateEmptyEntity();

        public static Entity CreateEntity() => EntityManager.CreateEntity();

        public static bool HasComponent(Entity entity, string componentTypeName)
        {
            return StringToComponent(componentTypeName, out ComponentType componentType) 
                   && EntityManager.HasComponent(entity, componentType);
        }

        public static void AddComponent(Entity entity, string componentName)
        {
            switch (componentName)
            {
                case "Transform":
                    EntityManager.AddComponent<LocalTransform>(entity);
                    break;
                case "SpriteRenderer":
                    EntityManager.AddComponent<SpriteRendererComponent>(entity);
                    break;
            }
        } 

        public static Table GetTranslation(Entity entity)
        {
            float3 value = EntityManager.GetComponentData<LocalTransform>(entity).Position;
            
            var script = EntityManager.GetComponentData<ScriptInstance>(entity);
            var table = new Table(script)
            {
                ["x"] = value.x,
                ["y"] = value.y,
                ["z"] = value.z
            };

            return table;
        }

        public static void SetTranslation(Entity entity, Table vector)
        {
            var value = new float3
            (
                (float)vector.Get("x").Number,
                (float)vector.Get("y").Number,
                (float)vector.Get("z").Number
            );

            var transform = EntityManager.GetComponentData<LocalTransform>(entity);
            transform.Position = value;
            EntityManager.SetComponentData(entity, transform);
        }

        public static Table GetRotation(Entity entity)
        {
            float4 value = EntityManager.GetComponentData<LocalTransform>(entity).Rotation.value;
            var script = EntityManager.GetComponentData<ScriptInstance>(entity);
            var table = new Table(script)
            {
                ["x"] = value.x,
                ["y"] = value.y,
                ["z"] = value.z,
                ["w"] = value.w
            };

            return table;
        }

        public static void SetRotation(Entity entity, Table vector)
        {
            var value = new quaternion
            (
                (float)vector.Get("x").Number,
                (float)vector.Get("y").Number,
                (float)vector.Get("z").Number,
                (float)vector.Get("w").Number
            );

            var transform = EntityManager.GetComponentData<LocalTransform>(entity);
            transform.Rotation = value;
            EntityManager.SetComponentData(entity, transform);
        }


        public static Table SpriteRenderer_GetColor(Entity entity)
        {
            Color value = EntityManager.GetComponentObject<SpriteRendererComponent>(entity).Color;
            var script = EntityManager.GetComponentData<ScriptInstance>(entity);
            
            var table = new Table(script)
            {
                [1] = value.r,
                [2] = value.g,
                [3] = value.b,
                [4] = value.a
            };

            return table;
        }

        public static void SpriteRenderer_SetColor(Entity entity, Table vector)
        {
            var value = new Color
            (
                (float)vector.Get(1).Number,
                (float)vector.Get(2).Number,
                (float)vector.Get(3).Number,
                (float)vector.Get(4).Number
            );

            EntityManager.GetComponentObject<SpriteRendererComponent>(entity).Color = value;
        }

        private struct NullComponent : IComponentData
        {
        }
        
        private static bool StringToComponent(string componentName, out ComponentType componentType)
        {
            switch (componentName) 
            {
                case "Transform":
                {
                    componentType = typeof(LocalToWorld);
                    return true;
                }
                case "SpriteRenderer":
                {
                    componentType = typeof(SpriteRendererComponent);
                    return true;
                }
                default:
                {
                    componentType = default;
                    return false;
                }
            }
        }
    }
}
