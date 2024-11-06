using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using SampleWebApi.Models.Domain;
using System.Linq;


namespace SampleWebApi.Data.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> AddWalkAsync(Walk walk);
        Task<IEnumerable<Walk>> GetAllAsync(string? filterOn=null, string? filterQuery = null, string? sortBy=null,bool isAscending=true
            , int pageNumber = 1, int pageSize = 1000 );
        Task<Walk> GetByIdAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> DeleteAsync(Guid id);


    }
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext) {
            this._nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id=Guid.NewGuid();
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk=await _nZWalksDbContext.Walks.Where(e=>e.Id==id).FirstOrDefaultAsync();
            if (existingWalk == null) {
                return null;
            }

            _nZWalksDbContext.Walks.Remove(existingWalk);
            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //var walks=await _nZWalksDbContext.Walks
            //    .Include(x=>x.Difficulty)
            //    .Include(x=>x.Region)
            //    .ToListAsync();

            var walks = _nZWalksDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false) 
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                
            }
            //Sorting

            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks=isAscending? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            //Pagination
            var skipResults=(pageNumber-1)* pageSize;


            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
            var existingWalk = await _nZWalksDbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .Where(e => e.Id == id).FirstOrDefaultAsync();
            if (existingWalk == null)
            {
                return null;
            }
            return existingWalk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _nZWalksDbContext.Walks.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (existingWalk != null)
            {
                existingWalk.LengthInKm = walk.LengthInKm;
                existingWalk.Name = walk.Name;
                existingWalk.DifficultyId = walk.DifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await _nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return existingWalk;
        }
    }
}
