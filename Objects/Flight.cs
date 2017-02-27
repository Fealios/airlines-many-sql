using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AirlineApp.Objects
{
    public class Flight
    {
        private string _status;
        private string _departure_time;
        private string _departure_city;
        private string _destination_city;
        private int _id;

        public Flight(string status, string departure_time, string departure_city, string destination_city, int id = 0)
        {
            _status = status;
            _departure_time = departure_time;
            _departure_city = departure_city;
            _destination_city = destination_city;
            _id = id;
        }

        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                bool departTimeEquality = this.GetDepartureTime() == newFlight.GetDepartureTime();
                bool departCityEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
                bool destCityEquality = this.GetDestinationCity() == newFlight.GetDestinationCity();

                return (idEquality && statusEquality && departTimeEquality && departCityEquality && destCityEquality);
            }
        }


        public static List<Flight> GetAll()
        {
            List<Flight> FlightList = new List<Flight> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int flightId = rdr.GetInt32(0);
                string status = rdr.GetString(1);
                string departTime = rdr.GetString(2);
                string departCity = rdr.GetString(3);
                string destCity = rdr.GetString(4);
                Flight newFlight = new Flight(status, departTime, departCity, destCity, flightId);
                FlightList.Add(newFlight);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return FlightList;
        }


        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO flights (status, departure_time, departure_city, destination_city) OUTPUT INSERTED.id VALUES (@Status, @Departure_time, @Departure_city, @Destination_city);", conn);

            SqlParameter statusParameter = new SqlParameter("@Status", this.GetStatus());
            SqlParameter deptTimeParameter = new SqlParameter("@Departure_time", this.GetDepartureTime());
            SqlParameter deptCityParameter = new SqlParameter("@Departure_city", this.GetDepartureCity());
            SqlParameter destCityParameter = new SqlParameter("@Destination_city", this.GetDestinationCity());

            cmd.Parameters.Add(statusParameter);
            cmd.Parameters.Add(deptTimeParameter);
            cmd.Parameters.Add(deptCityParameter);
            cmd.Parameters.Add(destCityParameter);

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

        public static Flight Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId", conn);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@FlightId";
            idParam.Value = id.ToString();
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundFlightId = 0;
            string foundStatus = null;
            string foundDepartTime = null;
            string foundDepartCity = null;
            string foundDestCity = null;

            while (rdr.Read())
            {
                foundFlightId = rdr.GetInt32(0);
                foundStatus = rdr.GetString(1);
                foundDepartTime = rdr.GetString(2);
                foundDepartCity = rdr.GetString(3);
                foundDestCity = rdr.GetString(4);
            }

            Flight foundFlight = new Flight(foundStatus, foundDepartTime, foundDepartCity, foundDestCity, foundFlightId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundFlight;
        }

    public static void DeleteAll()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public string GetStatus()
    {
        return _status;
    }

    public string GetDepartureTime()
    {
        return _departure_time;
    }

    public string GetDepartureCity()
    {
        return _departure_city;
    }

    public string GetDestinationCity()
    {
        return _destination_city;
    }

    public int GetId()
    {
        return _id;
    }

}
}
