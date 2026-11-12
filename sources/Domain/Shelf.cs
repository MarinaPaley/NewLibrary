// <copyright file="Shelf.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;

    /// <summary>
    /// Полка.
    /// </summary>
    public sealed class Shelf : NamedEntity<Shelf>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Shelf"/>.
        /// </summary>
        /// <param name="name"> Название.</param>
        /// <exception cref="ArgumentNullException"> В случае <paramref name="name"/> равным <see langword="null"/>. </exception>
        public Shelf(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Книги.
        /// </summary>
        public ISet<Book> Books { get; set; } = new HashSet<Book>();

        /// <summary>
        /// Добавляет книгу.
        /// </summary>
        /// <param name="book"> Книга. </param>
        /// <returns><see langword="true"/>, если добавили, иначе - <see langword="false"/>.</returns>
        public bool AddBook(Book book)
        {
            if (book is null)
            {
                return false;
            }

            book.Shelf = this;
            return this.Books.Add(book);
        }

        /// <summary>
        /// Удаляет книгу.
        /// </summary>
        /// <param name="book"> Книга.</param>
        /// <returns><see langword="true"/>, если удалили, иначе - <see langword="false"/>.</returns>
        public bool RemoveBook(Book book)
        {
            if (book is null)
            {
                return false;
            }

            book.Shelf = null;
            return this.Books.Remove(book);
        }
    }
}
