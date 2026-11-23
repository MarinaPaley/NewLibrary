// <copyright file="PublisherTests.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class PublisherTests
    {
        [TestCaseSource(nameof(InvalidNameCases))]
        public void Constructor_InvalidName_Throws(string? name)
        {
            // act & assert
            Assert.Throws<ArgumentException>(() => new Publisher(name!));
        }

        [Test]
        public void Constructor_Valid_InitializesFields()
        {
            // arrange & act
            var publisher = new Publisher("Publisher");

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(publisher.Name, Is.EqualTo("Publisher"));
                Assert.That(publisher.Books, Is.Not.Null);
                Assert.That(publisher.Books, Is.Empty);
            }
        }

        [Test]
        public void ToString_ReturnsBracketedName()
        {
            // act
            var result = new Publisher("Publisher").ToString();

            // assert
            Assert.That(result, Is.EqualTo("[Publisher]"));
        }

        private static IEnumerable<TestCaseData> InvalidNameCases()
        {
            yield return new TestCaseData((string?)null).SetArgDisplayNames("Null");
            yield return new TestCaseData(string.Empty).SetArgDisplayNames("Empty");
            yield return new TestCaseData("   ").SetArgDisplayNames("Whitespace");
        }
    }
}
