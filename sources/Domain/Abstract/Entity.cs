// <copyright file="Entity.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    using System;

    /// <summary>
    /// Абстрактный класс Сущность.
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <inheritdoc/>
        public virtual Guid Id { get; set; }
    }
}
