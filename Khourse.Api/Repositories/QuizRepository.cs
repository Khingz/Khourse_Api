using Khourse.Api.Data;
using Khourse.Api.Dtos.QuizDtos;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class QuizRepository(AppDbContext dbContext) : IQuizRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Quiz> CreateAsync(Quiz quiz)
    {
        await _dbContext.Quiz.AddAsync(quiz);
        await _dbContext.SaveChangesAsync();
        return quiz;
    }

    public async Task<Quiz?> DeleteAsync(Guid id, Guid moduleId)
    {
        var quiz = await _dbContext.Quiz.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Quiz not found!");
        _dbContext.Quiz.Remove(quiz);
        await _dbContext.SaveChangesAsync();
        return quiz;
    }

    public async Task<List<Quiz>> GetAllAsync(Guid moduleId)
    {
        return await _dbContext.Quiz
            .Where(q => q.ModuleId == moduleId)
            .ToListAsync();
    }

    public async Task<Quiz?> GetByIdAsync(Guid quizId, Guid moduleId)
    {
        var quiz = await _dbContext.Quiz.FirstOrDefaultAsync(r => r.Id == quizId && r.ModuleId == moduleId) ?? throw new KeyNotFoundException("Quiz not found!");
        return quiz;
    }

    public async Task<Quiz?> ResourceByIdAsync(Guid quizId)
    {
        var quiz = await _dbContext.Quiz.FirstOrDefaultAsync(r => r.Id == quizId) ?? throw new KeyNotFoundException("Quiz not found!");
        return quiz;
    }

    public async Task<Quiz?> UpdateAsync(Guid id, QuizUpdateRequestDto quizDto, Guid moduleId)
    {
        var quiz = await _dbContext.Quiz.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Quiz not found!");
        UpdateQuizFields(quiz, quizDto);
        await _dbContext.SaveChangesAsync();
        return quiz;
    }

    private static void UpdateQuizFields(Quiz quiz, QuizUpdateRequestDto updateDto)
    {
        quiz.Title = string.IsNullOrWhiteSpace(updateDto.Title)
            ? quiz.Title
            : updateDto.Title;
        quiz.Instructions = string.IsNullOrWhiteSpace(updateDto.Instructions)
            ? quiz.Instructions
            : updateDto.Instructions;
        quiz.Grade = string.IsNullOrWhiteSpace(updateDto.Grade)
            ? quiz.Grade
            : updateDto.Grade;
        quiz.PassingScore = updateDto.PassingScore ?? quiz.PassingScore;
        quiz.TimeLimitMins = updateDto.TimeLimitMins ?? quiz.TimeLimitMins;

    }
}
