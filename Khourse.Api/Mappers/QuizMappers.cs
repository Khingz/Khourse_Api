using Khourse.Api.Dtos.QuizDtos;
using Khourse.Api.Models;

namespace Khourse.Api.Mappers;

public static class QuizMappers
{
    public static QuizDto ToQuizDto(this Quiz quiz)
    {
        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Instructions = quiz.Instructions,
            PassingScore = quiz.PassingScore,
            TimeLimitMins = quiz.TimeLimitMins,
            IsPublished = quiz.IsPublished,
            Grade = quiz.Grade,
            ModuleId = quiz.ModuleId!.Value,
            Quizquestions = [.. quiz.Questions],
            CreatedAt = quiz.CreatedAt
        };
    }

    public static Quiz ToQuizEntity(this CreateQuizDto quizDto, Guid moduleId)
    {
        return new Quiz
        {
            Title = quizDto.Title,
            Instructions = quizDto.Instructions,
            PassingScore = quizDto.PassingScore,
            TimeLimitMins = quizDto.TimeLimitMins,
            ModuleId = moduleId!,
        };
    }
}
