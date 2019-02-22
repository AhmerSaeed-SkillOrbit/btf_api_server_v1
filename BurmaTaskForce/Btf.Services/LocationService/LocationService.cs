using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.User;

namespace Btf.Services.LocationService
{
    public class LocationService : ILocationService
    {
        private ILocationRepository locationRepo;

        public LocationService(ILocationRepository locationRepo)
        {
            this.locationRepo = locationRepo;
        }

        public async Task<List<City>> GetAllCities()
        {
            return await locationRepo.GetAllCitites();
        }

        public IQueryable<City> GetCities(int stateId)
        {
            return locationRepo.GetCities(stateId);
        }

        public async Task<City> GetCityByStateAndCityNamesAsync(string stateName, string cityName)
        {
            List<State> states = await locationRepo.GetStatesByNameAsync(stateName);
            City city = null;
            if (!states.Any())
            {
                //city = new City
                //{
                //    CityName = cityName,
                //    CreatedOn = DateTime.UtcNow,
                //    UpdatedOn = DateTime.UtcNow,
                //    IsActive = true,
                //    State = new State
                //    {
                //        StateName = stateName,
                //        CreatedOn = DateTime.UtcNow,
                //        UpdatedOn = DateTime.UtcNow,
                //        IsActive = true
                //    }
                //};
                //locationRepo.Add(city);

                //locationRepo.GetUow<LocationContext>().Commit();
            }
            else
            {
                foreach (var stateItem in states)
                {
                    var cities = locationRepo.GetCities(stateItem.Id);
                    city = cities.FirstOrDefault(c => c.CityName == cityName);
                    if (city != null)
                    {
                        return city;
                    }
                    else
                    {
                        city = new City
                        {
                            CityName = cityName,
                            CreatedOn = DateTime.UtcNow,
                            UpdatedOn = DateTime.UtcNow,
                            IsActive = true,
                            State = stateItem
                        };
                        locationRepo.Add(city);
                        locationRepo.GetUow<LocationContext>().Commit();
                    }
                }
            }
            return city;
        }

        public IQueryable<Country> GetCountries()
        {
            return locationRepo.GetCountries();
        }

        public async Task<Country> GetCountryByStateId(int stateId)
        {
            return await locationRepo.GetCountryByStateId(stateId);
        }

        public IQueryable<State> GetState(int countryId)
        {
            return locationRepo.GetStates(countryId);
        }

        public async Task<State> GetStateByCityId(int cityId)
        {
            return await locationRepo.GetStateByCityId(cityId);
        }
    }
}
