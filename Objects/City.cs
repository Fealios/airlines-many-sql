using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AirlineApp.Objects
{
  public class City
  {
    private string _name;
    private int _id;

    public City(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public override bool Equals(System.Object otherCity)
    {
        if (!(otherCity is City))
        {
            return false;
        }
        else
        {
            City newCity = (City) otherCity;
            bool idEquality = this.GetId() == newCity.GetId();
            bool nameEquality = this.GetName() == newCity.GetName();

            return (idEquality && nameEquality);
        }
    }


    public static List<City> GetAll()
    {
        List<City> CityList = new List<City> {};

        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            int cityId = rdr.GetInt32(0);
            string name = rdr.GetString(1);
            City newCity = new City(name, cityId);
            CityList.Add(newCity);
        }

        if (rdr != null)
        {
            rdr.Close();
        }
        if (conn != null)
        {
            conn.Close();
        }

        return CityList;
    }


    public void Save()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

        SqlParameter statusParameter = new SqlParameter("@Name", this.GetName());

        cmd.Parameters.Add(statusParameter);

        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            this._id = rdr.GetInt32(0);
        }
        if (rdr != null)
        {
            rdr.Close();
        }
        if(conn != null)
        {
            conn.Close();
        }
    }

    public static City Find(int id)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId", conn);

        SqlParameter idParam = new SqlParameter();
        idParam.ParameterName = "@CityId";
        idParam.Value = id.ToString();
        cmd.Parameters.Add(idParam);

        SqlDataReader rdr = cmd.ExecuteReader();

        int foundCityId = 0;
        string foundName = null;

        while (rdr.Read())
        {
            foundCityId = rdr.GetInt32(0);
            foundName = rdr.GetString(1);
        }

        City foundCity = new City(foundName, foundCityId);

        if (rdr != null)
        {
            rdr.Close();
        }
        if (conn != null)
        {
            conn.Close();
        }

        return foundCity;
    }

    public void AddFlight(Flight newFlight)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO flight_cities (city_id, flight_id) VALUES (@FlightId, @CityId);", conn);

        SqlParameter flightParameter = new SqlParameter("@FlightId", newFlight.GetId());
        SqlParameter cityParameter = new SqlParameter("@CityId", this.GetId());

        cmd.Parameters.Add(flightParameter);
        cmd.Parameters.Add(cityParameter);

        cmd.ExecuteNonQuery();

        if (conn != null)
        {
            conn.Close();
        }
    }

public static void DeleteAll()
{
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
    cmd.ExecuteNonQuery();
    conn.Close();
}

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }


  }
}
