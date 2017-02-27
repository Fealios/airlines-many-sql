using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using AirlineApp.Objects;

namespace AirlineApp
{
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void TEST_ChecksIfDBIsEmpty()
    {
      //Arrange, Act
      int result = Flight.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_CheckFlightSaveToDB()
    {
      List <Flight> totalFlights = new List<Flight>{};
      Flight tempFlight = new Flight("complete", "5 am", "seattle", "los angeles");
      totalFlights.Add(tempFlight);

      tempFlight.Save();
      Assert.Equal(totalFlights, Flight.GetAll());
    }

    [Fact]
    public void TEST_FindMethodReturnsSameObject()
    {
        Flight tempFlight = new Flight("complete", "5 am", "seattle", "los angeles");
        tempFlight.Save();
        // Console.WriteLine(tempFlight.GetDestinationCity());

        Flight testFlight = Flight.Find(tempFlight.GetId());
        // Console.WriteLine(testFlight.GetDestinationCity());

        Assert.Equal(tempFlight, testFlight);
    }

    [Fact]
    public void Test_AddCity_AddCityToFlight()
    {
        Flight testFlight = new Flight("complete", "5 am", "seattle", "los angeles");
        testFlight.Save();

        City city1 = new City("LA");
        city1.Save();

        City city2 = new City("SJ");
        city2.Save();

        testFlight.AddCity(city1);
        testFlight.AddCity(city2);

        List<City> testList = new List<City> {city1, city2};
        List<City> result = testFlight.GetCities();

        Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCities_GatAllCities()
    {
        Flight testFlight = new Flight("complete", "5 am", "seattle", "los angeles");
        testFlight.Save();

        City city1 = new City("LA");
        city1.Save();

        City city2 = new City("SJ");
        city2.Save();

        testFlight.AddCity(city1);
        List<City> savedCities = testFlight.GetCities();
        List<City> testList = new List<City> {city1};

        Assert.Equal(savedCities, testList);
    }

    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
