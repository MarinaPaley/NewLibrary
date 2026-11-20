// <copyright file="StringExtensions.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Staff
{
    /// <summary>
    /// Расширение для строкового типа (<see cref="string"/>).
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Обертка над <see cref="string.IsNullOrEmpty(string?)"/>.
        /// </summary>
        /// <param name="value"> Параметр. </param>
        /// <returns>
        /// <see langword="true"/> если <paramref name="value"/> <see langword="null"/> или пустая строка.
        /// </returns>
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

        /// <summary>
        /// Возвращает строку без начальных и конечных пробелов, или <see langword="null"/>.
        /// </summary>
        /// <param name="value"> Строка.</param>
        /// <returns>
        /// Или без начальных и конечных пробелов строка, или <see langword="null"/>.
        /// </returns>
        public static string? TrimOrNull(this string? value)
        {
            var trimmed = value?.Trim();

            return trimmed.IsNullOrEmpty()
                 ? null
                 : trimmed;
        }

        /// <summary>
        /// Возвращает строку без пробелов по краям, если она валидна; <see langword="null"/> — если вход был <see langword="null"/>.
        /// Бросает исключение, если после <see cref="string.Trim()"/> строка пуста или содержит символы,
        /// отличные от букв латиницы и кириллицы.
        /// </summary>
        /// <param name="value"> Проверяемая строка. </param>
        /// <returns> Обрезанная строка или <see langword="null"/>. </returns>
        /// <exception cref="ArgumentException">
        /// Если после обрезки строка пуста или содержит недопустимые символы.
        /// </exception>
        public static string? RequireValidOrNull(this string? value)
        {
            if (value is null)
            {
                return null;
            }

            var trimmed = value.Trim();

            if (trimmed.Length == 0)
            {
                throw new ArgumentException("Строка не может быть пустой или состоять только из пробелов.", nameof(value));
            }

            foreach (var character in trimmed)
            {
                if (!IsAllowedLetter(character))
                {
                    throw new ArgumentException($"Строка содержит недопустимый символ '{character}'.", nameof(value));
                }
            }

            return trimmed;
        }

        private static bool IsAllowedLetter(char character) =>
               char.IsAsciiLetter(character)
            || character is >= 'А' and <= 'я'
            || character is 'Ё' or 'ё';
    }
}
