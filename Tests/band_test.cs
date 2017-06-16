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
        DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Band_Test_DataBase_Empty_Initially()
    {
      int stage1BandsList = Band.GetAll().Count;

      Assert.Equal(0, stage1BandsList);
    }
    public void Dispose()
    {
      Band.DeleteAll();
    }
  }
}
