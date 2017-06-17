using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace WorldTour
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/bands"] = _ => {
        List<Band> AllBands = Band.GetAll();
        return View["bands.cshtml", AllBands];
      };
      Get["/venues"] = _ => {
        List<Venue> AllVenues = Venue.GetAll();
        return View["venues.cshtml", AllVenues];
      };
      Get["/venues/new"] = _ => {
        return View["venue_form.cshtml"];
      };
      Post["/venues/new"] = _ => {
        Venue newVenue = new Venue(Request.Form["venue-name"]);
        newVenue.Save();
        return View["success.cshtml", newVenue];
      };
      Get["/bands/new"] = _ => {
        List<Band>  AllBands = Band.GetAll();
        return View["bands_form.cshtml", AllBands];
      };
      Post["/bands/new"] = _ => {
        Band newBand = new Band(Request.Form["band-name"], Request.Form["venue-id"]);
        newBand.Save();
        List<Band> AllBands = Band.GetAll();
        return View["success.cshtml", AllBands];
      };
      Get["/bands/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedVenue = Venue.Find(parameters.id);
        var BandVenues = SelectedVenue.GetBands();
        model.Add("venue", SelectedVenue);
        model.Add("bands", BandVenues);
        return View["band.cshtml", model];
      };
      Get["/venues/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        List<Band> VenueBands = SelectedVenue.GetBands();
        List<Band> AllBands = Band.GetAll();
        model.Add("venues", SelectedVenue);
        model.Add("venuesBands", VenueBands);
        model.Add("allBands", AllBands);
        return View["venues.cshtml", model];
      };

      Post["/band/{id}/add_venues"] = parameters => {
        Venue venues = Venue.Find(Request.Form["venues-id"]);
        Band band = Band.Find(Request.Form["band-id"]);
        band.AddVenue(venues);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Venue> bandVenues = band.GetVenues();
        List<Venue> allVenues = Venue.GetAll();
        model.Add("band", band);
        model.Add("bandVenues", bandVenues);
        model.Add("allVenues", allVenues);
        return View["index.cshtml", model];
      };
      Post["/venues/{id}/add_band"] = parameters => {
        Venue venues = Venue.Find(Request.Form["venues-id"]);
        Band band = Band.Find(Request.Form["band-id"]);
        venues.AddBand(band);
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        List<Band> VenueBands = SelectedVenue.GetBands();
        List<Band> AllBands = Band.GetAll();
        model.Add("venues", SelectedVenue);
        model.Add("venuesBands", VenueBands);
        model.Add("allBands", AllBands);
        return View["venues.cshtml", model];
      };
      Get["/venues/edit/{id}"] = parameters => {
        Venue SelectedVenue = Venue.Find(parameters.id);
        return View["venues_edit.cshtml", SelectedVenue];
      };
      Patch["/venues/edit/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        SelectedVenue.Update(Request.Form["venue-id"]);
        List<Band> VenueBands = SelectedVenue.GetBands();
        List<Band> AllBands = Band.GetAll();
        model.Add("venues", SelectedVenue);
        model.Add("venuesBands", VenueBands);
        model.Add("allBands", AllBands);
        return View["venues.cshtml", model];
      };
      Get["/venues/delete/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        List<Band> VenueBands = SelectedVenue.GetBands();
        model.Add("venue", SelectedVenue);
        model.Add("bands", VenueBands);
        return View["venues_delete.cshtml", model];
      };
      Delete["/venues/delete/{id}"] = parameters => {
        Venue SelectedVenue = Venue.Find(parameters.id);
        SelectedVenue.Delete();
        List<Venue> AllVenues = Venue.GetAll();
        return View["venues.cshtml", AllVenues];
      };
    }
  }
}
