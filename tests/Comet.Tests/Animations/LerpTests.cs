﻿using System;
using Xunit;

namespace Comet.Tests
{
    public class LerpTests : TestBase
    {
		[Fact]
        public void DoubleLerps()
        {
            double start = 0;
            double end = 1;
            var lerp = Lerp.GetLerp(start.GetType());
            var progress = .1;
            var value = (double)lerp.Calculate(progress, start, end);
            Assert.Equal(progress,value);

            progress = .2;
            value = (double)lerp.Calculate(progress, start, end);
            Assert.Equal(progress, value);


            progress = .33333;
            value = (double)lerp.Calculate(progress, start, end);
            Assert.Equal(progress, value);


            progress = .5;
            value = (double)lerp.Calculate(progress, start, end);
            Assert.Equal(progress, value);


            progress = 1;
            value = (double)lerp.Calculate(progress, start, end);
            Assert.Equal(progress, value);

        }

        [Fact]
        public void BoolLerps()
        {
            bool start = false;
            bool end = true;

            var lerp = Lerp.GetLerp(start.GetType());
            var progress = .1;
            var value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(false, value);

            progress = .2;
            value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(false, value);


            progress = .4999999;
            value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(false, value);


            progress = .5;
            value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(true, value);

            progress = .75;
            value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(true, value);

            progress = 1;
            value = (bool)lerp.Calculate(progress, start, end);
            Assert.Equal(true, value);

        }
    }
}
