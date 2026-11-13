// <copyright file="INamedEntity{TEntity}.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    /// <summary>
    /// Шаблонизированный интерфейс именованной сущности.
    /// </summary>
    /// <typeparam name="TEntity"> Конкретный тип именованной сущности. </typeparam>
    public interface INamedEntity<TEntity> : INamedEntity
        where TEntity : class, INamedEntity<TEntity>
#pragma warning disable MA0206 // Remove unnecessary braces in type declaration
    {
#pragma warning restore MA0206 // Remove unnecessary braces in type declaration
    }
}
