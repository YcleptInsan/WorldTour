using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace WorldTour
{
  public class Band
  {
    private string _name;
		private int _id;

    public Band(string Name, int Id = 0)
    {
      _name = Name;
      _id = Id;
    }
		public override bool Equals(System.Object otherBand)
    {
    if (!(otherBand is Band))
    {
      return false;
    }
    	else
    	{
	      Band newBand = (Band) otherBand;
	      bool idEquality = (this.GetId() == newBand.GetId());
	      bool nameEquality = (this.GetName() == newBand.GetName());
	      return (idEquality && nameEquality);
    	}
  	}
		public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

		public static List<Band> GetAll()
		{
			SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd =new SqlCommand("SELECT * FROM bands;", conn);

      SqlDataReader rdr = cmd.ExecuteReader();

			List<Band> allBands = new List<Band>{};
			while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        Band newBand = new Band(bandName, bandId);
        allBands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allBands;
		}
		public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName);", conn);

			SqlParameter bandNameParameter = new SqlParameter();
      bandNameParameter.ParameterName = "@Bandname";
      bandNameParameter.Value = this.GetName();

		  cmd.Parameters.Add(bandNameParameter);

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
		public static Band Find(int IdToFind)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId", conn);
      SqlParameter idParam = new SqlParameter("@BandId", IdToFind);
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      int id = 0;
      string name = null;

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }

      Band newBand = new Band(name, id);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return newBand;
    }
		public void AddVenue(Venue newVenue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venues_id, bands_id) VALUES (@BandId, @VenueId);", conn);

      SqlParameter bandParam = new SqlParameter("@BandId", this.GetId());
      SqlParameter venueParam = new SqlParameter("@VenueId", newVenue.GetId());

      cmd.Parameters.Add(bandParam);
      cmd.Parameters.Add(venueParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Venue> GetVenues()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.bands_id) JOIN venues ON (venue.id = bands_venues.venues_id) WHERE band.id = @BandId;", conn);

      SqlParameter bandIdParameter = new SqlParameter("@BandId", this.GetId());
      cmd.Parameters.Add(bandIdParameter);

      List<Venue> venues = new List<Venue>{};
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);


        Venue newVenue = new Venue(name,  id);
        venues.Add(newVenue);
      }

      if(conn != null)
      {
        conn.Close();
      }
      if(rdr != null)
      {
         rdr.Close();
      }

      return venues;
    }
		public static void DeleteAll()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
			cmd.ExecuteNonQuery();
      conn.Close();
		}
 	}
}
