﻿using System;
using NJasmine.Core.FixtureVisitor;

namespace NJasmine.Core
{
    public abstract class SpecificationFixture 
    {
        internal TestPosition CurrentPosition { get; set; }
        internal ISpecPositionVisitor Visitor { get; set; }

        public SpecificationFixture()
        {
        }

        public void Run()
        {
            this.Specify();
        }

        public abstract void Specify();

        //
        //  Hmm, how to make sense of this?  It works.
        //
        
        protected T SetPositionForNestedCall_Run_Then_SetPositionForNextSibling<T>(Func<TestPosition, T> action)
        {
            T result = default(T);

            var position = CurrentPosition;
            var nextPosition = CurrentPosition.GetNextSiblingPosition();
            CurrentPosition = CurrentPosition.GetFirstChildPosition();
            result = action(position);
            CurrentPosition = nextPosition;

            return result;
        }

        protected void SetPositionForNestedCall_Run_Then_SetPositionForNextSibling(Action<TestPosition> action)
        {
            SetPositionForNestedCall_Run_Then_SetPositionForNextSibling<object>(tp =>
            {
                action(tp);
                return null;
            });
        }
    }
}