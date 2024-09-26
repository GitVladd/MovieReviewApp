﻿using System.ComponentModel.DataAnnotations;

namespace ReviewService.Attributes
{
    public class NonEmptyListAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var list = value as IList<Guid>;
            return list != null && list.Count > 0;
        }
    }
}