﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quizpractice.Models;

namespace Quizpractice.Pages.Quizs
{
    public class QuizDetailModel : PageModel
    {
        private readonly SWP391_DBContext _context;

        public QuizDetailModel(SWP391_DBContext context)
        {
            _context = context;
        }

        public List<QuizDetailViewModel> PaginatedResults { get; set; } = new List<QuizDetailViewModel>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;

        public void OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage > 0 ? currentPage : 1;

            // Logic phân trang
            var allResultsQuery = _context.QuizDetails
                .OrderByDescending(q => q.TakenDate)
                .Select(q => new QuizDetailViewModel
                {
                    QuizId = q.QuizId,
                    SubjectId = q.Quiz.SubId,
                    // Đảm bảo rằng Score được chuyển thành double, ngay cả khi trong cơ sở dữ liệu là int
                    Score = Convert.ToDouble(q.Score),  // Chuyển Score thành double rõ ràng
                    TakenDate = q.TakenDate,
                    Attempt = _context.QuizDetails
                        .Where(d => d.UserId == q.UserId && d.QuizId == q.QuizId)
                        .Count()
                });

            var totalCount = allResultsQuery.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            PaginatedResults = allResultsQuery
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public class QuizDetailViewModel
        {
            public int QuizId { get; set; }
            public int? SubjectId { get; set; }
            public double Score { get; set; }

            // Tính toán phần trăm với xử lý kiểu double
            public double Percentage => (Score / 10.0) * 100;  // Đảm bảo Score được xử lý là double

            public int Attempt { get; set; }
            public System.DateTime TakenDate { get; set; }
        }

    }
}
