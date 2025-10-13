using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.TrainerViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;

        public TrainerService(IGenericRepository<Trainer> trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public bool CreateTrainers(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Specialities = createdTrainer.Specialties,
                    Address = new Address()
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        Street = createdTrainer.Street,
                        City = createdTrainer.City
                    }
                };
                return _trainerRepository.Add(trainer) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _trainerRepository.GetAll();
            if (trainers is null || !trainers.Any()) return [];

            var trainerViewModels = trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialities.ToString()
            });
            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _trainerRepository.GetById(trainerId);
            if (trainer is null) return null;

            var trainerViewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialities.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = trainer.Address is not null ? $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}" : null,
                JobTitle = $"{trainer.Specialities} Trainer"
            };
            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _trainerRepository.GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialities,
                DateOfBirth = trainer.DateOfBirth,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City
            };
        }

        public bool RemoveTrainer(int trainerId)
        {
            try
            {
                var trainer = _trainerRepository.GetById(trainerId);
                if (trainer is null) return false;

                var hasFutureSessions = trainer.TrainerSessions.Any(s => s.CreatedAt > DateTime.Now);
                if (hasFutureSessions) return false;

                return _trainerRepository.Delete(trainer) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel updatedTrainer)
        {
            try
            {
                if (IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone)) return false;

                var trainer = _trainerRepository.GetById(id);
                if (trainer is null) return false;

                trainer.Name = updatedTrainer.Name;
                trainer.Email = updatedTrainer.Email;
                trainer.Phone = updatedTrainer.Phone;
                trainer.Specialities = updatedTrainer.Specialties;
                trainer.DateOfBirth = updatedTrainer.DateOfBirth;
                trainer.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                trainer.Address.Street = updatedTrainer.Street;
                trainer.Address.City = updatedTrainer.City;
                return _trainerRepository.Update(trainer) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _trainerRepository.GetAll(m => m.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _trainerRepository.GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
