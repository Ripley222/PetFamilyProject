﻿namespace PetFamily.Application.Volunteers.DTOs;

public record SocialNetworksDto
{
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}