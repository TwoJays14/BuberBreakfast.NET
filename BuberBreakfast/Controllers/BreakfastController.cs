using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[controller]")] // [controller] is a token that is replaced with the name of the controller, in this case "Breakfast"

public class BreakfastController : ApiController
{
    private readonly IBreakfastService _breakfastService;
    
    public BreakfastController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }
    
    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // using request body to create a new Breakfast object in the shape we want it to be
        var breakfast = new Breakfast(
            Guid.NewGuid(), 
            request.Name, 
            request.Description, 
            request.StartDateTime, 
            request.EndDateTime, 
            DateTime.Now, 
            request.Savoury, 
            request.Sweet
            );
        
        //TODO: save breakfast to database
        ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

        return createBreakfastResult.Match(
            created => CreatedAsGetBreakfast(breakfast),
            errors => Problem(errors));
        
        return CreatedAsGetBreakfast(breakfast);
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors));
       
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(UpsertBreakfastRequest request, Guid id)
    {
        var breakfast = new Breakfast(id, request.Name, request.Description, request.StartDateTime, request.EndDateTime, DateTime.Now, request.Savoury, request.Sweet);
        
        ErrorOr<UpsertedBreakfastResponse> upsertedBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);
        
        // TODO: return 201 if new breakfast created
        return upsertedBreakfastResult.Match(
            upserted => upserted.isNewlyCreated  ?CreatedAsGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors));
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deletedBreakfastResult = _breakfastService.DeleteBreakfast(id);
        return deletedBreakfastResult.Match(
            deleted => NoContent(),
            errors => Problem(errors));

    }
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        var response = new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedDateTime, breakfast.Savoury, breakfast.Sweet);
        return response;
    }
    private CreatedAtActionResult CreatedAsGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(actionName: nameof(GetBreakfast), routeValues: new {id = breakfast.Id}, value: MapBreakfastResponse(breakfast));
    }
}
