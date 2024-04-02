﻿using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Infrastructure;
using Infrastructure.Entities;

namespace Application.UseCases.Events;

public class RegisterEventUseCase
{
    public ResponseRegisteredEventJson Execute(RequestEventJson request)
    {
        Validate(request);

        var dbContext = new PassInDbContext();

        var entity = new Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-"),
        };

        dbContext.Events.Add(entity);
        dbContext.SaveChanges();

        return new ResponseRegisteredEventJson
        {
            Id = entity.Id,
        };
    }

    private void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees <= 0)
        {
            throw new PassInException("The maximum attendees is invalid.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new PassInException("The title is invalid");
        }

        if (string.IsNullOrWhiteSpace(request.Details))
        {
            throw new PassInException("The details is invalid");
        }
    }
}