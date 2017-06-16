using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace WorldTour
{
  public class Band
  {
    private string _name;
		private int _venueId;
		private int _id;

    public Band(string Name, int VenueId, int Id = 0)
    {
      _name = Name;
      _venueId = VenueId;
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
	      bool venueEquality = this.GetVenueId()== newBand.GetVenueId();
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

    public int GetVenueId()
    {
      return _venueId;
    }
    public void SetVenueId(int newVenueId)
    {
      _venueId = newVenueId;
    }
		public static List<Band> GetAll()
		{
			List<Band> allBands = new List<Band>{};

			SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd =new SqlCommand("SELECT * FROM bands;", conn);

      SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        int bandVenueId = rdr. GetInt32(2);
        Band newBand = new Band(bandName, bandVenueId, bandId);
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
