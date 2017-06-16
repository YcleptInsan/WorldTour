using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;



namespace WorldTour
{
  [Collection("WorldTour")]
  public class BandTest : IDisposable
  {
    public BandTest()
    {
        DBConfiguration.ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Band_Test_DataBase_Empty_Initially()
    {
      int stage1BandsList = Band.GetAll().Count;

      Assert.Equal(0, stage1BandsList);
    }
    [Fact]
    public void Bands_Test_Save_SaveToDatabase()
    {
      Band newBand = new Band("Jenny and the LowRiders",1 ,1);
      newBand.Save();

      Band testBand = Band.GetAll()[0];
      Assert.Equal(newBand, testBand);
    }
    public void Dispose()
    {
      Band.DeleteAll();
    }
  }
}
