using System;
using Khourse.Api.Data;
using Khourse.Api.Dtos.LessonDtos;
using Khourse.Api.Models;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Khourse.Api.Repositories;

public class LessonRepository(AppDbContext dbContext) : ILessonRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<Lesson> CreateAsync(Lesson lesson)
    {
        await _dbContext.Lesson.AddAsync(lesson);
        await _dbContext.SaveChangesAsync();
        return lesson;
    }

    public async Task<Lesson?> DeleteAsync(Guid id, Guid moduleId)
    {
        var lesson = await _dbContext.Lesson.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Lesson not found!");
        _dbContext.Lesson.Remove(lesson);
        await _dbContext.SaveChangesAsync();
        return lesson;
    }

    public async Task<List<Lesson>> GetAllAsync(Guid moduleId)
    {
        return await _dbContext.Lesson
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.Position)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(Guid lessonId, Guid moduleId)
    {
        var lesson = await _dbContext.Lesson.FirstOrDefaultAsync(l => l.Id == lessonId && l.ModuleId == moduleId) ?? throw new KeyNotFoundException("Lesson not found!");
        return lesson;
    }

    public async Task<Lesson?> LessonByIdAsync(Guid lessonId)
    {
        var module = await _dbContext.Lesson.FirstOrDefaultAsync(m => m.Id == lessonId) ?? throw new KeyNotFoundException("Lesson not found!");
        return module;
    }

    public async Task<Lesson?> UpdateAsync(Guid id, UpdateLessonRequestDto lessonDto, Guid moduleId)
    {
        var lesson = await _dbContext.Lesson.FirstOrDefaultAsync(x => x.Id == id && x.ModuleId == moduleId) ?? throw new KeyNotFoundException("Lesson not found!");
        UpdateLessonFields(lesson, lessonDto);
        await _dbContext.SaveChangesAsync();
        return lesson;
    }

    private static void UpdateLessonFields(Lesson module, UpdateLessonRequestDto updateDto)
    {

    }
}
