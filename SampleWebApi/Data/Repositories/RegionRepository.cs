using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models.Domain;

namespace SampleWebApi.Data.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetByIdAsync(Guid id);
        Task<Region> AddRegionAsync(Region region);
        Task<Region?> UpdateRegionAsync(Region region, Guid id);

        Task<Region?> DeleteRegionAsynch(Guid id);

    }
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this._nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddRegionAsync(Region region)
        {
            await _nZWalksDbContext.Regions.AddAsync(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsynch(Guid id)
        {
            var region = await _nZWalksDbContext.Regions.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (region == null)
            {
                return null;
            }

            _nZWalksDbContext.Regions.Remove(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await _nZWalksDbContext.Regions.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Region?> UpdateRegionAsync(Region region, Guid id)
        {
            var existingReg = await _nZWalksDbContext.Regions.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (existingReg == null)
            {
                return null;
            }

            existingReg.Code = region.Code;
            existingReg.RegionImageUrl = region.RegionImageUrl;
            existingReg.Name = region.Name;

            _nZWalksDbContext.Update(existingReg);
            await _nZWalksDbContext.SaveChangesAsync();
            return existingReg;
        }
    }
}
