using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Model.User;

namespace Btf.Data.Contracts.Interfaces
{
    public interface ILocationRepository : IRepository<City>
    {
        IQueryable<Country> GetCountries();
        IQueryable<State> GetStates(int countryId);
        IQueryable<City> GetCities(int cityId);
        Task<List<City>> GetAllCitites();
        Task<State> GetStateByCityId(int cityId);
        Task<Country> GetCountryByStateId(int stateId);
        Task<List<State>> GetStatesByNameAsync(string stateName);
    }
}
