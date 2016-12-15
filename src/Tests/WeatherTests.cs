using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather;
using Xunit;

namespace Tests
{
    public class WeatherTests
    {
  

        [Fact]
        public void ShouldReturnWarmWeather()
        {
            var weatherServiceMock = new Mock<IWeatherInfo>();
            weatherServiceMock.Setup(x => x.TemperatureIn("Madrid")).Returns(23);

            var weatherUser = new WeatherUser(weatherServiceMock.Object);

            var result = weatherUser.WeatherString("Madrid");

            Assert.Equal("Het is warm weer in Madrid", result);
        }

        [Fact]
        public void DemoSimpleMockVerifyMethodCall()
        {
            var mock = new Mock<IWeatherInfo>();
            WeatherUser wu = new WeatherUser(mock.Object);

            wu.SubScribeAlarm("TestStad", "docent@hhs.nl");

            mock.Verify(x => x.SubScribeWeatherAlarm("TestStad", "docent@hhs.nl"), Times.Once());
        }

    }
}
