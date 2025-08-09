using System;
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
        };
    }

    public static Quiz ToQuizEntity(this CreateQuizDto quizDto)
    {
        return new Quiz
        {

        };
    }
}
