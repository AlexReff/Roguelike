﻿using System;

// interface needed only in Unity to use GetComponent and such features for generic agents
namespace ReGoap.Core
{
    internal interface IReGoapAgentHelper
    {
        Type[] GetGenericArguments();
    }
}
