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
        public DateOnly? DateBirth
        {
            get;
            set
            {
                if (value.HasValue && this.DateDeath.HasValue && value.Value >= this.DateDeath.Value)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Дата рождения должна быть раньше даты смерти.");
                }

                field = value;
            }
        }

        /// <summary>
        /// Дата смерти.
        /// </summary>
        public DateOnly? DateDeath
        {
            get;
            set
            {
                if (value.HasValue && this.DateBirth.HasValue && value.Value <= this.DateBirth.Value)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Дата смерти должна быть позже даты рождения.");
                }

                field = value;
            }
        }

        /// <summary>
        /// Книги.
        /// </summary>
        public ISet<Book> Books { get; private set; } = new HashSet<Book>();

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

            var added = this.Books.Add(book);
            book.Authors.Add(this);

            return added;
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

            var removed = this.Books.Remove(book);
            book.Authors.Remove(this);

            return removed;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var dateBirth = this.DateBirth.HasValue
                ? $" Дата рождения: {this.DateBirth.Value.ToString("dd.MM.yyyy", this.culture)}"
                : string.Empty;

            var dateDeath = this.DateDeath.HasValue
                ? $" Дата смерти: {this.DateDeath.Value.ToString("dd.MM.yyyy", this.culture)}"
                : string.Empty;

            return new StringBuilder()
                .Append(this.Name)
                .AppendIf(this.Patronymic is not null, $" {this.Patronymic}")
                .AppendIf(this.FamilyName is not null, $" {this.FamilyName}")
                .AppendIf(this.DateBirth is not null, dateBirth)
                .AppendIf(this.DateDeath is not null, dateDeath)
                .ToString();
        }

        /// <inheritdoc/>
        protected override bool EqualsFields(Author other)
        {
            return base.EqualsFields(other)
                && string.Equals(this.FamilyName, other.FamilyName, StringComparison.Ordinal)
                && string.Equals(this.Patronymic, other.Patronymic, StringComparison.Ordinal)
                && this.DateBirth == other.DateBirth
                && this.DateDeath == other.DateDeath;
        }

        /// <inheritdoc/>
        protected override int GetHashCodeFields()
        {
            return HashCode.Combine(
                base.GetHashCodeFields(),
                this.FamilyName,
                this.Patronymic,
                this.DateBirth,
                this.DateDeath);
        }
    }
}
