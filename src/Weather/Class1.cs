using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather
{
    public interface IWeatherInfo
    {
        int TemperatureIn(string city);
        string SubScribeWeatherAlarm(string city, string email);
    }

    public class WeatherUser
    {
        IWeatherInfo weatherInfoFromTheInternet;

        public WeatherUser(IWeatherInfo weatherInfoProvider) 			//dependency injection
        {
            this.weatherInfoFromTheInternet = weatherInfoProvider;
        }

        public string WeatherString(string city)
        {
            int temperature = weatherInfoFromTheInternet.TemperatureIn(city);
            if (temperature > 20)
            {
                return string.Format("Het is warm weer in {0}", city);
            }
            else
            {
                return string.Format("Het is koud weer in {0}", city);
            }        
        }

        public void SubScribeAlarm(string city, string email)
        {
         	weatherInfoFromTheInternet.SubScribeWeatherAlarm(city, email);
        }



    }
}
