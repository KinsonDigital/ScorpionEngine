using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using KDScorpionEngine.Factories;
using KDScorpionEngine.Graphics;
using Raptor.Content;
using Raptor.Factories;
using Raptor.Graphics;

namespace KDScorpionEngine.Entities
{
    public class EntityPool<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly Dictionary<Guid, TEntity> entitites = new Dictionary<Guid, TEntity>();
        private readonly IContentLoader contentLoader;

        public EntityPool()
        {
            this.contentLoader = ContentLoaderFactory.CreateContentLoader();
        }

        public int MaxSize { get; set; } = 10;

        public int TotalActiveItems => entitites.Where(e => e.Value.IsRecycled).Count();

        public int TotalInactiveItems => entitites.Where(e => !e.Value.IsRecycled).Count();

        public TEntity GenerateAnimated(string atlasName, string subTextureName)
        {
            if (TryTake<TEntity>(out var entity))
            {
                entity.AtlasData = this.contentLoader.Load<IAtlasData>(atlasName);
                entity.RenderSection = new RenderSection()
                {
                    IsAnimated = true,
                    TypeOfTexture = TextureType.WholeTexture,
                };
                entity.Init();

                return entity;
            }

            IEntity newEntity = new TEntity
            {
                AtlasData = this.contentLoader.Load<IAtlasData>(atlasName)
            };

            this.entitites.Add(newEntity.ID, (TEntity)newEntity);

            return (TEntity)newEntity;
        }

        public TEntity GenerateNonAnimatedFromTextureAtlas<T>(string atlasName, string subTextureName)
            where T : class, IEntity, new()
        {
            if (TryTake<TEntity>(out var entity))
            {
                if (entity is null)
                {
                    //TODO: Possibly create a custom exception named GenerateEntityException
                    throw new Exception($"The generated entity for atlas '{atlasName}' and sub texture '{subTextureName}' cannot be null.");
                }

                entity.AtlasData = this.contentLoader.Load<IAtlasData>(atlasName);
                entity.RenderSection = new RenderSection()
                {
                    IsAnimated = false,
                    TypeOfTexture = TextureType.WholeTexture,
                };
                entity.Init();

                return entity;
            }

            IEntity newEntity = new T();

            newEntity.Init();
            newEntity.LoadContent(contentLoader);

            this.entitites.Add(newEntity.ID, (TEntity)newEntity);

            return (TEntity)newEntity;
        }

        public IEntity GenerateNonAnimatedFromTexture(string name)
        {
            if (TryTake<TEntity>(out var entity))
            {
                entity.Texture = this.contentLoader.Load<ITexture>(name);
                entity.RenderSection = new RenderSection()
                {
                    IsAnimated = false,
                    TypeOfTexture = TextureType.SubTexture,
                };
                entity.Init();

                return entity;
            }

            IEntity newEntity = new TEntity
            {
                Texture = this.contentLoader.Load<ITexture>(name)
            };

            this.entitites.Add(newEntity.ID, (TEntity)newEntity);

            return (TEntity)newEntity;
        }

        public void Recycle(TEntity obj)
        {
            if (entitites.ContainsKey(obj.ID))
            {
                // Reset the obj
                entitites[obj.ID].Recycle();
            }
            else
            {
                entitites.Add(obj.ID, obj);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entitites.Values)
            {
                if (!entity.IsRecycled)
                {
                    entity.Update(gameTime);
                }
            }
        }

        public void Render(Renderer renderer)
        {
            foreach (var entity in entitites.Values)
            {
                if (!entity.IsRecycled)
                {
                    renderer.Render(entity);
                }
            }
        }

        private bool TryTake<T>(out TEntity? entity)
            where T : class, IEntity
        {
            foreach (var currentEntity in entitites.Values)
            {
                if (currentEntity.IsRecycled && currentEntity.GetType() == typeof(T))
                {
                    entity = currentEntity;

                    return true;
                }
            }

            entity = null;

            return false;
        }
    }
}
