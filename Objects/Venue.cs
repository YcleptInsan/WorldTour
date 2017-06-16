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
    public void AddBand(Band newBand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id) VALUES (@BandId, @VenueId);", conn);

      SqlParameter bandParam = new SqlParameter("@BandId", this.GetId());
      SqlParameter venueParam = new SqlParameter("@VenueId", newBand.GetId());

      cmd.Parameters.Add(bandParam);
      cmd.Parameters.Add(venueParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Band> GetBands()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venue.id = bands_venues.venue_id) JOIN bands ON (band.id = bands_venues.band_id) WHERE venue.id = @VenueId;", conn);

      SqlParameter venueParameter = new SqlParameter("@VenueId", this.GetId());
      cmd.Parameters.Add(venueParameter);

      List<Band> bands = new List<Band>{};
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);


        Band newBand = new Band(name,  id);
        venues.Add(newBand);
      }

      if(conn != null)
      {
        conn.Close();
      }
      if(rdr != null)
      {
         rdr.Close();
      }

      return bands;
    }
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
