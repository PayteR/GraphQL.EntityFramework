﻿using System;

public class Level2Entity
{
    public Guid Id { get; set; }
    public Guid? Level3EntityId { get; set; }
    public Level3Entity Level3Entity { get; set; }
}