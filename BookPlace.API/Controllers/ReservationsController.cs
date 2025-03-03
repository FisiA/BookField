using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Enum;
using BookPlace.Core.DTO;
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

    [HttpGet("GetAll/{reservationStatus}")]
    public async Task<IActionResult> GetAllReservations(ReservationState reservationStatus = ReservationState.All)
    {
        var res = await _reservationService.GetAllReservationsAsync(reservationStatus);
        return Ok(res);
    }

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
        return CreatedAtAction(nameof(GetReservationById), new { id = newReservation.Id }, newReservation);
    }

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        var reservationDeleted = await _reservationService.DeleteReservationAsync(id);
        return Ok(reservationDeleted);
    }    
}
