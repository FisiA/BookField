using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookPlace.API.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationsService _reservationService;

    public ReservationsController(IReservationsService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize]
    [HttpGet("GetAll/{reservationStatus}")]
    public async Task<IActionResult> GetAllReservations(ReservationState reservationStatus = ReservationState.All)
    {
        var res = await _reservationService.GetAllReservationsAsync(reservationStatus);
        return Ok(res);
    }

    [Authorize]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetReservationById(Guid id)
    {
        var res = await _reservationService.GetReservationByIdAsync(id);
        if(res == null)
        {
            return NotFound();
        }

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(ReservationDTO reservationDetails)
    {
        var newReservation = await _reservationService.CreateReservationAsync(reservationDetails);
        return Ok(newReservation);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(Guid id, ReservationDTO reservationDetails)
    {
        if(id != reservationDetails.Id)
        {
            return BadRequest("Reservation Id mismatch");
        }

        var updatedReservation = await _reservationService.UpdateReservationAsync(reservationDetails);

        if(updatedReservation == null)
        {
            return NotFound();
        }

        return Ok(updatedReservation);
    }

    [HttpPut("ConfirmReservation/{id}")]
    public async Task<IActionResult> ConfirmReservation(Guid id)
    {
        var reservationConfirmed = await _reservationService.ConfirmReservationAsync(id);
        return Ok(reservationConfirmed);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        var reservationDeleted = await _reservationService.DeleteReservationAsync(id);
        return Ok(reservationDeleted);
    }    
}
