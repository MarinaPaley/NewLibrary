// <copyright file="Book.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    /// <summary>
    /// Книга.
    /// </summary>
    public sealed class Book
    {
        private const int Value = 42;

        /// <inheritdoc/>
        public override string ToString() => $"value is {Value}";
    }
}
