using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlineApp.Objects;

namespace AirlineApp
{
    public class CityTest : IDisposable
    {
        public CityTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_CheckDBIsEmpty()
        {
            int result = City.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void TEST_SaveCityToDB()
        {
            List <City> totalCitys = new List<City>{};
            City tempCity = new City("Seattle");
            totalCitys.Add(tempCity);

            tempCity.Save();
            Assert.Equal(totalCitys, City.GetAll());
        }

        [Fact]
        public void TEST_FindMethodReturnsSameCity()
        {
            City tempCity = new City("Seattle");
            tempCity.Save();
            // Console.WriteLine(tempCity.GetDestinationCity());

            City testCity = City.Find(tempCity.GetId());
            // Console.WriteLine(testCity.GetDestinationCity());

            Assert.Equal(tempCity, testCity);
        }

        public void Dispose()
        {
            City.DeleteAll();
        }
    }
}
