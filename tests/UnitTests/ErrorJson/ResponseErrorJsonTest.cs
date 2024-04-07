using Communication.Responses;
using Xunit;

namespace UnitTests.ErrorJson;

public class ResponseErrorJsonTest
{
    [Fact]
    public void Sucess()
    {
        string expectedMessage = "Error message";

        var response = new ResponseErrorJson(expectedMessage);

        Assert.NotNull(response);
        Assert.IsType<ResponseErrorJson>(response);
        Assert.IsType<string>(response.Message);
        Assert.Equal(expectedMessage, response.Message);
    }
}