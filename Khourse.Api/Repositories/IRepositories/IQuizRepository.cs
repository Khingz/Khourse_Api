using System;
using Khourse.Api.Dtos.QuizDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Repositories.IRepositories;

public interface IQuizRepository
{
    Task<List<Quiz>> GetAllAsync(Guid moduleId);
    Task<Quiz> CreateAsync(Quiz resource);
    Task<Quiz?> GetByIdAsync(Guid quizId, Guid moduleId);
    Task<Quiz?> UpdateAsync(Guid id, QuizUpdateRequestDto quizDto, Guid moduleId);
    Task<Quiz?> DeleteAsync(Guid id, Guid moduleId);
    Task<Quiz?> ResourceByIdAsync(Guid quizId);
}
