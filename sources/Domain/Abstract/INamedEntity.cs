// <copyright file="INamedEntity.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    /// <summary>
    /// Интерфейс именованной сущности.
    /// </summary>
    public interface INamedEntity : IEntity
    {
        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; set; }
    }
}
