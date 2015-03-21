using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Testing
{
    public class AutoMock<T> where T : class
    {
        private readonly Type _typeToMock = typeof(T);
        private readonly Dictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public AutoMock()
        {
            MockDependencies();
        }

        private void MockDependencies()
        {
            var constructor = _typeToMock.GetConstructors().First();
            var constructorValues = new List<object>();
            foreach (var param in constructor.GetParameters())
            {
                var dependencyType = param.ParameterType;
                var mockInstance = CreateMock(dependencyType);

                constructorValues.Add(mockInstance.Object);

                _mocks.Add(dependencyType, mockInstance);
            }

            this.Object = (T)constructor.Invoke(constructorValues.ToArray());
        }

        private static Mock CreateMock(Type dependencyType)
        {
            var notSoGenericMock = typeof(Mock<>).MakeGenericType(dependencyType);
            return (Mock)notSoGenericMock.GetConstructor(new Type[0]).Invoke(new object[0]);
        }

        public Mock<TDependency> GetMock<TDependency>() where TDependency : class
        {
            return (Mock<TDependency>)_mocks[typeof(TDependency)];
        }

        public T Object { get; private set; }
    }
}
