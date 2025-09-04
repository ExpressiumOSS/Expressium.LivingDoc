using log4net;
using NUnit.Framework;
using System;

namespace Expressium.LivingDoc.UITests.Utilities
{
    public class Asserts
    {
        private readonly ILog logger;

        public Asserts(ILog logger)
        {
            this.logger = logger;
        }

        public void EqualTo(object actual, object expected, string message)
        {
            try
            {
                Assert.That(actual, Is.EqualTo(expected), message);

                logger.Info($"{message}");
                logger.Info($"Expected to be equal [{expected}] and was [{actual}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be equal [{expected}] but was [{actual}] - FAILED");
            }
        }

        public void IsTrue(bool actual, string message)
        {
            try
            {
                Assert.That(actual, Is.True, message);

                logger.Info($"{message}");
                logger.Info($"Expected to be equal [True] and was [True] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be equal [True] but was [False] - FAILED");
            }
        }

        public void IsFalse(bool actual, string message)
        {
            try
            {
                Assert.That(actual, Is.False, message);

                logger.Info($"{message}");
                logger.Info($"Expected to be equal [False] and was [False] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be equal [False] but was [True] - FAILED");
            }
        }

        public void IsNull(object actual, string message)
        {
            try
            {
                Assert.That(actual, Is.Null, message);

                logger.Info($"{message}");
                logger.Info($"Expected to be equal [Null] and was [Null]");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be equal [Null] but was [{actual}]");
            }
        }

        public void IsNotNull(string actual, string message)
        {
            try
            {
                Assert.That(actual, Is.Not.Null, message);

                logger.Info($"{message}");
                logger.Info($"Expected to be different from [Null] and was [{actual}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be be different from [Null] but was [Null] - FAILED");
            }
        }

        public void IsNotEmpty(string actual, string message)
        {
            try
            {
                Assert.That(actual, Is.Not.Empty, message);

                logger.Info($"{message}");
                logger.Info($"Expected to be different from [Empty] and was [{actual}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be be different from [Empty] but was [Empty] - FAILED");
            }
        }

        public void GreaterThan(int actual, int expected, string message)
        {
            try
            {
                Assert.That(actual, Is.GreaterThan(expected), message);

                logger.Info($"{message}");
                logger.Info($"Expected to be greater than [{expected}] and was [{actual}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be greater than [{expected}] but was [{actual}] - FAILED");
            }
        }

        public void LessThan(int actual, int expected, string message)
        {
            try
            {
                Assert.That(actual, Is.LessThan(expected), message);

                logger.Info($"{message}");
                logger.Info($"Expected to be less than [{expected}] and was [{actual}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be less than [{expected}] but was [{actual}] - FAILED");
            }
        }

        public void DoesContain(string actual, string expected, string message)
        {
            var actualTruncated = Truncate(actual, 25);

            try
            {
                Assert.That(actual, Does.Contain(expected), message);

                logger.Info($"{message}");
                logger.Info($"Expected to contain [{expected}] in [{actualTruncated}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to contain [{expected}] in [{actualTruncated}] - FAILED");
            }
        }

        public void DoesNotContain(string actual, string expected, string message)
        {
            var actualTruncated = Truncate(actual, 25);

            try
            {
                Assert.That(actual, Does.Not.Contain(expected), message);

                logger.Info($"{message}");
                logger.Info($"Expected to not contain [{expected}] in [{actualTruncated}] - PASSED");
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to not contain [{expected}] in [{actualTruncated}] - FAILED");
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (value == null)
                return string.Empty;

            if (value.Length > maxLength)
                return value.Substring(0, maxLength) + "...";

            return value;
        }
    }
}
