using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.User;

namespace Btf.Services.LocationService
{
    public interface ILocationService
    {
        IQueryable<Country> GetCountries();
        IQueryable<State> GetState(int countryId);
        IQueryable<City> GetCities(int stateId);
        Task<List<City>> GetAllCities();
        Task<State> GetStateByCityId(int cityId);
        Task<Country> GetCountryByStateId(int stateId);
        Task<City> GetCityByStateAndCityNamesAsync(string stateName, string cityName);
    }
}
