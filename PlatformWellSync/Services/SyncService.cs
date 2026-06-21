using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlatformWellSync.Data;
using PlatformWellSync.Models;

namespace PlatformWellSync.Services
{
    internal class SyncService
    {
        private readonly ApiService _apiService;
        private readonly ApplicationDbContext _db;

        public SyncService(
            ApiService apiService,
            ApplicationDbContext db)
        {
            _apiService = apiService;
            _db = db;
        }

        public async Task SyncAsync()
        {
            var token = await _apiService.LoginAsync();

            var platforms =
                await _apiService.GetPlatformWellActualAsync(
                    token);

            foreach (var platformDto in platforms)
            {
                await UpsertPlatform(platformDto);
            }

            await _db.SaveChangesAsync();
        }

        private async Task UpsertPlatform(
            PlatformDto platformDto)
        {
            var platform =
                await _db.Platforms
                    .Include(x => x.Wells)
                    .FirstOrDefaultAsync(
                        x => x.Id == platformDto.Id);

            if (platform == null)
            {
                platform = new Platform
                {
                    Id = platformDto.Id,
                    PlatformName = platformDto.PlatformName,
                    CreatedDate = platformDto.CreatedDate,
                    UpdatedDate = platformDto.UpdatedDate
                };

                await _db.Platforms.AddAsync(platform);
            }
            else
            {
                platform.PlatformName =
                    platformDto.PlatformName;

                platform.UpdatedDate =
                    platformDto.UpdatedDate;
            }

            foreach (var wellDto in platformDto.Wells)
            {
                await UpsertWell(wellDto, platformDto.Id);
            }
        }

        private async Task UpsertWell(
            WellDto wellDto,
            int platformId)
        {
            var well =
                await _db.Wells
                    .FirstOrDefaultAsync(
                        x => x.Id == wellDto.Id);

            if (well == null)
            {
                await _db.Wells.AddAsync(new Well
                {
                    Id = wellDto.Id,
                    PlatformId = platformId,
                    WellName = wellDto.WellName,
                    CreatedDate = wellDto.CreatedDate,
                    UpdatedDate = wellDto.UpdatedDate
                });
            }
            else
            {
                well.WellName = wellDto.WellName;
                well.PlatformId = platformId;
                well.UpdatedDate = wellDto.UpdatedDate;
            }
        }
    }
}
