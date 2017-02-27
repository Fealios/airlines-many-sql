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

    public void Dispose()
    {
      Flight.DeleteAll();
      // Category.DeleteAll();
    }
  }
}
