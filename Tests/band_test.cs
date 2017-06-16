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
        DBConfiguration.ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=band_tracker;Integrated Security=SSPI;";
    }
    [Fact]
    public void Band_Test_DataBase_Empty_Initially()
    {
      int stage1BandsList = Band.GetAll().Count;

      Assert.Equal(0, stage1BandsList);
    }
    [Fact]
    public void Band_Test_Save_SaveToDatabase()
    {
      Band newBand = new Band("Jenny and the LowRiders");
      newBand.Save();

      Band testBand = Band.GetAll()[0];
      Assert.Equal(newBand, testBand);
    }
    [Fact]
    public void Band_Test_CheckForDuplicate_True()
    {
      Band newBand= new Band("Jenny and the LowRiders");
      Band testBand = new Band("Jenny and the LowRiders");

      Assert.Equal(newBand.GetName(), testBand.GetName());
    }
    public void Dispose()
    {
      Band.DeleteAll();
    }
  }
}
