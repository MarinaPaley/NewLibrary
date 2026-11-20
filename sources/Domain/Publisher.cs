// <copyright file="Publisher.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;

    /// <summary>
    /// Издательство.
    /// </summary>
    public sealed class Publisher : NamedEntity<Publisher>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Publisher"/>.
        /// </summary>
        /// <param name="name"> Название. </param>
        /// <exception cref="ArgumentNullException"> В случае <paramref name="name"/> равным <see langword="null"/>. </exception>
        public Publisher(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Книги.
        /// </summary>
        public ISet<Book> Books { get; private set; } = new HashSet<Book>();
    }
}
