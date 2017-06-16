using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace WorldTour
{
  public class Band
  {

		private int _id;
    private string _name;
		private int _venueId;

    public Band(string Name, int Id = 0)
    {
      _name = Name;
      _venueId = 0;
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
	      return (idEquality && nameEquality && venueEquality);
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
 	}
}
