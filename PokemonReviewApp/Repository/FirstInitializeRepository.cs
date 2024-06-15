using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class FirstInitializeRepository : IFirstInitializeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly RepositoryUtils _repositoryUtility;
        private readonly IAuthRepository _authrepository;

        public FirstInitializeRepository(DataContext context, IMapper mapper, RepositoryUtils repositoryUtility, IAuthRepository authrepository)
        {
            _context = context;
            _mapper = mapper;
            _repositoryUtility = repositoryUtility;
            _authrepository = authrepository;
        }

        public async Task InjectDefaultCity(List<City> data_)
        {
            var existingCityIds = await _context.Cities.Select(c => c.Id).ToListAsync();

            data_.ForEach(city =>
            {
                if (!existingCityIds.Contains(city.Id))
                    _context.Cities.Add(city);
                else
                    _context.Cities.Update(city);
            });

            await _context.SaveChangesAsync();
        }

        public async Task InjectDefaultCountry(List<Country> data_)
        {
            var existingCountryIds = await _context.Countries.Select(c => c.Id).ToListAsync();

            data_.ForEach(country =>
            {
                if (!existingCountryIds.Contains(country.Id))
                    _context.Countries.Add(country);
                else
                    _context.Countries.Update(country);
            });

            await _context.SaveChangesAsync();
        }

        public async Task InjectDefaultStandardValue(List<StandardValue> data_)
        {
            var existingStandardValueIds = await _context.StandardValues.Select(s => s.Id).ToListAsync();

            data_.ForEach(standardvalue =>
            {
                if (!existingStandardValueIds.Contains(standardvalue.Id))
                    _context.StandardValues.Add(standardvalue);
                else
                    _context.StandardValues.Update(standardvalue);
            });

            await _context.SaveChangesAsync();
        }

        public async Task InjectDefaultZipCode(List<Zipcode> data_)
        {
            var existingZipCodes = await _context.Zipcodes.Select(z => z.Id).ToListAsync();

            data_.ForEach(zip =>
            {
                if (!existingZipCodes.Contains(zip.Id))
                    _context.Zipcodes.Add(zip);
                else
                    _context.Zipcodes.Update(zip);
            });

            await _context.SaveChangesAsync();
        }

        public async Task InjectDefaultModule(List<Module> data_)
        {
            var existingModule = await _context.Modules.Select(m => m.Id).ToListAsync();

            data_.ForEach(module =>
            {
                if (!existingModule.Contains(module.Id))
                    _context.Modules.Add(module);
                else
                    _context.Modules.Update(module);
            });

            await _context.SaveChangesAsync();
        }

        public async Task InjectDefaultMenu(List<Menu> data_)
        {
            var existingMenus = await _context.Menus.Select(m => m.Id).ToListAsync();

            data_.ForEach(menu =>
            {
                if (!existingMenus.Contains(menu.Id))
                    _context.Menus.Add(menu);
                else
                    _context.Menus.Update(menu);
            });

            await _context.SaveChangesAsync();
        }
    }
}
