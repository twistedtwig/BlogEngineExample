using NUnit.Framework;

namespace BlogEngine.Domain.LogicTests
{
    [TestFixture]
    public class AutoMapperTests
    {

        [Test]
        public void EnsureAllMappingsSorted()
        {
            AutoMapperWebConfiguration.Configure();
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
