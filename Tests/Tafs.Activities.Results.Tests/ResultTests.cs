﻿//
//  ResultTests.cs
//
//  Author:
//       TAFS RPA Developers
//
//  Copyright (c) 2021 TAFS, LLC.. All rights reserved.
//

using System;
using Tafs.Activities.Results.Errors;
using Xunit;

#pragma warning disable CS0618

namespace Tafs.Activities.Results.Tests
{
    /// <summary>
    /// Tests the <see cref="Result"/> struct.
    /// </summary>
    public static class ResultTests
    {
        /// <summary>
        /// Tests the <see cref="Result.IsSuccess"/> property.
        /// </summary>
        public class IsSuccess
        {
            /// <summary>
            /// Tests whether <see cref="Result.IsSuccess"/> returns true on a successful result.
            /// </summary>
            [Fact]
            public void ReturnsTrueForSuccessfulResult()
            {
                var successful = Result.FromSuccess();
                Assert.True(successful.IsSuccess);
            }

            /// <summary>
            /// Tests whether <see cref="Result.IsSuccess"/> returns false on an unsuccessful result.
            /// </summary>
            [Fact]
            public void ReturnsFalseForUnsuccessfulResult()
            {
                var unsuccessful = Result.FromError(new GenericError("Dummy error"));
                Assert.False(unsuccessful.IsSuccess);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.Inner"/> property.
        /// </summary>
        public class Inner
        {
            /// <summary>
            /// Tests whether <see cref="Result.Inner"/> returns null if no wrapped result exists.
            /// </summary>
            [Fact]
            public void ReturnsNullIfNoWrappedResultExists()
            {
                var plainResult = Result.FromError(new GenericError("Dummy error"));
                Assert.Null(plainResult.Inner);
            }

            /// <summary>
            /// Tests whether <see cref="Result.Inner"/> returns a valid object if a wrapped result exists.
            /// </summary>
            [Fact]
            public void ReturnsObjectIfWrappedResultExists()
            {
                var wrapped = Result.FromError(new GenericError("Dummy wrapped"));
                var plainResult = Result.FromError(new GenericError("Wrapping"), wrapped);

                Assert.NotNull(plainResult.Inner);
            }

            /// <summary>
            /// Tests whether <see cref="Result.Inner"/> returns the correct object if a wrapped result exists.
            /// </summary>
            [Fact]
            public void ReturnsCorrectObjectIfWrappedResultExists()
            {
                var wrapped = Result.FromError(new GenericError("Dummy wrapped"));
                var plainResult = Result.FromError(new GenericError("Wrapping"), wrapped);

                Assert.Equal(wrapped, plainResult.Inner);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.Error"/> property.
        /// </summary>
        public class Error
        {
            /// <summary>
            /// Tests whether <see cref="Result.Error"/> returns null if the result is successful.
            /// </summary>
            [Fact]
            public void ReturnsNullIfResultIsSuccessful()
            {
                var successful = Result.FromSuccess();
                Assert.Null(successful.Error);
            }

            /// <summary>
            /// Tests whether <see cref="Result.Error"/> returns an object if the result is unsuccessful.
            /// </summary>
            [Fact]
            public void ReturnsObjectIfResultIsUnsuccessful()
            {
                var unsuccessful = Result.FromError(new GenericError("Dummy error"));
                Assert.NotNull(unsuccessful.Error);
            }

            /// <summary>
            /// Tests whether <see cref="Result.Error"/> returns the correct object if the result is unsuccessful.
            /// </summary>
            [Fact]
            public void ReturnsCorrectObjectIfResultIsUnsuccessful()
            {
                var expected = new GenericError("Dummy error");
                var unsuccessful = Result.FromError(expected);

                Assert.Same(expected, unsuccessful.Error);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.FromSuccess"/> method.
        /// </summary>
        public class FromSuccess
        {
            /// <summary>
            /// Tests whether <see cref="Result.FromSuccess"/> creates a successful result.
            /// </summary>
            [Fact]
            public void CreatesASuccessfulResult()
            {
                var successful = Result.FromSuccess();
                Assert.True(successful.IsSuccess);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.FromError{TError}(TError)"/> method and its overloads.
        /// </summary>
        public class FromError
        {
            /// <summary>
            /// Tests whether <see cref="Result.FromSuccess"/> creates an unsuccessful result from a plain error
            /// instance.
            /// </summary>
            [Fact]
            public void CreatesAnUnsuccessfulResultFromAnErrorInstance()
            {
                var result = Result.FromError(new GenericError("Dummy error"));
                Assert.False(result.IsSuccess);
            }

            /// <summary>
            /// Tests whether <see cref="Result.FromSuccess"/> creates an unsuccessful result from a plain error
            /// instance and a wrapped result.
            /// </summary>
            [Fact]
            public void CreatesAnUnsuccessfulResultFromAnErrorInstanceAndAWrappedResult()
            {
                var wrapped = Result.FromError(new GenericError("Dummy error."));
                var result = Result.FromError(new GenericError("Dummy error"), wrapped);

                Assert.False(result.IsSuccess);
                Assert.NotNull(result.Inner);
            }

            /// <summary>
            /// Tests whether <see cref="Result.FromSuccess"/> creates an unsuccessful result from another result type.
            /// </summary>
            [Fact]
            public void CreatesAnUnsuccessfulResultFromAnotherResult()
            {
                var wrapped = Result<int>.FromError(new GenericError("Dummy error."));
                var result = Result.FromError(wrapped);

                Assert.False(result.IsSuccess);
                Assert.NotNull(result.Inner);
                Assert.IsType<GenericError>(result.Error);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.op_Implicit(ResultError)"/> operator.
        /// </summary>
        public class ResultErrorOperator
        {
            /// <summary>
            /// Tests whether <see cref="Result.op_Implicit(ResultError)"/> creates an unsuccessful result from a plain
            /// error instance.
            /// </summary>
            [Fact]
            public void CreatesAnUnsuccessfulResultFromAnErrorInstance()
            {
                Result result = new GenericError("Dummy error");
                Assert.False(result.IsSuccess);
            }
        }

        /// <summary>
        /// Tests the <see cref="Result.op_Implicit(Exception)"/> operator.
        /// </summary>
        public class ExceptionOperator
        {
            /// <summary>
            /// Tests whether <see cref="Result.op_Implicit(Exception)"/> creates an unsuccessful result from an
            /// exception instance.
            /// </summary>
            [Fact]
            public void CreatesAnUnsuccessfulResultFromAnExceptionInstance()
            {
                Result result = new Exception("Dummy error");

                Assert.False(result.IsSuccess);
                Assert.IsType<ExceptionError>(result.Error);
            }
        }
    }
}
