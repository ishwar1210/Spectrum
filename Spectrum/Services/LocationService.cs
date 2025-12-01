using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<(bool Success, string Message, LocationResponseDTO? Location)> GetByIdAsync(int locationId)
    {
        var location = await _locationRepository.GetByIdAsync(locationId);
        if (location == null) return (false, "Location not found", null);
        return (true, "Location retrieved successfully", MapToResponseDTO(location));
    }

    public async Task<(bool Success, string Message, IEnumerable<LocationResponseDTO> Locations)> GetAllAsync()
    {
        var locations = await _locationRepository.GetAllAsync();
        return (true, "Locations retrieved successfully", locations.Select(MapToResponseDTO));
    }

    public async Task<(bool Success, string Message, IEnumerable<LocationResponseDTO> Locations)> GetActiveAsync()
    {
        var locations = await _locationRepository.GetActiveAsync();
        return (true, "Active locations retrieved successfully", locations.Select(MapToResponseDTO));
    }

    public async Task<(bool Success, string Message, int? LocationId)> CreateAsync(CreateLocationDTO createDto)
    {
        if (await _locationRepository.LocationNameExistsAsync(createDto.LocationName))
            return (false, "Location name already exists", null);

        var location = new Location
        {
            LocationName = createDto.LocationName,
            Description = createDto.Description,
            IsActive = createDto.IsActive ?? true
        };

        var locationId = await _locationRepository.CreateAsync(location);
        return (true, "Location created successfully", locationId);
    }

    public async Task<(bool Success, string Message, LocationResponseDTO? Location)> UpdateAsync(int locationId, UpdateLocationDTO updateDto)
    {
        var existing = await _locationRepository.GetByIdAsync(locationId);
        if (existing == null) return (false, "Location not found", null);

        if (!string.IsNullOrWhiteSpace(updateDto.LocationName) && await _locationRepository.LocationNameExistsAsync(updateDto.LocationName, locationId))
            return (false, "Location name already exists", null);

        if (!string.IsNullOrWhiteSpace(updateDto.LocationName)) existing.LocationName = updateDto.LocationName;
        if (updateDto.Description != null) existing.Description = updateDto.Description;
        if (updateDto.IsActive.HasValue) existing.IsActive = updateDto.IsActive.Value;

        var success = await _locationRepository.UpdateAsync(locationId, existing);
        if (!success) return (false, "Failed to update location", null);

        var updated = await _locationRepository.GetByIdAsync(locationId);
        return (true, "Location updated successfully", MapToResponseDTO(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int locationId)
    {
        var existing = await _locationRepository.GetByIdAsync(locationId);
        if (existing == null) return (false, "Location not found");

        var success = await _locationRepository.DeleteAsync(locationId);
        if (!success) return (false, "Failed to delete location");

        return (true, "Location deleted successfully");
    }

    private static LocationResponseDTO MapToResponseDTO(Location location)
    {
        return new LocationResponseDTO
        {
            LocationId = location.LocationId,
            LocationName = location.LocationName,
            Description = location.Description,
            CreatedDate = location.CreatedDate,
            UpdatedDate = location.UpdatedDate,
            IsActive = location.IsActive,
            CreatedBy = location.CreatedBy
        };
    }
}
