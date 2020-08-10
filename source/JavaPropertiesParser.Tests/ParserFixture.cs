﻿using System;
using FluentAssertions;
using JavaPropertiesParser.Expressions;
using JavaPropertiesParser.Tests.TestUtils;
using NUnit.Framework;
using Sprache;
using static JavaPropertiesParser.Build;

namespace JavaPropertiesParser.Tests
{
    public class ParserFixture
    {
        [Test]
        public void CanParseEmptyFile()
        {
            var input = ResourceUtils.ReadEmbeddedResource("empty.properties");
            var parsed = Parser.Parse(input);

            var expected = new Document();

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAHashComment()
        {
            var input = ResourceUtils.ReadEmbeddedResource("hash-comment.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                HashComment(" This is a comment")
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAnExclamationComment()
        {
            var input = ResourceUtils.ReadEmbeddedResource("exclamation-comment.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                BangComment(" This is a comment")
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseWhitespace()
        {
            var input = ResourceUtils.ReadEmbeddedResource("blank-line.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Whitespace("\r\n")
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAColonSeparator()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-colon-separator.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseASpaceSeparator()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-space-separator.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(" "),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAnEqualsSeparator()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-equals-separator.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator("="),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAnEqualsSeparatorAndLeadingWhitespace()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-equals-separator-and-leading-whitespace.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(" ="),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAnEqualsSeparatorWithTrailingWhitespace()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-equals-separator-and-trailing-whitespace.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator("= "),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseATabSeparator()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-tab-separator.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator("\t"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseASeparatorWithMultipleWhitespaceCharacters()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-multi-whitespace-separator.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(" 	"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithALogicalNewLine()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-logical-newline.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key\n1", "key\\n1"),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithAPhysicalNewLine()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-physical-newline.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key", "ke\\\r\ny"),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithAPhysicalNewLineAndIndentation()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-physical-newline-and-indentation.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key", "ke\\\r\n   y"),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithAUnicodeEscape()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-unicode-escape.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("keyὅ", "key\\u1f45"),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithNeitherSeparatorNorValue()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-neither-separator-nor-value.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key", "key"),
                    null, //Separator(""),
                    null //Value("", "")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAKeyWithSeparatorButNoValue()
        {
            var input = ResourceUtils.ReadEmbeddedResource("key-with-separator-but-no-value.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key", "key"),
                    Separator(":"),
                    null //Value("", "")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAnEmptyKey()
        {
            var input = ResourceUtils.ReadEmbeddedResource("empty-key.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    null, //Key(""),
                    Separator(":"),
                    Value("value")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseWhitespaceBeforeAComment()
        {
            var input = ResourceUtils.ReadEmbeddedResource("whitespace-before-comment.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Whitespace("   "),
                HashComment(" This comment has leading whitespace")
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAValueWithAUnicodeEscape()
        {
            var input = ResourceUtils.ReadEmbeddedResource("value-with-unicode-escape.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(":"),
                    Value("valũe", "val\\u0169e")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAValueWithALogicalNewLine()
        {
            var input = ResourceUtils.ReadEmbeddedResource("value-with-logical-newline.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(":"),
                    Value("val\r\nue", "val\\r\\nue")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAValueWithAPhysicalNewLine()
        {
            var input = ResourceUtils.ReadEmbeddedResource("value-with-physical-newline.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(":"),
                    Value("value", "val\\\r\nue")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void CanParseAValueWithAPhysicalNewLineAndIndentation()
        {
            var input = ResourceUtils.ReadEmbeddedResource("value-with-physical-newline-and-indentation.properties");
            var parsed = Parser.Parse(input);

            var expected = Doc(
                Pair(
                    Key("key"),
                    Separator(":"),
                    Value("value", "val\\\r\n   ue")
                )
            );

            parsed.Should().Be(expected);
        }

        [Test]
        public void ThrowsExceptionOnInvalidUnicodeEscape()
        {
            var input = ResourceUtils.ReadEmbeddedResource("invalid-unicode-escape.properties");

            Action action = () => Parser.Parse(input);
            action.Should()
                .ThrowExactly<ParseException>();
        }
    }
}