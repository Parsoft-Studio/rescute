﻿using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Infrastructure;
using rescute.Shared;
using rescute.Tests.InfrastructureTests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    // [Collection("Database collection")]
    public class SamaritanRepositoryTests
    {
        [Fact]
        public async void SamaritanRepositoryAddsAndGetsSamaritan()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.GetTestDatabaseOptions()))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = TestUtility.RandomTestSamaritan();

                    unitOfWork.Samaritans.Add(samaritan);
                    await unitOfWork.Complete();
                    var same = await unitOfWork.Samaritans.GetAsync(samaritan.Id);


                    same.Should().NotBe(null);
                    same.Should().Be(samaritan);
                }
            }
        }

    }
}
