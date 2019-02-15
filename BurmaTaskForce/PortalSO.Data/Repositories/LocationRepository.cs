using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btf.Data.Contexts;
using Btf.Data.Contracts.Base;
using Btf.Data.Contracts.Interfaces;
using Btf.Data.Model.User;

namespace Btf.Data.Repositories
{
    public class LocationRepository : BaseRepository<City, LocationContext>, ILocationRepository
    {
        DbSet<Country> Countries;
        DbSet<State> States;
        DbSet<City> Cities;
        public LocationRepository(IUnitOfWork<LocationContext> uow) : base(uow)
        {
            Countries = Context.Set<Country>();
            States = Context.Set<State>();
            Cities = Context.Set<City>();
        }

        public async Task<List<City>> GetAllCitites()
        {
            return await Cities.Include(c => c.State).Where(c => c.IsActive).OrderBy(c => c.CityName).ToListAsync();
        }

        public IQueryable<City> GetCities(int stateId)
        {
            return Cities.Include(c => c.State).Where(c => c.StateId == stateId).OrderBy(c => c.CityName).AsNoTracking();
        }

        public IQueryable<Country> GetCountries()
        {
            return Countries.Where(c => c.IsActive).OrderBy(c => c.CountryName).AsNoTracking();
        }

        public IQueryable<State> GetStates(int countryId)
        {
            return States.Where(s => s.IsActive && s.CountryId == countryId).OrderBy(s => s.StateName).AsNoTracking();
        }

        public async Task<State> GetStateByCityId(int cityId)
        {
            return await States.Include(s => s.Cities)
                .Where(s => s.Cities.Any(c => c.Id == cityId && c.IsActive) && s.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByStateId(int stateId)
        {
            return await Countries.Include(c => c.States)
                .Where(c => c.States.Any(s => s.Id == stateId && s.IsActive) && c.IsActive).FirstOrDefaultAsync();
        }

        public async Task<List<State>> GetStatesByNameAsync(string stateName)
        {
            return await States.Where(s => s.StateName == stateName && s.IsActive).OrderBy(s => s.StateName).ToListAsync();
        }
    }
}
