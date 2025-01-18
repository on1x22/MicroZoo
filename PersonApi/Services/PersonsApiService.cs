﻿using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.PersonsApi.Repository;

namespace MicroZoo.PersonsApi.Services
{
    public class PersonsApiService : IPersonsApiService
    {
        private readonly IPersonRepository _repository;
        public PersonsApiService(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetPersonResponse> GetPersonAsync(int personId)
        {
            var response = new GetPersonResponse
            {
                Person = await _repository.GetPersonAsync(personId)
            };

            if (response.Person == null)
            {
                response.ErrorMessage = $"Person with id = {personId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetPersonResponse> AddPersonAsync(PersonDto personDto)
        {
            var response = new GetPersonResponse();

            if (!await _repository.CheckPersonIsManager(personDto.ManagerId))
            {
                response.ErrorMessage = $"Manager with id={personDto.ManagerId} is not exist";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }
            
            var addedPerson = personDto.ToPerson();

            response.Person = await _repository.AddPersonAsync(addedPerson);
            
            return response;
        }

        public async Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto)
        {
            var response = new GetPersonResponse();

            if (!await _repository.CheckPersonIsManager(personDto.ManagerId))
            {
                response.ErrorMessage = $"Manager with id={personDto.ManagerId} is not exist";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            var personWithChanges = personDto.ToPerson();

            response.Person = await _repository.UpdatePersonAsync(personId, personWithChanges);

            if (response.Person == null)
            {
                response.ErrorMessage = $"Person with id = {personId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }
            return response;
        }

        public async Task<GetPersonResponse> SoftDeletePersonAsync(int personId)
        {
            /*var response = new GetPersonResponse
            {
                Person = await _repository.SoftDeletePersonAsync(personId)
            };

            if (response.Person == null)
                response.ErrorMessage = $" with id = {personId} not found";*/
            var response = new GetPersonResponse();

            var personForDelete = await _repository.GetPersonAsync(personId);
            if (personForDelete == null)
            {
                response.ErrorMessage = $"Person with Id {personId} not found";
                return response;
            }

            response.Person = await _repository.SoftDeletePersonAsync(personForDelete);            

            return response;
        }

        public async Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId)
        {
            var response = new GetPersonsResponse
            {
                Persons = await _repository.GetSubordinatePersonnelAsync(personId)
            };

            if(response.Persons == null || response.Persons.Count() == 0)
            {
                response.Persons = null;
                response.ErrorMessage = $"Employee with id={personId} has no subordinate personnel";
                response.ErrorCode = ErrorCodes.BadRequest400;
            }

            return response;
        }

        public async Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId, 
                                                                             int newManagerId)
        {
            var response = new GetPersonsResponse();

            if (!await _repository.CheckPersonIsManager(currentManagerId))
            {
                response.ErrorMessage = $"Manager with id={currentManagerId} is not exist";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            if (!await _repository.CheckPersonIsManager(newManagerId))
            {
                response.ErrorMessage = $"Manager with id={newManagerId} is not exist";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            response.Persons = await _repository.ChangeManagerForSubordinatePersonnel(currentManagerId,
                                                                                      newManagerId);

            if(response.Persons == null || response.Persons.Count == 0)
            {
                response.Persons = null;
                response.ErrorMessage = $"Manager with id={currentManagerId} has no subordinate " +
                    $"personnel, therefore thera are no changes";
                response.ErrorCode = ErrorCodes.BadRequest400;
            }

            return response;
        }
    }
}
