namespace PetFamily.Domain.Shared;

public static class Errors
{
     public static class General
     {
          public static Error ValueIsInvalid(string? name = null)
          {
               var label = name == null ? "value " : $"{name} ";
               return Error.Validation("value.is.invalid", $"{label}is invalid", name);
          } 
          
          public static Error NotFound(Guid? id = null)
          {
               var forId = id == null ? "" : $" for id '{id}'";
               return Error.Validation("record.not.found", $"record not found{forId}");
          } 
          
          public static Error ValueIsRequired(string? name = null)
          {
               var label = name == null ? "value " : $"{name} ";
               return Error.Validation("value.is.required", $"{label}is required", name);
          } 
     }

     public static class Volunteer
     {
          public static Error AlreadyExist()
          {
               return Error.Validation("record.already.exist", "volunteer already exist");
          }
          
          public static Error NotFound()
          {
               return Error.NotFound("record.not.found", $"volunteer not found");
          }
     }
     
     public static class Pet
     {
          public static Error NotFound()
          {
               return Error.NotFound("record.not.found", $"pet not found");
          }
     }

     public static class Species
     {
          public static Error NotFound()
          {
               return Error.NotFound("record.not.found", $"species not found");
          }
     }
     
     public static class Breeds
     {
          public static Error NotFound()
          {
               return Error.NotFound("record.not.found", $"breeds not found");
          }
     }

     public static class Database
     {
          public static Error AddError()
          {
               return Error.Failure("add.database.error", "error added data to database");
          }
          
          public static Error GetError()
          {
               return Error.Failure("get.database.error", "error getting data to database");
          }
          
          public static Error SaveError()
          {
               return Error.Failure("save.database.error", "error saving data to database");
          }
          
          public static Error DeleteError()
          {
               return Error.Failure("delete.database.error", "error deleting data to database");
          }
     }
}