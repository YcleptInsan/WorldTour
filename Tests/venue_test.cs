using Xunit;
using System;
using System.Collections.Generic;

namespace WorldTour
{
  [Collection("WorldTour")]
  public class VenueTests : IDisposable
  {
    public VenueTests()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Venue_GetAll_DatabaseEmptyOnload()
    {
      List<Venue> testList = Venue.GetAll();
      List<Venue> newList = new List<Venue>{};

      Assert.Equal(newList, testList);
    }

    [Fact]
    public void Venue_Save_SaveToDatabase()
    {
      Venue newVenue = new Venue("World Star Tour");
      newVenue.Save();

      Venue testVenue = Venue.GetAll()[0];
      Assert.Equal(newVenue, testVenue);
    }

    [Fact]
    public void Venue_Equals_VenueEqualsVenue()
    {
      Venue newVenue = new Venue("World Star Tour");
      Venue testVenue = new Venue("World Star Tour");

      Assert.Equal(newVenue, testVenue);
    }

    [Fact]
    public void Venue_Find_FindsVenueInDB()
    {
      Venue newVenue = new Venue("World Star Tour");
      newVenue.Save();

      Venue testVenue = Venue.Find(newVenue.GetId());

      Assert.Equal(newVenue, testVenue);
    }
		[Fact]
    public void Venue_AddBand_AddsBandToVenue()
    {
      Venue newVenue = new Venue("World Star Tour");
      newVenue.Save();

      Band newBand1 = new Band("Jenny and the LowRiders");
      newBand1.Save();
      Band newBand2 = new Band("Frankie and the LowRiders");
      newBand2.Save();

      newVenue.AddBand(newBand1);


      List<Band> testList = newVenue.GetBands();
      List<Band> newList = new List<Band>{newBand1, newBand2};

      Assert.Equal(newList, testList);
    }

		public void Dispose()
    {

      Band.DeleteAll();
			Venue.DeleteAll();
    }
	}
}
