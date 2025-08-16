﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Move;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using PetFamily.Application.VolunteersFeatures.Update.MainInfo;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;
using PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<AddPetFileHandler>();
        services.AddScoped<GetPetFileLinkHandler>();
        services.AddScoped<DeletePetFileHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<MovePetHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}