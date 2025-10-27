using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Utility class providing common awaitable helpers similar to Task.WhenAll,
    /// but for custom <c>Awaitable</c> types used in the project.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public static partial class AwaitableUtility
    {
        /// <summary>
        /// Awaits all awaitables in the provided list. If one or more throw,
        /// completes after all have finished and then throws an <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="awaitables">Awaitables to await.</param>
        /// <exception cref="AggregateException">
        /// Thrown after all awaitables complete if any of them faulted. Contains all collected exceptions.
        /// </exception>
        public static async Awaitable WhenAll(IList<Awaitable> awaitables)
        {
            using (ListPool<Exception>.Get(out var exceptions))
            {
                await CaptureExceptionsAsync(awaitables, exceptions);

                if (exceptions.Count > 0)
                {
                    throw CreateAggregateException(exceptions);
                }
            }
        }

        /// <summary>
        /// Awaits both awaitables. Aggregates exceptions like <see cref="Task.WhenAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <param name="awaitable1">First awaitable.</param>
        /// <param name="awaitable2">Second awaitable.</param>
        /// <exception cref="AggregateException">If one or both faulted.</exception>
        public static async Awaitable WhenAll(Awaitable awaitable1, Awaitable awaitable2)
        {
            using (ListPool<Awaitable>.Get(out var awaitables))
            {
                awaitables.Add(awaitable1);
                awaitables.Add(awaitable2);

                await WhenAll(awaitables);
            }
        }

        /// <summary>
        /// Awaits three awaitables. Aggregates exceptions like <see cref="Task.WhenAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <param name="awaitable1">First awaitable.</param>
        /// <param name="awaitable2">Second awaitable.</param>
        /// <param name="awaitable3">Third awaitable.</param>
        /// <exception cref="AggregateException">If any faulted.</exception>
        public static async Awaitable WhenAll(Awaitable awaitable1, Awaitable awaitable2, Awaitable awaitable3)
        {
            using (ListPool<Awaitable>.Get(out var awaitables))
            {
                awaitables.Add(awaitable1);
                awaitables.Add(awaitable2);
                awaitables.Add(awaitable3);

                await WhenAll(awaitables);
            }
        }

        /// <summary>
        /// Awaits four awaitables. Aggregates exceptions like <see cref="Task.WhenAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <param name="awaitable1">First awaitable.</param>
        /// <param name="awaitable2">Second awaitable.</param>
        /// <param name="awaitable3">Third awaitable.</param>
        /// <param name="awaitable4">Fourth awaitable.</param>
        /// <exception cref="AggregateException">If any faulted.</exception>
        public static async Awaitable WhenAll(Awaitable awaitable1, Awaitable awaitable2, Awaitable awaitable3, Awaitable awaitable4)
        {
            using (ListPool<Awaitable>.Get(out var awaitables))
            {
                awaitables.Add(awaitable1);
                awaitables.Add(awaitable2);
                awaitables.Add(awaitable3);
                awaitables.Add(awaitable4);

                await WhenAll(awaitables);
            }
        }

        /// <summary>
        /// Awaits a value-returning awaitable and a non-returning awaitable in parallel.
        /// Propagates all exceptions after both complete and returns the first result.
        /// </summary>
        /// <typeparam name="TResult">Result type of the first awaitable.</typeparam>
        /// <param name="awaitable1">Value-returning awaitable.</param>
        /// <param name="awaitable2">Non-returning awaitable.</param>
        /// <returns>The result of <paramref name="awaitable1"/>.</returns>
        /// <exception cref="AggregateException">If either awaitable faulted.</exception>
        public static async Awaitable<TResult> WhenAll<TResult>(Awaitable<TResult> awaitable1, Awaitable awaitable2)
        {
            using (ListPool<Exception>.Get(out var exceptions))
            {
                Optional<TResult> result = await CaptureExceptionsAsync(awaitable1, exceptions);
                await CaptureExceptionsAsync(awaitable2, exceptions);

                if (exceptions.Count > 0)
                {
                    throw CreateAggregateException(exceptions);
                }
                
                return result.GetValueUnsafe();
            }
        }

        /// <summary>
        /// Awaits two value-returning awaitables in parallel and returns both results as a tuple.
        /// Aggregates exceptions like <see cref="Task.WhenAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <typeparam name="TResult1">Result type of the first awaitable.</typeparam>
        /// <typeparam name="TResult2">Result type of the second awaitable.</typeparam>
        /// <param name="awaitable1">First value-returning awaitable.</param>
        /// <param name="awaitable2">Second value-returning awaitable.</param>
        /// <returns>Tuple of both results <c>(result1, result2)</c>.</returns>
        /// <exception cref="AggregateException">If either awaitable faulted.</exception>
        public static async Awaitable<(TResult1, TResult2)> WhenAll<TResult1, TResult2>(Awaitable<TResult1> awaitable1, Awaitable<TResult2> awaitable2)
        {
            using (ListPool<Exception>.Get(out var exceptions))
            {
                Optional<TResult1> result1 = await CaptureExceptionsAsync(awaitable1, exceptions);
                Optional<TResult2> result2 = await CaptureExceptionsAsync(awaitable2, exceptions);

                if (exceptions.Count > 0)
                {
                    throw CreateAggregateException(exceptions);
                }
                
                return (result1.GetValueUnsafe(), result2.GetValueUnsafe());
            }
        }

        /// <summary>
        /// Awaits three value-returning awaitables in parallel and returns all results as a tuple.
        /// Aggregates exceptions like <see cref="Task.WhenAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <typeparam name="TResult1">Result type of the first awaitable.</typeparam>
        /// <typeparam name="TResult2">Result type of the second awaitable.</typeparam>
        /// <typeparam name="TResult3">Result type of the third awaitable.</typeparam>
        /// <param name="awaitable1">First value-returning awaitable.</param>
        /// <param name="awaitable2">Second value-returning awaitable.</param>
        /// <param name="awaitable3">Third value-returning awaitable.</param>
        /// <returns>Tuple of results <c>(result1, result2, result3)</c>.</returns>
        /// <exception cref="AggregateException">If any awaitable faulted.</exception>
        public static async Awaitable<(TResult1, TResult2, TResult3)> WhenAll<TResult1, TResult2, TResult3>(Awaitable<TResult1> awaitable1, Awaitable<TResult2> awaitable2, Awaitable<TResult3> awaitable3)
        {
            using (ListPool<Exception>.Get(out var exceptions))
            {
                Optional<TResult1> result1 = await CaptureExceptionsAsync(awaitable1, exceptions);
                Optional<TResult2> result2 = await CaptureExceptionsAsync(awaitable2, exceptions);
                Optional<TResult3> result3 = await CaptureExceptionsAsync(awaitable3, exceptions);

                if (exceptions.Count > 0)
                {
                    throw CreateAggregateException(exceptions);
                }
                
                return (result1.GetValueUnsafe(), result2.GetValueUnsafe(), result3.GetValueUnsafe());
            }
        }

        /// <summary>
        /// Convenience overload: awaits a non-returning awaitable and a value-returning awaitable,
        /// returning the latter's result. Equivalent to calling the typed overload with reversed arguments.
        /// </summary>
        /// <typeparam name="TResult">Result type of <paramref name="awaitable2"/>.</typeparam>
        /// <param name="awaitable1">Non-returning awaitable.</param>
        /// <param name="awaitable2">Value-returning awaitable.</param>
        /// <returns>The result of <paramref name="awaitable2"/>.</returns>
        /// <exception cref="AggregateException">If either awaitable faulted.</exception>
        public static Awaitable<TResult> WhenAll<TResult>(Awaitable awaitable1, Awaitable<TResult> awaitable2)
            => WhenAll(awaitable2, awaitable1);

        // -------- Private helpers (no public XML needed) --------

        private static async Awaitable CaptureExceptionsAsync(IList<Awaitable> awaitables, IList<Exception> exceptions)
        {
            foreach (Awaitable awaitable in awaitables)
            {
                await CaptureExceptionsAsync(awaitable, exceptions);
            }
        }

        private static async Awaitable CaptureExceptionsAsync(Awaitable awaitable, IList<Exception> exceptions)
        {
            try
            {
                await awaitable;
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
        }

        private static async Awaitable<Optional<TResult>> CaptureExceptionsAsync<TResult>(Awaitable<TResult> awaitable, IList<Exception> exceptions)
        {
            try
            {
                return await awaitable;
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
            
            return new None();
        }

        private static Exception CreateAggregateException(IList<Exception> exceptions)
        {
            if (!exceptions.Any(static e => e is OperationCanceledException))
            {
                return new AggregateException(exceptions).Flatten();
            }

            if (exceptions.All(static e => e is OperationCanceledException))
            {
                return new AggregateException(exceptions[0]);
            }

            return new AggregateException(exceptions.Where(static e => e is not OperationCanceledException)).Flatten();
        }

        /// <summary>
        /// Sentinel struct representing absence of a value for internal optional flow.
        /// </summary>
        readonly struct None { }

        /// <summary>
        /// Lightweight optional wrapper used to carry a value or "none" without allocations.
        /// </summary>
        /// <typeparam name="T">Wrapped value type.</typeparam>
        readonly struct Optional<T>
        {
            private readonly bool _HasValue;
            private readonly T? _Value;

            /// <summary>True if a value is present.</summary>
            public readonly bool HasValue => _HasValue;

            /// <summary>True if no value is present.</summary>
            public readonly bool IsNone => !HasValue;

            /// <summary>Implicitly creates an <see cref="Optional{T}"/> containing a value.</summary>
            public static implicit operator Optional<T>(T value) => new(value, true);

            /// <summary>Implicitly creates an empty <see cref="Optional{T}"/>.</summary>
            public static implicit operator Optional<T>(None _) => new(default, false);

            /// <summary>
            /// Returns the contained value or throws if none is present.
            /// </summary>
            /// <exception cref="InvalidOperationException">If no value is present.</exception>
            public readonly T GetValueUnsafe() => _Value ?? throw new InvalidOperationException("Optional has no value to retrieve.");

            public readonly override string ToString() => HasValue ? $"Some({_Value})" : "None";

            // Private
            private Optional(T? value, bool hasValue)
            {
                _Value = value;
                _HasValue = hasValue;
            }
        }
    }
}
