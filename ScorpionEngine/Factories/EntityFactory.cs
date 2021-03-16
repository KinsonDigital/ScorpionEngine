// <copyright file="EntityFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Factories
{
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;

    /// <summary>
    /// Creates new instances of entities.
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// Creates a non animating entity with a texture atlas with the given <paramref name="atlasName"/>
        /// and sub texture in the atlas with the given <paramref name="subTextureName"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="atlasName">The name of the texture atlas that contains the image of the entity.</param>
        /// <param name="subTextureName">The name of the sub texture in the atlas.</param>
        /// <returns>A new entity of the type <typeparamref name="TEntity"/>.</returns>
        public static IEntity CreateNonAnimatedFromTextureAtlas<TEntity>(string atlasName, string subTextureName)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = null;
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }

        /// <summary>
        /// Creates a non animated entity with an entire texture that matches the name <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="name">The name fo the texture used for the entity.</param>
        /// <returns>A new entity of the type <typeparamref name="TEntity"/>.</returns>
        public static IEntity CreateNonAnimatedFromTexture<TEntity>(string name)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = name;
            newEntity.SectionToRender.SubTextureName = string.Empty;
            newEntity.SectionToRender.Animator = null;
            newEntity.SectionToRender.TypeOfTexture = TextureType.WholeTexture;

            return newEntity;
        }

        /// <summary>
        /// Creates an animated entity with a texture atlas with the given <paramref name="atlasName"/>
        /// and sub texture in the atlas with the given <paramref name="subTextureName"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="atlasName">The name of the texture atlas that contains the image of the entity.</param>
        /// <param name="subTextureName">The name of the sub texture in the atlas.</param>
        /// <returns>A new entity of the type <typeparamref name="TEntity"/>.</returns>
        public static IEntity CreateAnimated<TEntity>(string atlasName, string subTextureName)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = new Animator();
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }

        /// <summary>
        /// Creates an animated entity with a texture atlas with the given <paramref name="atlasName"/>
        /// and sub texture in the atlas with the given <paramref name="subTextureName"/> and with animation
        /// that is controlled with the given <paramref name="animator"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="atlasName">The name of the texture atlas that contains the image of the entity.</param>
        /// <param name="subTextureName">The name of the sub texture in the atlas.</param>
        /// <param name="animator">The animator used to control the animation.</param>
        /// <returns>A new entity of the type <typeparamref name="TEntity"/>.</returns>
        public static IEntity CreateAnimated<TEntity>(string atlasName, string subTextureName, IAnimator animator)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = animator;
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }
    }
}
