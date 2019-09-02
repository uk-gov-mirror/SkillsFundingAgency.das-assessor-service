using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.AssessorService.Api.Types.Models.Validation;
using SFA.DAS.QnA.Api.Types.Page;

namespace SFA.DAS.AssessorService.Web.ViewModels.Apply
{
    public class PageViewModel
    {
        public Guid Id { get; }

        public PageViewModel(Guid Id, int sequenceNo, Guid sectionId, string pageId, Page page, string redirectAction, string returnUrl, List<ValidationErrorDetail> errorMessages)
        {
            this.Id = Id;
            SequenceNo = sequenceNo.ToString();
            SectionId = sectionId;
            PageId = pageId;
            RedirectAction = redirectAction;
            ReturnUrl = returnUrl;
            ErrorMessages = errorMessages;

            if (page != null)
            {
                SetupPage(page, errorMessages);
            }
        }

        public bool HasFeedback { get; set; }
        public List<Feedback> Feedback { get; set; }

        public string LinkTitle { get; set; }

        public string PageId { get; set; }
        public string Title { get; set; }

        public string DisplayType { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
        public string SequenceNo { get; set; }
        public Guid SectionId { get; set; }

        public bool AllowMultipleAnswers { get; set; }
        public List<PageOfAnswers> PageOfAnswers { get; set; }
        public string BodyText { get; set; }

        public PageDetails Details { get; set; }

        public string RedirectAction { get; set; }
        public string ReturnUrl { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        private void SetupPage(Page page, List<ValidationErrorDetail> errorMessages)
        {
            Title = page.Title;
            LinkTitle = page.LinkTitle;
            DisplayType = page.DisplayType;
            PageId = page.PageId;
            SequenceNo = SequenceNo;
            AllowMultipleAnswers = page.AllowMultipleAnswers;
            if (errorMessages != null && errorMessages.Any())
            {
                PageOfAnswers = page.PageOfAnswers.Take(page.PageOfAnswers.Count - 1).ToList();
            }
            else
            {
                PageOfAnswers = page.PageOfAnswers;
            }

            SectionId = SectionId;

            var questions = page.Questions;
            var answers = new List<Answer>();
            foreach(var pageAnswer in page.PageOfAnswers)
            {
                answers.AddRange(pageAnswer.Answers);
            }

            Questions = new List<QuestionViewModel>();

            Questions.AddRange(questions.Select(q => new QuestionViewModel()
            {
                Label = q.Label,
                ShortLabel = q.ShortLabel,
                QuestionBodyText = q.QuestionBodyText,
                QuestionId = q.QuestionId,
                Type = q.Input.Type,
                InputClasses = q.Input.InputClasses,
                Hint = q.Hint,
                Options = q.Input.Options,
                Value = page.AllowMultipleAnswers ? GetMultipleValue(page.PageOfAnswers.LastOrDefault()?.Answers, q, errorMessages) : answers?.SingleOrDefault(a => a?.QuestionId == q.QuestionId)?.Value,
                ErrorMessages = errorMessages?.Where(f => f.Field == q.QuestionId).ToList(),
                SequenceNo = int.Parse(SequenceNo),
                SectionId = SectionId,
                Id = Id,
                PageId = PageId,
                RedirectAction = RedirectAction
            }));

            Feedback = page.Feedback;
            HasFeedback = page.HasFeedback;
            BodyText = page.BodyText;

            Details = page.Details;

            foreach (var question in Questions)
            {
                if (question.Options == null) continue;
                foreach (var option in question.Options)
                {
                    if (option.FurtherQuestions == null) continue;
                    foreach (var furtherQuestion in option.FurtherQuestions)
                    {
                        furtherQuestion.Value = answers
                            ?.SingleOrDefault(a => a?.QuestionId == furtherQuestion.QuestionId.ToString())?.Value;
                    }
                }
            }
        }

        private string GetMultipleValue(List<Answer> answers, Question question, List<ValidationErrorDetail> errorMessages)
        {
            if (errorMessages != null && errorMessages.Any())
            {
                return answers?.LastOrDefault(a => a?.QuestionId == question.QuestionId)?.Value;
            }

            return null;
        }
    }
}