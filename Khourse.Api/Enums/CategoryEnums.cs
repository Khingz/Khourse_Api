using System.ComponentModel.DataAnnotations;

namespace Khourse.Api.Enums;

public enum CourseCategory
{
    [Display(Name = "Web Development")]
    Programming,

    [Display(Name = "Graphic Design")]
    Design,

    [Display(Name = "Digital Marketing")]
    Marketing,

    [Display(Name = "Business Management")]
    Business,

    [Display(Name = "Finance & Accounting")]
    Finance,

    [Display(Name = "Health & Fitness")]
    Health,

    [Display(Name = "Music & Audio")]
    Music,

    [Display(Name = "Photography & Video")]
    Photography,

    [Display(Name = "Languages")]
    Language,

    [Display(Name = "Science & Tech")]
    Science,

    [Display(Name = "Engineering")]
    Engineering,

    [Display(Name = "Personal Development")]
    PersonalDevelopment,

    [Display(Name = "Lifestyle")]
    Lifestyle,

    [Display(Name = "Education & Teaching")]
    Education,

    [Display(Name = "Mathematics")]
    Mathematics,

    [Display(Name = "Exam & Test Prep")]
    TestPreparation
}
