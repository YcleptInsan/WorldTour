using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WorldTour
{
  public class Venue
  {
    private int _id;
    private string _name;

  public Venue(string Name, int id = 0)
  {
    _id = id;
    _name = Name;
  }
  public int GetId()
  {
    return _id;
  }
  public string GetName()
  {
    return _name;
  }

  public void SetName(string Name)
  {
    _name = Name;
  }


  public override bool Equals(System.Object otherVenue)
    {
      if(!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool nameEquality = this.GetName() == newVenue.GetName();
        bool idEquality = this.GetId() == newVenue.GetId();

        return (nameEquality && idEquality);
      }
    }
    public static List<Venue> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Venue> venues = new List<Venue>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Venue newVenue = new Venue(name, id);
        venues.Add(newVenue);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return venues;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@Name)", conn);

      SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());

      cmd.Parameters.Add(nameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

    }
    public static Venue Find(int idToFind)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId", conn);
      SqlParameter idParam = new SqlParameter("@VenueId", idToFind);
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      int id = 0;
      string name = null;

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }

      Venue foundVenue  = new Venue(name, id);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundVenue;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId; DELETE FROM band_venues WHERE venue_id = @VenueId;", conn);
      SqlParameter idParam = new SqlParameter("@VenueId", this.GetId());
      cmd.Parameters.Add(idParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
    // public void AddVenue(Venue newVenue)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO patrons_venues (patron_id, venue_id) VALUES (@PatronId, @VenueId);", conn);
    //
    //   SqlParameter patronParam = new SqlParameter("@PatronId", this.GetId());
    //   SqlParameter venueParam = new SqlParameter("@VenueId", newVenue.GetId());
    //
    //   cmd.Parameters.Add(patronParam);
    //   cmd.Parameters.Add(venueParam);
    //   cmd.ExecuteNonQuery();
    //
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public List<Venue> GetVenue()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT venues.* FROM patrons JOIN patrons_venues ON (patrons.id = patrons_venues.patron_id) JOIN venues ON (venues.id = patrons_venues.venue_id) WHERE patrons.id = @PatronId;", conn);
    //
    //   SqlParameter patronParameter = new SqlParameter("@PatronId", this.GetId());
    //   cmd.Parameters.Add(patronParameter);
    //
    //   List<Venue> venues = new List<Venue>{};
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     int id = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     string author = rdr.GetString(2);
    //     DateTime dueDate = rdr.GetDateTime(3);
    //
    //     Venue newVenue = new Venue(name, author,dueDate, id);
    //     venues.Add(newVenue);
    //   }
    //
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    //   if(rdr != null)
    //   {
    //      rdr.Close();
    //   }
    //
    //   return venues;
    // }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM venues;", conn);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
  }
}
