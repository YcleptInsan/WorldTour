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

      List<Venue> allVenues = new List<Venue>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Venue newVenue = new Venue(name, id);
        allVenues.Add(newVenue);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allVenues;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@VenueName)", conn);

      SqlParameter nameParameter = new SqlParameter("@VenueName", this.GetName());

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

      int foundVenueId = 0;
      string foundVenueName = null;

      while(rdr.Read())
      {
        foundVenueId = rdr.GetInt32(0);
        foundVenueName = rdr.GetString(1);
      }

      Venue foundVenue  = new Venue(foundVenueName, foundVenueId);

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

      SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId; DELETE FROM bands_venues WHERE venues_id = @VenueId;", conn);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (bands_id, venues_id) VALUES (@BandId, @VenueId);", conn);
      SqlParameter bandIdParameter= new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = newBand.GetId();
      cmd.Parameters.Add(bandIdParameter);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = this.GetId();
      cmd.Parameters.Add(venueIdParameter);
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

      SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venues.id = bands_venues.venues_id) JOIN bands ON (bands.id = bands_venues.bands_id) WHERE venues.id = @VenueId;", conn);

      SqlParameter venueParameter = new SqlParameter();
      venueParameter.ParameterName = "@VenueId";
      venueParameter.Value = this.GetId().ToString();
      cmd.Parameters.Add(venueParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Band> bands = new List<Band>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);


        Band newBand = new Band(name,  id);
        bands.Add(newBand);
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
    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE venues SET name = @NewName OUTPUT INSERTED.name WHERE id = @VenueId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter venuesIdParameter = new SqlParameter();
      venuesIdParameter.ParameterName = "@VenueId";
      venuesIdParameter.Value = this.GetId();
      cmd.Parameters.Add(venuesIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
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
