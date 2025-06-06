﻿namespace Library.Core.Entities;

public abstract class BaseClass
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}