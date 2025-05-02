using log4net;
using NUnit.Framework;
using System;

namespace Expressium.Coffeeshop.Web.API.Tests
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be equal [{0}] and was [{1}] - PASSED", expected, actual);
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be equal [True] and was [True] - PASSED");
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be equal [False] and was [False] - PASSED");
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be equal [Null] and was [Null]");
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be different from [Null] and was [{0}] - PASSED", actual);
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be different from [Empty] and was [{0}] - PASSED", actual);
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be greater than [{0}] and was [{1}] - PASSED", expected, actual);
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

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to be less than [{0}] and was [{1}] - PASSED", expected, actual);
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to be less than [{expected}] but was [{actual}] - FAILED");
            }
        }

        public void DoesContain(string actual, string expected, string message)
        {
            int maxLength = 25;
            string actualTruncated = actual;
            if (actualTruncated.Length > maxLength)
                actualTruncated = actualTruncated.Substring(0, maxLength) + "...";

            try
            {
                Assert.That(actual, Does.Contain(expected), message);

                logger.InfoFormat("{0}", message);
                logger.InfoFormat("Expected to contain [{0}] in [{1}] - PASSED", expected, actualTruncated);
            }
            catch (AssertionException)
            {
                throw new ApplicationException($"{message}\nExpected to contain [{expected}] in [{actualTruncated}] - FAILED");
            }
        }

        public void Inconclusive(string message)
        {
            logger.ErrorFormat(message);
            Assert.Inconclusive(message);
        }

        public void Ignore(string message)
        {
            logger.ErrorFormat(message);
            Assert.Ignore(message);
        }
    }
}
