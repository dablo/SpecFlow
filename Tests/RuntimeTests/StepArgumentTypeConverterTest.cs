using System;
using System.Collections.Generic;
using System.Globalization;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;

namespace TechTalk.SpecFlow.RuntimeTests
{
    [TestFixture]
    public class StepArgumentTypeConverterTests
    {
        private IStepArgumentTypeConverter _stepArgumentTypeConverter;
        private CultureInfo _enUSCulture;

        [SetUp]
        public void SetUp()
        {
            Mock<IBindingRegistry> bindingRegistryStub = new Mock<IBindingRegistry>();
            List<StepTransformationBinding> stepTransformations = new List<StepTransformationBinding>();
            bindingRegistryStub.Setup(br => br.StepTransformations).Returns(stepTransformations);

            _stepArgumentTypeConverter = new StepArgumentTypeConverter(new Mock<ITestTracer>().Object, bindingRegistryStub.Object, new Mock<IContextManager>().Object);
            _enUSCulture = new CultureInfo("en-US");
        }

        [Test]
        public void ShouldConvertStringToStringType()
        {
            var result = _stepArgumentTypeConverter.Convert("testValue", typeof(string), _enUSCulture);
            Assert.That(result, Is.EqualTo("testValue"));
        }

        [Test]
        public void ShouldConvertStringToIntType()
        {
            var result = _stepArgumentTypeConverter.Convert("10", typeof(int), _enUSCulture);
            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void ShouldConvertStringToDateType()
        {
            var result = _stepArgumentTypeConverter.Convert("2009/10/06", typeof(DateTime), _enUSCulture);
            Assert.That(result, Is.EqualTo(new DateTime(2009, 10, 06)));
        }

        [Test]
        public void ShouldConvertStringToFloatType()
        {
            var result = _stepArgumentTypeConverter.Convert("10.01", typeof(float), _enUSCulture);
            Assert.That(result, Is.EqualTo(10.01f));
        }

        private enum TestEnumeration
        {
            Value1
        }

        [Test]
        public void ShouldConvertStringToEnumerationType()
        {
            var result = _stepArgumentTypeConverter.Convert("Value1", typeof(TestEnumeration), _enUSCulture);
            Assert.That(result, Is.EqualTo(TestEnumeration.Value1));
        }

        [Test]
        public void ShouldConvertStringToEnumerationTypeWithDifferingCase()
        {
            var result = _stepArgumentTypeConverter.Convert("vALUE1", typeof(TestEnumeration), _enUSCulture);
            Assert.That(result, Is.EqualTo(TestEnumeration.Value1));
        }
    }
}