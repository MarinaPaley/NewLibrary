// <copyright file="IEntity{TEntity}.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    /// <summary>
    /// Curiously Recurring Template Pattern (CRTP).
    /// Интерфейс «знает», какой конкретный тип его реализует.
    /// TEntity — это «я сам, но в будущем».
    /// </summary>
    /// <typeparam name="TEntity"> Конкретный тип сущности. </typeparam>
    public interface IEntity<TEntity> : IEntity
        where TEntity : class, IEntity<TEntity>
#pragma warning disable MA0206 // Remove unnecessary braces in type declaration
    {
#pragma warning restore MA0206 // Remove unnecessary braces in type declaration
    }
}
