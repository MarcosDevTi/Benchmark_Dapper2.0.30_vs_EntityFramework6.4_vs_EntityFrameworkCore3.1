﻿using System;

namespace EntityFrameworkVsCoreDapper.Contracts
{
    public interface IEf6Service
    {
        TimeSpan InsertSingleProducts(int interactions);
        TimeSpan InsertComplexCustomers(int interactions);

        TimeSpan SelectComplexCustomers(int take);
        TimeSpan SelectSingleProducts(int take);
    }
}
