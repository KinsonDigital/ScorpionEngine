// <copyright file="EntityPool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Factories;

    /// <summary>
    /// Creates a pool of entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that the pool will contain.</typeparam>
    /// <remarks>
    ///     This is used for performance when large quantities of entities are needed,
    ///     such as bullets or waves of enemies.
    /// </remarks>
    public class EntityPool<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly Dictionary<Guid, TEntity> entitites = new Dictionary<Guid, TEntity>();
        private readonly IContentLoader contentLoader;
        private readonly IEntityFactory entityFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPool{TEntity}"/> class.
        /// </summary>
        /// <param name="contentLoader">Loads content for newly created entities.</param>
        /// <param name="entityFactory">Generates entity instances.</param>
        public EntityPool(IContentLoader contentLoader, IEntityFactory entityFactory)
        {
            this.contentLoader = contentLoader;
            this.entityFactory = entityFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPool{TEntity}"/> class.
        /// </summary>
        /// <remarks>
        ///     A content loaders is automatically provided.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public EntityPool()
        {
            this.contentLoader = ContentLoaderFactory.CreateContentLoader();
            this.entityFactory = IoC.Container.GetInstance<IEntityFactory>();
        }

        /// <summary>
        /// Gets or sets the total amount of items that the pool can contain.
        /// </summary>
        /// <remarks>
        ///     The ideal number is just enough items to satisfy the purpose of the pool.
        /// <para>
        ///     Example: If the bullets from a player ship move fast enough across the entire
        ///     window where it is only possibly to visibly see 100 bullets at one time, then
        ///     the ideal <see cref="MaxPoolSize"/> would be 100-110.
        /// </para>
        /// </remarks>
        public int MaxPoolSize { get; set; } = 10;

        /// <summary>
        /// Gets the total number of active entities.
        /// </summary>
        /// <remarks>
        ///     An entity is considered active when it is visible or enabled.
        /// </remarks>
        public int TotalActive
        {
            get
            {
                var result = 0;

                var keys = this.entitites.Keys.ToArray();

                for (var i = 0; i < keys.Length; i++)
                {
                    if (this.entitites[keys[i]].Visible && this.entitites[keys[i]].Enabled)
                    {
                        result += 1;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the totla number of inactive entities.
        /// </summary>
        /// <remarks>
        ///     An entity is considered inactive it is both hidden and disabled.
        /// </remarks>
        public int TotalInactive
        {
            get
            {
                var result = 0;

                var keys = this.entitites.Keys.ToArray();

                for (var i = 0; i < keys.Length; i++)
                {
                    if (!this.entitites[keys[i]].Visible && !this.entitites[keys[i]].Enabled)
                    {
                        result += 1;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the list of entities in the pool.
        /// </summary>
        public ReadOnlyCollection<TEntity> Entitities => new ReadOnlyCollection<TEntity>(this.entitites.Values.ToArray());

        /// <summary>
        /// Generates an entity that can animate from a texture that matches the given
        /// <paramref name="atlasName"/> that uses the sub textures in the texture atlas for animation frames.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the graphical content.</param>
        /// <param name="subTextureName">The name of the sub textures in the atlas to use.</param>
        public void GenerateAnimated(string atlasName, string subTextureName)
        {
            if (this.entitites.Count >= MaxPoolSize)
            {
                return;
            }

            GenerateEntity(
                () => RenderSection.CreateAnimatedSubTexture(atlasName, subTextureName),
                () => this.entityFactory.CreateAnimated<TEntity>(atlasName, subTextureName));
        }

        /// <summary>
        /// Generates an entity that does not animate from a texture that matches the given
        /// <paramref name="atlasName"/> that uses a single sub texture in the texture atlas.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the graphical content.</param>
        /// <param name="subTextureName">The name of the sub textures in the atlas to use.</param>
        public void GenerateNonAnimatedFromTextureAtlas(string atlasName, string subTextureName)
        {
            if (this.entitites.Count >= MaxPoolSize)
            {
                return;
            }

            GenerateNonAnimatedFromTextureAtlas(atlasName, subTextureName, (_) => { });
        }

        /// <summary>
        /// Generates an entity that does not animate from a texture that matches the given
        /// <paramref name="atlasName"/> that uses a single sub texture in the texture atlas.
        ///
        /// <para>
        /// Once the entity has been created, gives te ability to apply changes to the entity via
        /// the execution of the <paramref name="onGenerate"/> action delegate.
        /// </para>
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the graphical content.</param>
        /// <param name="subTextureName">The name of the sub textures in the atlas to use.</param>
        /// <param name="onGenerate">Used to perform manipulation on the new entity after creation.</param>
        public void GenerateNonAnimatedFromTextureAtlas(string atlasName, string subTextureName, Action<TEntity> onGenerate)
        {
            if (this.entitites.Count >= MaxPoolSize)
            {
                return;
            }

            GenerateEntity(
                () => RenderSection.CreateNonAnimatedSubTexture(atlasName, subTextureName),
                () =>
                {
                    var entity = (TEntity)this.entityFactory.CreateNonAnimatedFromTextureAtlas<TEntity>(atlasName, subTextureName);

                    if (entity is null)
                    {
                        throw new NullReferenceException($"The generated entity from '{atlasName}' with sub texture '{subTextureName}' cannot be null.");
                    }

                    onGenerate(entity);

                    return entity;
                });
        }

        /// <summary>
        /// Generates an entity that does not animate from an entire texture.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        public void GenerateNonAnimatedFromTexture(string textureName)
        {
            if (this.entitites.Count >= MaxPoolSize)
            {
                return;
            }

            GenerateEntity(
                () => RenderSection.CreateNonAnimatedWholeTexture(textureName),
                () => this.entityFactory.CreateNonAnimatedFromTexture<TEntity>(textureName));
        }

        /// <summary>
        /// Updates all of the entities in the pool.
        /// </summary>
        /// <param name="gameTime">The amount of time passed this current frame and the game.</param>
        public void Update(GameTime gameTime)
        {
            foreach (var entity in this.entitites.Values)
            {
                entity.Update(gameTime);
            }
        }

        /// <summary>
        /// Renders all of the entitites in the pool.
        /// </summary>
        /// <param name="renderer">Used to render the entities.</param>
        public void Render(IRenderer renderer)
        {
            foreach (var entity in this.entitites.Values)
            {
                renderer.Render(entity);
            }
        }

        /// <summary>
        /// Generates an entity using content that matches the given texture name.
        /// </summary>
        /// <param name="generateSection">Generates a <see cref="RenderSection"/>.</param>
        /// <param name="generateEntity">Generates an entity.</param>
        /// <returns>A new entity or one from the pool.</returns>
        private TEntity GenerateEntity(Func<RenderSection> generateSection, Func<IEntity> generateEntity)
        {
            var success = TryTake(out TEntity? newEntity);

            if (success && !(newEntity is null))
            {
                // TODO: This might not be necessary if it is reassinged on line below.
                // Look into removing this
                newEntity.SectionToRender.Reset();
                newEntity.SectionToRender = generateSection();
            }
            else
            {
                newEntity = (TEntity)generateEntity();
            }

            newEntity.Init();
            newEntity.LoadContent(this.contentLoader);

            if (!this.entitites.ContainsKey(newEntity.ID))
            {
                this.entitites.Add(newEntity.ID, newEntity);
            }

            return newEntity;
        }

        /// <summary>
        /// Attemptes to take an available/recycled entity from the pool.
        /// If an available entity is not found, returns a null entity and false.
        /// </summary>
        /// <param name="entity">The recycled entity from the pool.</param>
        /// <returns>
        ///     <see langword="true"/> if an available entity was found.
        /// </returns>
        private bool TryTake(out TEntity? entity)
        {
            foreach (var currentEntity in this.entitites.Values)
            {
                if (!currentEntity.Visible && !currentEntity.Enabled)
                {
                    currentEntity.Visible = true;
                    currentEntity.Enabled = true;
                    entity = currentEntity;

                    return true;
                }
            }

            entity = null;

            return false;
        }
    }
}
