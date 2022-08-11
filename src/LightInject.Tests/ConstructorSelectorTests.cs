using System;
using LightInject.SampleLibrary;
using Xunit;
namespace LightInject.Tests
{
    public class ConstructorSelectorTests : TestBase
    {
        [Fact]
        public void Execute_StaticConstructor_IsNotReturned()
        {
            var container = CreateContainer();
            var selector = new MostResolvableConstructorSelector(container.CanGetInstance);
            Assert.Throws<InvalidOperationException>(() => selector.Execute(typeof(FooWithStaticConstructor)));
        }

        [Fact]
        public void Execute_MultipleConstructors_ReturnsMostResolvableConstructor()
        {
            var container = CreateContainer();
            container.RegisterInstance("SomeValue");
            var selector = new MostResolvableConstructorSelector(container.CanGetInstance);
            var constructorInfo = selector.Execute(typeof(FooWithMultipleParameterizedConstructors));
            Assert.Equal(typeof(string), constructorInfo.GetParameters()[0].ParameterType);
        }

        [Fact]
        public void Execute_MultipleConstructors_ThrowsException()
        {
            var container = CreateContainer();
            var selector = new MostResolvableConstructorSelector(container.CanGetInstance);

            var message = Assert.Throws<InvalidOperationException>(
                () => selector.Execute(typeof(FooWithMultipleParameterizedConstructors))).Message;
            Assert.StartsWith("No resolvable", message);
        }

        [Fact]
        public void Execute_MultipleConstructors_UsesParameterNameAsServiceName()
        {
            var container = CreateContainer();
            container.RegisterInstance(42, "SomeValue");
            container.RegisterInstance(84, "IntValue");
            var selector = new MostResolvableConstructorSelector(container.CanGetInstance);
            var constructorInfo = selector.Execute(typeof(FooWithMultipleParameterizedConstructors));
            Assert.Equal(typeof(int), constructorInfo.GetParameters()[0].ParameterType);
        }

        [Fact]
        public void Execute_PrivateConstructor_IsNotReturned()
        {
            var container = CreateContainer();
            var selector = new MostResolvableConstructorSelector(container.CanGetInstance);
            Assert.Throws<InvalidOperationException>(() => selector.Execute(typeof(FooWithPrivateConstructor)));
        }
    }
}
