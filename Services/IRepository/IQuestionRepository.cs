﻿using Quizpractice.Models;

namespace Quizpractice.Services.IRepository
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
       
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionWithAnswersAsync(int questionId);
        Task AddQuestionAsync(Question question);
    }


}

