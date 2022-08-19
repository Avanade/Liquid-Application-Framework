using Liquid.Core.Utils;
using System.Text;
using System.Threading.Tasks;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = false)]

namespace Liquid.Core.Tests
{
    public class JsonUtilTest
    {
        [Fact]
        public void ShouldSerialize_WhenSerializeObject_ReturnJsonString()
        {
            
            //Arrange
            string exectedJsonString = "{\"stringProperty\":\"1\",\"intPropery\":2}";
            var content = new { StringProperty = "1", IntPropery = 2 };

            //Act
            var result = JsonUtils.ToJson(content);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(exectedJsonString, result);
        } 

        [Fact]
        public void ShouldSerialize_WhenSerializeObject_ReturnJsonArrayBytes()
        {
            //Arrange
            var content = new { paramFake1 = "1", paramFake2 = 2 };

            //Act
            var result = JsonUtils.ToJsonBytes(content);

            //Assert
            Assert.IsType<byte[]>(result);
            Assert.True((result as byte[]).IsUTF8());
        }

        [Fact]
        public void ShouldSerialize_WhenSerializeObjectNull_ReturnJsonArrayBytes()
        {
            //Arrange
            var content = new { paramFake1 = "1", paramFake2 = 2 };
            content = null;

            //Act
            var result = JsonUtils.ToJsonBytes(content);

            //Assert
            Assert.IsType<byte[]>(result);
            Assert.NotNull(result);
            Assert.True(result.Length == 0);
        }

        [Fact]
        public void ShouldParseTypedOjectString_WhenDeserializeNullOrWhiteSpace_ReturnNotNull()
        {
            //Arrange
            string content = "{\"stringProperty\":\"1\",\"intPropery\":2}";
            

            //Act
            var result = JsonUtils.ParseTypedOject<dynamic>(content);
            
            Assert.NotNull(result);
            
        }

        [Fact]
        public void ShouldParseTypedOjectString_WhenDeserializeNullOrWhiteSpace_ReturnNull()
        {
            //Arrange
            string content = null;
            string content2 = "";
            string content3 = "     ";

            //Act
            var result = JsonUtils.ParseTypedOject<dynamic>(content);
            var result2 = JsonUtils.ParseTypedOject<dynamic>(content2);
            var result3 = JsonUtils.ParseTypedOject<dynamic>(content3);

            Assert.Null(result);
            Assert.Null(result2);
            Assert.Null(result3);
        }

        [Fact]
        public async Task ShouldParseTypedOjectArrayBytes_WhenSerializeObject_ReturnJsonStringAsync()
        {
            //Arrange
            string exectedJsonString = "{\"stringProperty\":\"1\",\"intPropery\":2}";

            var expectedjsonArrayByte = new UTF8Encoding(false).GetBytes(exectedJsonString);

            //Act
            var result = await expectedjsonArrayByte.ParseTypedOjectAsync<dynamic>().ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldParseTypedOjectArrayBytes_WhenSerializeObject_ReturnJsonString()
        {
            //Arrange
            string exectedJsonString = "{\"stringProperty\":\"1\",\"intPropery\":2}";

            var expectedjsonArrayByte = new UTF8Encoding(false).GetBytes(exectedJsonString);

            //Act
            var result =  expectedjsonArrayByte.ParseTypedOject<dynamic>();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldParseTypedOjectArrayBytes_WhenNullOrEmpty_ReturnJsonStringAsync()
        {
            //Arrange
            byte[] expectedjsonArrayByte = null;

            //Act
            var result = await expectedjsonArrayByte.ParseTypedOjectAsync<dynamic>();
            var result2 = await (new byte[0]).ParseTypedOjectAsync<dynamic>();

            //Assert
            Assert.Null(result);
            Assert.Null(result2);
        }
    }
}
