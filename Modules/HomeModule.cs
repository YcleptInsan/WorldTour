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
      Get["/bands/new"] = _ => {
        return View["band_form.cshtml"];
      };
      Post["/bands/new"] = _ => {
        Band newBand = new Band(Request.Form["band-name"], Request.Form["venue-id"]);
        newBand.Save();
        List<Band> AllBands = Band.GetAll();
        return View["bands.cshtml", AllBands];
      };
      Get["/venues/new"] = _ => {
        return View["venue_form.cshtml"];
      };
      Post["/venues/new"] = _ => {
        Venue newVenue = new Venue(Request.Form["venue-name"]);
        newVenue.Save();
        List<Venue> AllVenues = Venue.GetAll();
        return View["venues.cshtml", AllVenues];
      };
      Get["/bands/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Band SelectedBand = Band.Find(parameters.id);
        List<Venue> BandVenues = SelectedBand.GetVenues();
        List<Venue> AllVenues = Venue.GetAll();
        model.Add("band", SelectedBand);
        model.Add("bandVenues", BandVenues);
        model.Add("allVenues", AllVenues);
        return View["band.cshtml", model];
      }; //returns individual instance of band
      Get["/venues/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        List<Band> VenueBands = SelectedVenue.GetBands();
        List<Band> AllBands = Band.GetAll();
        model.Add("venues", SelectedVenue);
        model.Add("venuesBands", VenueBands);
        model.Add("allBands", AllBands);
        return View["venues.cshtml", model];
      }; //returns individual instance of venues
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
        model.Add("isCompleted", band);
        return View["index.cshtml", model];
      }; //posts from form adding venues
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
      }; //posts from form adding band to venues page
      Get["/venues/edit/{id}"] = parameters => {
        Venue SelectedVenue = Venue.Find(parameters.id);
        return View["venues_edit.cshtml", SelectedVenue];
      }; //edit individual venues
      Patch["/venues/edit/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue SelectedVenue = Venue.Find(parameters.id);
        SelectedVenue.Update(Request.Form["venues-name"]);
        List<Band> VenueBands = SelectedVenue.GetBands();
        List<Band> AllBands = Band.GetAll();
        model.Add("venues", SelectedVenue);
        model.Add("venuesBands", VenueBands);
        model.Add("allBands", AllBands);
        return View["venues.cshtml", model];
      }; //posts from editing individual venues
      Get["/venues/delete/{id}"] = parameters => {
        Venue SelectedVenue = Venue.Find(parameters.id);
        return View["venues_delete.cshtml", SelectedVenue];
      }; //delete individual venues
      Delete["/venues/delete/{id}"] = parameters => {
        Venue SelectedVenue = Venue.Find(parameters.id);
        SelectedVenue.Delete();
        List<Venue> AllVenues = Venue.GetAll();
        return View["venues.cshtml", AllVenues];
      }; //delete individual venues
    }
  }
}
