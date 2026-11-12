// <copyright file="Author.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Domain.Abstract;
    using Staff;

    /// <summary>
    /// Автор.
    /// </summary>
    public sealed class Author : NamedEntity<Author>
    {
        private readonly CultureInfo culture = new("ru-RU");

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Author"/>.
        /// </summary>
        /// <param name="name"> Имя.</param>
        /// <param name="familyName"> Фамилия. </param>
        /// <param name="patronymic"> Отчество.</param>
        public Author(string? familyName, string name, string? patronymic)
            : base(name)
        {
            this.FamilyName = familyName;
            this.Patronymic = patronymic;
        }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string? FamilyName
        {
            get;
            set => field = value.RequireValidOrNull();
        }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string? Patronymic
        {
            get;
            set => field = value.RequireValidOrNull();
        }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateOnly? DateBirth { get; set; }

        /// <summary>
        /// Дата смерти.
        /// </summary>
        public DateOnly? DateDeath { get; set; }

        /// <summary>
        /// Книги.
        /// </summary>
        public ISet<Book> Books { get; set; } = new HashSet<Book>();

        /// <summary>
        /// Добавляет книгу Автору.
        /// </summary>
        /// <param name="book"> Книга. </param>
        /// <returns><see langword="true"/>, если добавили, иначе - <see langword="false"/>.</returns>
        public bool AddBook(Book book)
        {
            if (book is null)
            {
                return false;
            }

            return this.Books.Add(book) || book.Authors.Add(this);
        }

        /// <summary>
        /// Удаляет книгу у Автора.
        /// </summary>
        /// <param name="book"> Книга.</param>
        /// <returns><see langword="true"/>, если удалили, иначе - <see langword="false"/>.</returns>
        public bool RemoveBook(Book book)
        {
            if (book is null)
            {
                return false;
            }

            return this.Books.Remove(book) && book.Authors.Remove(this);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Author author && this.Equals(author);

        /// <inheritdoc/>
        public override bool Equals(Author? other)
        {
            return base.Equals(other)
                && string.Equals(this.FamilyName, other.FamilyName, StringComparison.Ordinal)
                && string.Equals(this.Patronymic, other.Patronymic, StringComparison.Ordinal)
                && this.DateBirth == other.DateBirth
                && this.DateDeath == other.DateDeath;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Id == Guid.Empty
                ? HashCode.Combine(this.Name, this.FamilyName, this.Patronymic, this.DateBirth, this.DateDeath)
                : this.Id.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var dateBirth = this.DateBirth.HasValue
                ? $" Год рождения: {this.DateBirth.Value.ToString("dd.MM.yyyy", this.culture)}"
                : string.Empty;

            var dateDeath = this.DateDeath.HasValue
                ? $" Год смерти: {this.DateDeath.Value.ToString("dd.MM.yyyy", this.culture)}"
                : string.Empty;

            return new StringBuilder()
                .Append(this.Name)
                .AppendIf(this.Patronymic is not null, string.Empty)
                .AppendIf(this.FamilyName is not null, string.Empty)
                .AppendIf(this.DateBirth is not null, dateBirth)
                .AppendIf(this.DateDeath is not null, dateDeath)
                .ToString();
        }
    }
}
