// <copyright file="GenreTests.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class GenreTests
    {
        [TestCaseSource(nameof(InvalidNameCases))]
        public void Constructor_InvalidName_Throws(string? name)
        {
            // act & assert
            Assert.Throws<ArgumentException>(() => new Genre(name!));
        }

        [Test]
        public void Constructor_Valid_InitializesFields()
        {
            // arrange & act
            var genre = new Genre("Fantasy");

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(genre.Name, Is.EqualTo("Fantasy"));
                Assert.That(genre.Books, Is.Not.Null);
                Assert.That(genre.Books, Is.Empty);
            }
        }

        [Test]
        public void ToString_ReturnsBracketedName()
        {
            // act
            var result = new Genre("Fantasy").ToString();

            // assert
            Assert.That(result, Is.EqualTo("[Fantasy]"));
        }

        private static IEnumerable<TestCaseData> InvalidNameCases()
        {
            yield return new TestCaseData((string?)null).SetArgDisplayNames("Null");
            yield return new TestCaseData(string.Empty).SetArgDisplayNames("Empty");
            yield return new TestCaseData("   ").SetArgDisplayNames("Whitespace");
        }
    }
}
